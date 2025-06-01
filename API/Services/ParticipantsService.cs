using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IParticipantsService
{
    Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "");
    Task<List<ParticipantModel>> GetParticipantsForLoginUser(string loginUserBapsId);
    Task<ParticipantModel> GetByMISId(int misId);
    Task<ParticipantModel> GetByBAPSId(string bapsId);
    Task<List<ParticipantModel>> GetParticipantsForEvent(int eventId, string gender);
    Task<ParticipantModel> GetParticipantForJudging(string bapsId, int judgeUserId);
    Task<ParticipantModel> GetParticipantForCheckIn(string bapsId, int loginUserId);
    Task<ParticipantModel> UpdateHostCenter(ParticipantUpdateHostCenterModel model);
    Task<List<ParticipantModel>> Import(List<ParticipantModel> models);
}

public class ParticipantsService : BaseService, IParticipantsService
{
    private readonly IMongoCollection<Participant> _participantsCollection;
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<CompetitionEvent> _competitionEventsCollection;

    public ParticipantsService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
        _competitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
        _usersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
    }

    public async Task<ParticipantModel> GetByMISId(int misId)
    {
        var entity = await _participantsCollection.Find(item => item.MISId == misId).FirstOrDefaultAsync();

        return entity?.Map<ParticipantModel>(mapper);
    }

    public async Task<ParticipantModel> GetByBAPSId(string bapsId)
    {
        var entity = await _participantsCollection.Find(item => item.BAPSId == bapsId).FirstOrDefaultAsync();

        return entity?.Map<ParticipantModel>(mapper);
    }

    public async Task<List<ParticipantModel>> GetParticipantsForEvent(int eventId, string gender)
    {
        var compEvent = await _competitionEventsCollection.Find(item => item.CompetitionEventId == eventId).FirstOrDefaultAsync();
        if (compEvent == null)
            throw new ApplicationException($"Competition Event not foud for the given eventId: {eventId}");

        var participants = await _participantsCollection.Find(item => item.Gender == gender &&
                                                        (item.Speech_Pravachan_Category != null ||
                                                        item.Emcee_Category != null) &&
                                                        (compEvent.Centers.Contains(item.Center) ||
                                                        compEvent.Centers.Contains(item.HostCenter))
                                                        ).ToListAsync();

        return participants.Select(c => c.Map<ParticipantModel>(mapper)).ToList();
    }

    public async Task<ParticipantModel> GetParticipantForJudging(string bapsId, int judgeUserId)
    {
        // Get Judge
        var judgeUser = await _usersCollection.Find(item => item.UserId == judgeUserId).FirstOrDefaultAsync();
        if (judgeUser == null)
            throw new ApplicationException($"Judge not found");

        if (judgeUser.BAPSId == bapsId)
            throw new ApplicationException($"Judges are not allowed to grade themselves");

        // Judge Events
        if (!judgeUser.AssignedEventIds.Any())
            throw new ApplicationException($"Judge {judgeUser.FullName} is not assigned any Competition Event");

        // Active Events
        var events = await _competitionEventsCollection.Find(item => judgeUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();
        if (!events.Any())
            throw new ApplicationException($"Judge {judgeUser.FullName}'s assigned Competition Events are Not Active");

        // Get participant by MIS Id
        var participant = await _participantsCollection.Find(item => item.BAPSId == bapsId).FirstOrDefaultAsync();
        if (participant == null)
            throw new ApplicationException($"Participant not found for the given BAPS Id: {bapsId}");

        // Judge assigned Genders
        if (!judgeUser.AssignedGenders.Contains(participant.Gender))
            throw new ApplicationException($"Judge {judgeUser.FullName} is not now allowed to do judging for {participant.Gender} gender");

        // Judge assigned Skills
        if (judgeUser.AssignedSkillCategories == null || !judgeUser.AssignedSkillCategories.Any())
            throw new ApplicationException($"Judge {judgeUser.FullName} hasn't assigned any skills for judging");

        // Event Center vs Participant Center or HostCenter
        var eventCenters = string.Join(", ", events.SelectMany(e => e.Centers));
        if (!events.SelectMany(e => e.Centers).Contains(participant.Center) && !events.SelectMany(e => e.Centers).Contains(participant.HostCenter ?? ""))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' Center ({participant.Center}) OR asigned Host Center({participant.HostCenter ?? ""}) is not matching with judge's assigned events' center: {eventCenters}");

        // Validate participant skill
        var judgeSkillsMatchedWithParticipantSkills = false;
        foreach (var judgeSkillCategory in judgeUser.AssignedSkillCategories!)
        {
            var skill = judgeSkillCategory.Split(":")[0].Trim();
            var category = judgeSkillCategory.Split(":")[1].Trim();

            if (skill == "Pravachan" && participant.Speech_Pravachan_Category.Contains(category))
                judgeSkillsMatchedWithParticipantSkills = true;

            if (skill == "Emcee" && participant.Emcee_Category.Contains(category))
                judgeSkillsMatchedWithParticipantSkills = true;
        }

        if (judgeSkillsMatchedWithParticipantSkills == false)
            throw new ApplicationException($"Judge {judgeUser.FullName} assigned skills for judging are not matching with '{participant.FirstName} {participant.LastName}'s skills");

        return participant?.Map<ParticipantModel>(mapper);
    }

    public async Task<ParticipantModel> GetParticipantForCheckIn(string bapsId, int loginUserId)
    {
        // Get User
        var checkInUser = await _usersCollection.Find(item => item.UserId == loginUserId).FirstOrDefaultAsync();
        if (checkInUser == null)
            throw new ApplicationException($"Check In User not found");

        if (checkInUser.BAPSId == bapsId)
            throw new ApplicationException($"User is not allowed to check in themselves");

        // User Events
        if (!checkInUser.AssignedEventIds.Any())
            throw new ApplicationException($"User {checkInUser.FullName} is not assigned any Competition Event");

        // User Roles
        if (!checkInUser.AssignedRoles.Any(role => role == "Check In"))
            throw new ApplicationException($"User {checkInUser.FullName} is not assigned 'Check In' role");

        // Active Events
        var events = await _competitionEventsCollection.Find(item => checkInUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();
        if (!events.Any())
            throw new ApplicationException($"User {checkInUser.FullName}'s assigned Competition Events are Not Active");

        // Get participant by MIS Id
        var participant = await _participantsCollection.Find(item => item.BAPSId == bapsId).FirstOrDefaultAsync();
        if (participant == null)
            throw new ApplicationException($"Participant not found for the given BAPS Id: {bapsId}");

        // User assigned Genders
        if (!checkInUser.AssignedGenders.Contains(participant.Gender))
            throw new ApplicationException($"User {checkInUser.FullName} is not now allowed to do check in for {participant.Gender} gender");

        // Event Center vs Participant Center or HostCenter
        var eventCenters = string.Join(", ", events.SelectMany(e => e.Centers));
        if (!events.SelectMany(e => e.Centers).Contains(participant.Center) && !events.SelectMany(e => e.Centers).Contains(participant.HostCenter ?? ""))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' Center ({participant.Center}) OR asigned Host Center({participant.HostCenter ?? ""}) is not matching with Check In user's assigned events' center: {eventCenters}");

        return participant?.Map<ParticipantModel>(mapper);
    }

    public async Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "")
    {
        var entities = await _participantsCollection.Find(item => (region == "" || item.Region == region) && (center == "" || item.Center == center) && (mandal == "" || item.Mandal == mandal)).ToListAsync();

        var models = entities.Select(c => c.Map<ParticipantModel>(mapper))
                            .OrderByDescending(c => c.Gender)
                            .ThenBy(c => c.Region)
                            .ThenBy(c => c.Center)
                            .ThenBy(c => c.FullName).ToList();

        return models;
    }

    public async Task<List<ParticipantModel>> GetParticipantsForLoginUser(string loginUserBapsId)
    {
        var loginUser = await _usersCollection.Find(item => item.BAPSId == loginUserBapsId).FirstOrDefaultAsync();

        if (loginUser == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {loginUserBapsId}");

        if (!loginUser.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to login user {loginUser.FullName}");

        var isNationalAdmin = loginUser.AssignedRoles.Contains("National Admin");

        var centers = new List<string>();

        if (!isNationalAdmin)
        {
            var events = await _competitionEventsCollection.Find(item => loginUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();

            if (!events.Any())
                throw new ApplicationException($"No any event assigned to user");

            centers = events.SelectMany(c => c.Centers).ToList();
            if (!centers.Any())
                throw new ApplicationException($"No any event center assigned to user");
        }

        var entities = await _participantsCollection.Find(item =>
                        (isNationalAdmin || centers.Contains(item.Center) || centers.Contains(item.HostCenter))
                        && loginUser.AssignedGenders.Contains(item.Gender)
                        ).ToListAsync();

        var models = entities.Select(c => c.Map<ParticipantModel>(mapper))
                            .OrderByDescending(c => c.Gender)
                            .ThenBy(c => c.Region)
                            .ThenBy(c => c.Center)
                            .ThenBy(c => c.FullName).ToList();

        return models;
    }

    public async Task<ParticipantModel> UpdateHostCenter(ParticipantUpdateHostCenterModel model)
    {
        var entity = await _participantsCollection.Find(item => item.MISId == model.MISId).FirstOrDefaultAsync();

        if (entity == null)
            throw new ApplicationException($"Participant not found for the given MIS Id: {model.MISId}");

        entity.HostCenter = model.HostCenter;

        _participantsCollection.ReplaceOne(item => item.MISId == model.MISId, entity);

        return entity.Map<ParticipantModel>(mapper);

    }

    public async Task<List<ParticipantModel>> Import(List<ParticipantModel> models)
    {
        foreach (var model in models)
        {
            // if (string.IsNullOrEmpty(model.MISId) || model.FirstLastName_MISID.IndexOf("-") == -1)
            //     continue;

            // if (int.TryParse(model.FirstLastName_MISID.Split(new[] { '-' })[1].Trim(), out int tmp))
            //     model.MISId = tmp;

            _participantsCollection.DeleteOne(c => c.MISId == model.MISId);

            var entity = model.Map<Participant>(mapper);

            _participantsCollection.InsertOne(entity);
        }

        models = await Get();

        return models;
    }
}
