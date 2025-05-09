namespace AdhiveshanGrading.Services;

public interface ICompetitionEventsService
{
    Task<List<CompetitionEventModel>> Get();
    Task<List<CompetitionEventModel>> GetEventsForLoginUser(string loginUserBapsId);
    Task<CompetitionEventModel> Get(int id);
    CompetitionEventModel Create(CompetitionEventCreateModel createModel);
    void Update(int id, CompetitionEventUpdateModel updateModel);
}

public class CompetitionEventsService : BaseService, ICompetitionEventsService
{
    private readonly IMongoCollection<CompetitionEvent> _CompetitionEventsCollection;
    private readonly IMongoCollection<User> _UsersCollection;

    public CompetitionEventsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);

        _CompetitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
    }

    public async Task<List<CompetitionEventModel>> Get()
    {
        var entities = await _CompetitionEventsCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<CompetitionEventModel>(mapper)).OrderBy(c => c.Region).ThenBy(c => c.Name).ToList();

        return models;
    }

    public async Task<List<CompetitionEventModel>> GetEventsForLoginUser(string loginUserBapsId)
    {
        var loginUser = await _UsersCollection.Find(item => item.BAPSId == loginUserBapsId).FirstOrDefaultAsync();

        if (loginUser == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {loginUserBapsId}");

        if (!loginUser.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to login user {loginUser.FullName}");

        var entities = await _CompetitionEventsCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<CompetitionEventModel>(mapper)).OrderBy(c => c.Region).ThenBy(c => c.Name).ToList();

        // If login user is not a National Admin
        if (!loginUser.AssignedRoles.Contains("National Admin"))
        {
            models = entities.Where(c =>
                                // If login user is not a National Admin, filter for the assigned event
                                loginUser.AssignedEventIds.Contains(c.CompetitionEventId))
                             .Select(c => c.Map<CompetitionEventModel>(mapper))
                             .OrderBy(c => c.Region)
                             .ThenBy(c => c.Name)
                             .ToList();
        }

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
