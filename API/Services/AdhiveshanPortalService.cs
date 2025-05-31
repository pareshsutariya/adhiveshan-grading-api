using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IAdhiveshanPortalService
{
    Task<AdhiveshanPortalUserModel> GetUserForBapsId(string bapsId);
}

public class AdhiveshanPortalService : BaseService, IAdhiveshanPortalService
{
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<CompetitionEvent> _competitionEventsCollection;

    public AdhiveshanPortalService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _competitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
    }

    public async Task<AdhiveshanPortalUserModel> GetUserForBapsId(string bapsId)
    {
        var loginUser = await _UsersCollection.Find<User>(item => item.BAPSId == bapsId).FirstOrDefaultAsync();

        if (loginUser == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {bapsId}");

        if (!loginUser.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to user {loginUser.FullName}");

        var centers = new List<string>();

        var isNationalAdmin = loginUser.AssignedRoles.Contains("National Admin");

        if (!isNationalAdmin)
        {
            var events = await _competitionEventsCollection.Find(item => loginUser.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();

            if (!events.Any())
                throw new ApplicationException($"No any event assigned to user");

            centers = events.SelectMany(c => c.Centers).ToList();
            if (!centers.Any())
                throw new ApplicationException($"No any event center assigned to user");
        }

        var model = loginUser.Map<AdhiveshanPortalUserModel>(mapper);

        return model;
    }
}