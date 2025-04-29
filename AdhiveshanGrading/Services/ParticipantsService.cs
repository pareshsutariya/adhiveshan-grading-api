namespace AdhiveshanGrading.Services;

public interface IParticipantsService
{
    Task<List<ParticipantModel>> Get(string region = "", string center = "", string mandal = "");
    Task<ParticipantModel> GetByMISId(int misId);
    Task<ParticipantModel> GetByMISIdAneSkillCategory(int misId, string skillCategory);
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

    public async Task<ParticipantModel> GetByMISIdAneSkillCategory(int misId, string skillCategory)
    {
        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        var entity = await _participantsCollection.Find(item => item.MISId == misId).FirstOrDefaultAsync();

        if (skill == "Pravachan" && entity.Speech_Pravachan_Category.Contains(category) ||
            skill == "Emcee" && entity.Emcee_Category.Contains(category))
            return entity?.Map<ParticipantModel>(mapper);

        return null;
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
