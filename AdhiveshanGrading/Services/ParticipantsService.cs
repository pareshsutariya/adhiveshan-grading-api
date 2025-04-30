namespace AdhiveshanGrading.Services;

public interface IParticipantsService
{
    Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "");
    Task<ParticipantModel> GetByMISId(int misId);
    Task<ParticipantModel> GetCandidateForProctoring(int misId, string skillCategory, int proctorUserId);
    Task<List<ParticipantModel>> Import(List<ParticipantModel> models);
}

public class ParticipantsService : BaseService, IParticipantsService
{
    private readonly IMongoCollection<Participant> _participantsCollection;
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<CompetitionEvent> _competitionEventsCollection;

    public ParticipantsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
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

    public async Task<ParticipantModel> GetCandidateForProctoring(int misId, string skillCategory, int proctorUserId)
    {
        // Get participant by MIS Id
        var participant = await _participantsCollection.Find(item => item.MISId == misId).FirstOrDefaultAsync();
        if (participant == null)
            throw new ApplicationException($"Participant not found for the given MIS Id: {misId}");

        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        // Validate participant skill
        if (skill == "Pravachan" && !participant.Speech_Pravachan_Category.Contains(category))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' has not participated in {skillCategory}");
        if (skill == "Emcee" && !participant.Emcee_Category.Contains(category))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' has not participated in {skillCategory}");

        // Get Proctor
        var proctorUser = await _usersCollection.Find(item => item.UserId == proctorUserId).FirstOrDefaultAsync();
        if (proctorUser == null)
            throw new ApplicationException($"Proctor not found");

        // Proctor assigned Genders
        if (!proctorUser.AssignedGenders.Contains(participant.Gender))
            throw new ApplicationException($"Proctor {proctorUser.FullName} is not now allowed to do proctoring for {participant.Gender} gender");

        // Proctor Events
        if (!proctorUser.AssignedEventIds.Any())
            throw new ApplicationException($"Proctor {proctorUser.FullName} is not assigned any Competition Event");

        // Active Events
        var events = await _competitionEventsCollection.Find(item => proctorUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();
        if (!events.Any())
            throw new ApplicationException($"Proctor {proctorUser.FullName}'s assigned Competition Events are Not Active");

        // Event Center vs Participant Center
        if (!events.SelectMany(e => e.Centers).Contains(participant.Center))
            throw new ApplicationException($"Participant '{participant.FirstName} {participant.LastName}' center {participant.Center} is not matching with proctor's assigned events' center");

        return participant?.Map<ParticipantModel>(mapper);
    }

    public async Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "")
    {
        var entities = await _participantsCollection.Find(item => (region == "" || item.Region == region) && (center == "" || item.Center == center) && (mandal == "" || item.Mandal == mandal)).ToListAsync();

        var models = entities.Select(c => c.Map<ParticipantModel>(mapper))
                            .OrderBy(c => c.Gender)
                            .ThenBy(c => c.FullName).ToList();

        return models;
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
