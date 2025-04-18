namespace AdhiveshanGrading.Services;

public interface IParticipantsService
{
    Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "");
    Task<ParticipantModel> GetByMISId(int misId);
    Task<List<ParticipantModel>> Import(List<ParticipantModel> models);
}

public class ParticipantsService : BaseService, IParticipantsService
{
    private readonly IMongoCollection<Participant> _participantsCollection;

    public ParticipantsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
    }

    public async Task<ParticipantModel> GetByMISId(int misId)
    {
        var entity = await _participantsCollection.Find(item => item.MISId == misId).FirstOrDefaultAsync();

        return entity?.Map<ParticipantModel>(mapper);
    }

    public async Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "")
    {
        var entities = await _participantsCollection.Find(item => (region == "" || item.Region == region) && (center == "" || item.Center == center) && (mandal == "" || item.Mandal == mandal)).ToListAsync();

        var models = entities.Select(c => c.Map<ParticipantModel>(mapper))
                            .OrderBy(c => c.Gender)
                            .ThenByDescending(c => c.Completed_Pushpo_Range_Title)
                            .ThenBy(c => c.FirstLastName_MISID).ToList();

        return models;
    }

    public async Task<List<ParticipantModel>> Import(List<ParticipantModel> models)
    {
        var gender = models.Where(m => !string.IsNullOrWhiteSpace(m.Gender)).FirstOrDefault().Gender;
        var region = models.Where(m => !string.IsNullOrWhiteSpace(m.Region)).FirstOrDefault().Region;
        _participantsCollection.DeleteMany(c => c.Gender == gender && c.Region == region);

        foreach (var model in models)
        {
            if (string.IsNullOrEmpty(model.FirstLastName_MISID) || model.FirstLastName_MISID.IndexOf("-") == -1)
                continue;

            if (int.TryParse(model.FirstLastName_MISID.Split(new[] { '-' })[1].Trim(), out int tmp))
                model.MISId = tmp;

            var entity = model.Map<Participant>(mapper);

            _participantsCollection.InsertOne(entity);
        }

        models = await Get();

        return models;
    }
}
