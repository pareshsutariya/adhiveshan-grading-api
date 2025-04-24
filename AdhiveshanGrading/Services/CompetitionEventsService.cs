namespace AdhiveshanGrading.Services;

public interface ICompetitionEventsService
{
    Task<List<CompetitionEventModel>> Get();
    Task<CompetitionEventModel> Get(int id);
    CompetitionEventModel Create(CompetitionEventCreateModel createModel);
    void Update(int id, CompetitionEventUpdateModel updateModel);
}

public class CompetitionEventsService : BaseService, ICompetitionEventsService
{
    private readonly IMongoCollection<CompetitionEvent> _CompetitionEventsCollection;

    public CompetitionEventsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _CompetitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
    }

    public async Task<List<CompetitionEventModel>> Get()
    {
        var entities = await _CompetitionEventsCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<CompetitionEventModel>(mapper)).OrderBy(c => c.Region).ThenBy(c => c.Name).ToList();

        return models;
    }

    public async Task<CompetitionEventModel> Get(int id)
    {
        var entity = await _CompetitionEventsCollection.Find<CompetitionEvent>(item => item.CompetitionEventId == id).FirstOrDefaultAsync();

        var model = entity.Map<CompetitionEventModel>(mapper);

        return model;
    }

    public CompetitionEventModel Create(CompetitionEventCreateModel createModel)
    {
        var maxId = _CompetitionEventsCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.CompetitionEventId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var entity = createModel.Map<CompetitionEvent>(mapper);
        entity.CompetitionEventId = (maxId.Value + 1);

        entity.Centers = entity.Centers.OrderBy(c => c).ToList();

        _CompetitionEventsCollection.InsertOne(entity);

        var model = entity.Map<CompetitionEventModel>(mapper);

        return model;
    }

    public void Update(int id, CompetitionEventUpdateModel updateModel)
    {
        updateModel.Centers = updateModel.Centers.OrderBy(c => c).ToList();
        _CompetitionEventsCollection.ReplaceOne(item => item.CompetitionEventId == id, updateModel.Map<CompetitionEvent>(mapper));
    }
}
