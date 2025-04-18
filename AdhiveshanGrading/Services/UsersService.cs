namespace AdhiveshanGrading.Services;

public interface IUsersService
{
    Task<List<UserModel>> Get();
    Task<UserModel> Get(int id);
    UserModel Create(UserCreateModel createModel);
    void Update(int id, UserUpdateModel updateModel);
    Task Remove(int id);
    Task<UserModel> GetUserByUsernameAndPassword(string username, string password);
}

public class UsersService : BaseService, IUsersService
{
    private readonly IMongoCollection<User> _UsersCollection;

    public UsersService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
    }

    public async Task<List<UserModel>> Get()
    {
        var entities = await _UsersCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<UserModel>(mapper)).OrderBy(c => c.Username).ToList();

        return models;
    }

    public async Task<UserModel> Get(int id)
    {
        var entity = await _UsersCollection.Find<User>(item => item.UserId == id).FirstOrDefaultAsync();

        var model = entity.Map<UserModel>(mapper);

        return model;
    }

    public async Task<UserModel> GetUserByUsernameAndPassword(string username, string password) =>
        (await _UsersCollection.Find(item => item.Username.ToLower() == username.ToLower() && item.Password == password).FirstOrDefaultAsync())?.Map<UserModel>(mapper);

    public UserModel Create(UserCreateModel createModel)
    {
        var maxId = _UsersCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.UserId;
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
}
