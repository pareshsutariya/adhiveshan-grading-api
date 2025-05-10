using System.Linq;

namespace AdhiveshanGrading.Services;

public interface IUsersService
{
    Task<List<UserModel>> GetUsersForLoginUser(string loginUserBapsId);
    Task<UserModel> Get(int id);
    UserModel Create(UserCreateModel createModel);
    void Update(int id, UserUpdateModel updateModel);
    Task Remove(int id);
    Task<UserModel> GetUserByUsernameAndPassword(string username, string password);
}

public class UsersService : BaseService, IUsersService
{
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<CompetitionEvent> _EventsCollection;

    public UsersService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _EventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
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

    public async Task<UserModel> GetUserByUsernameAndPassword(string username, string password)
    {
        var userModel = (await _UsersCollection.Find(item => item.BAPSId.ToLower() == username.ToLower() && item.Password == password)
                                               .FirstOrDefaultAsync()
                        )?.Map<UserModel>(mapper);

        if (userModel == null)
            throw new ApplicationException($"User not found for the given credentials");

        if (userModel.Status != "Active")
            throw new ApplicationException($"User {userModel.FullName} is not Active");

        if (userModel != null && userModel.AssignedRoles.Any())
        {
            userModel.AssignedPermissions = RolePermissionsService.GetRolePermissions()
                            .Where(r => userModel.AssignedRoles.Contains(r.RoleName))
                            .SelectMany(c => c.Permissions).ToList();
        }

        return userModel;
    }

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
