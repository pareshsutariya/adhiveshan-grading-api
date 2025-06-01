using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IUsersService
{
    Task<ParticipantModel> GetByBAPSIdToAddAsUser(string participantBapsId, string loginUserBapsId);
    Task<List<UserModel>> GetUsersForLoginUser(string loginUserBapsId);
    Task<UserModel> Get(int id);
    UserModel Create(UserCreateModel createModel);
    void Update(int id, UserUpdateModel updateModel);
    Task Remove(int id);
    Task<bool> JudgesImport(string loginUserBapsId, List<UserJudgeImport> models);
}

public class UsersService : BaseService, IUsersService
{
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<CompetitionEvent> _EventsCollection;
    private readonly IMongoCollection<Participant> _participantsCollection;
    private readonly IMongoCollection<CompetitionEvent> _competitionEventsCollection;

    public UsersService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _EventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
        _competitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
    }

    public async Task<ParticipantModel> GetByBAPSIdToAddAsUser(string participantBapsId, string loginUserBapsId)
    {
        var participant = await _participantsCollection.Find(item => item.BAPSId == participantBapsId).FirstOrDefaultAsync();
        var loginUser = await _UsersCollection.Find(item => item.BAPSId == loginUserBapsId).FirstOrDefaultAsync();

        var loginUserEvents = await _EventsCollection.Find(item => loginUser.AssignedEventIds.Contains(item.CompetitionEventId)).ToListAsync();

        if (participant == null)
            throw new ApplicationException($"Participant not found for BAPS Id: {participantBapsId}");

        var existingUser = await _UsersCollection.Find(item => item.BAPSId == participant.BAPSId).FirstOrDefaultAsync();
        if (existingUser != null)
            throw new ApplicationException($"BAPS Id: {participantBapsId}. Participant {participant.FirstName} {participant.LastName}'s is already added as a User");

        var events = await _competitionEventsCollection.Find(item => loginUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();
        if (!events.Any())
            throw new ApplicationException($"Judge {loginUser.FullName}'s assigned Competition Events are Not Active. Hence cannot add user.");

        var eventCenters = string.Join(", ", events.SelectMany(e => e.Centers));
        if (!events.SelectMany(e => e.Centers).Contains(participant.Center) && !events.SelectMany(e => e.Centers).Contains(participant.HostCenter ?? ""))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' Center ({participant.Center}) OR asigned Host Center({participant.HostCenter ?? ""}) is not matching with judge's assigned events' center: {eventCenters}");

        if (!loginUser.AssignedGenders.Contains(participant.Gender))
            throw new ApplicationException($"BAPS Id: {participantBapsId}. Participant {participant.FirstName} {participant.LastName}'s gender ({participant.Gender}) is not matching with login user {loginUser.FullName}'s gender");


        return participant?.Map<ParticipantModel>(mapper);
    }

    public async Task<List<UserModel>> GetUsersForLoginUser(string loginUserBapsId)
    {
        var loginUser = await _UsersCollection.Find(item => item.BAPSId == loginUserBapsId).FirstOrDefaultAsync();

        if (loginUser == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {loginUserBapsId}");

        if (!loginUser.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to login user {loginUser.FullName}");

        var entities = await _UsersCollection.Find(item => true).ToListAsync();
        var models = entities.Select(c => c.Map<UserModel>(mapper)).OrderBy(c => c.FullName).ToList();

        // If login user is not a National Admin
        if (!loginUser.AssignedRoles.Contains("National Admin"))
        {
            models = entities.Where(c =>
                                // If login user is not a National Admin, filter for gender
                                loginUser.AssignedGenders.Intersect(c.AssignedGenders).Any() &&
                                // If login user is not a National Admin, filter for assinged events
                                loginUser.AssignedEventIds.Intersect(c.AssignedEventIds).Any())
                             .Select(c => c.Map<UserModel>(mapper))
                             .OrderBy(c => c.FullName)
                             .ToList();
        }

        var events = await _EventsCollection.Find(item => true).ToListAsync();
        foreach (var user in models)
        {
            if (user.AssignedEventIds?.Count > 0 && events?.Count > 0)
                user.AssignedEvents = events?.Where(c => user.AssignedEventIds.Contains(c.CompetitionEventId))
                                            .Select(c => c.Map<CompetitionEventModel>(mapper)).ToList();
        }

        return models;
    }

    public async Task<UserModel> Get(int id)
    {
        var entity = await _UsersCollection.Find<User>(item => item.UserId == id).FirstOrDefaultAsync();

        var model = entity.Map<UserModel>(mapper);

        return model;
    }

    public UserModel Create(UserCreateModel createModel)
    {
        var maxId = _UsersCollection.Find(c => true).SortByDescending(c => c.UserId).FirstOrDefault()?.UserId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var entity = createModel.Map<User>(mapper);
        entity.UserId = (maxId.Value + 1);

        _UsersCollection.InsertOne(entity);

        var model = entity.Map<UserModel>(mapper);

        return model;
    }

    public void Update(int id, UserUpdateModel updateModel) =>
        _UsersCollection.ReplaceOne(item => item.UserId == id, updateModel.Map<User>(mapper));

    public async Task Remove(int id) =>
        await _UsersCollection.UpdateOneAsync(Builders<User>.Filter.Eq(s => s.UserId, id),
                                                 Builders<User>.Update.Set(s => s.Status, "Deleted"));

    public async Task<bool> JudgesImport(string loginUserBapsId, List<UserJudgeImport> models)
    {
        var errors = new List<string>();

        var loginUser = await _UsersCollection.Find(item => item.BAPSId == loginUserBapsId).FirstOrDefaultAsync();

        if (loginUser == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {loginUserBapsId}");

        if (!loginUser.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to login user {loginUser.FullName}");

        var loginUserEvents = await _EventsCollection.Find(item => loginUser.AssignedEventIds.Contains(item.CompetitionEventId)).ToListAsync();

        foreach (var model in models)
        {
            var participant = await _participantsCollection.Find(item => item.BAPSId == model.BAPSId).FirstOrDefaultAsync();

            if (participant == null)
            {
                errors.Add($"Participant not found for BAPS Id: {model.BAPSId}");
                continue;
            }

            if (!loginUser.AssignedGenders.Contains(participant.Gender))
            {
                errors.Add($"BAPS Id: {model.BAPSId}. Participant {participant.FirstName} {participant.LastName}'s gender ({participant.Gender}) is not allowed to import");
                continue;
            }

            if (!DateTime.TryParse(model.EventDate, out DateTime _eventDate))
            {
                errors.Add($"BAPS Id: {model.BAPSId}. Participant {participant.FirstName} {participant.LastName}. Given Event Date ({model.EventDate}) is invalid");
                continue;
            }

            var assignedEvent = loginUserEvents.FirstOrDefault(evt => evt.StartDate.ToString("yyyy-MM-dd") == _eventDate.ToString("yyyy-MM-dd"));
            if (assignedEvent == null)
            {
                errors.Add($"BAPS Id: {model.BAPSId}. Participant {participant.FirstName} {participant.LastName}. Given Event Date ({model.EventDate}) is not matching");
                continue;
            }

            // If user already exists, delete it
            var existingUser = await _UsersCollection.Find(item => item.BAPSId == participant.BAPSId).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                _UsersCollection.DeleteOne(c => c.UserId == existingUser.UserId);
            }

            // Add Judge user
            Create(new UserCreateModel
            {
                BAPSId = participant.BAPSId,
                Password = participant.BAPSId,
                FullName = $"{participant.FirstName} {participant.LastName}",
                Region = participant.Region,
                Center = participant.Center,
                Status = "Active",
                AssignedGenders = new List<string> { participant.Gender },
                AssignedRoles = new List<string> { "Judge" },
                AssignedSkillCategories = model.AssignedSkillCategories.Split(",").Select(c => c.Trim()).ToList(),
                AssignedEventIds = new List<int> { assignedEvent.CompetitionEventId },
            });
        }

        if (errors.Count > 0)
        {
            throw new ApplicationException(string.Join(" | ", errors));
        }

        return true;

    }
}
