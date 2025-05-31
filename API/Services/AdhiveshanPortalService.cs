using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IAdhiveshanPortalService
{
    Task<AdhiveshanPortalUserModel> GetUserForBapsId(string bapsId);
    Task<string> GetParticipantStatus(string bapsId);
}

public class AdhiveshanPortalService : BaseService, IAdhiveshanPortalService
{
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<CompetitionEvent> _competitionEventsCollection;
    private readonly IMongoCollection<Participant> _participantsCollection;

    public AdhiveshanPortalService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _competitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
    }

    public async Task<AdhiveshanPortalUserModel> GetUserForBapsId(string bapsId)
    {
        var user = await _UsersCollection.Find<User>(item => item.BAPSId == bapsId).FirstOrDefaultAsync();

        if (user == null)
            throw new ApplicationException($"User not found for the given BAPS Id: {bapsId}");

        if (!user.AssignedRoles.Any())
            throw new ApplicationException($"No any role assigned to user {user.FullName}");

        var centers = new List<string>();

        var isNationalAdmin = user.AssignedRoles.Contains("National Admin");

        if (!isNationalAdmin)
        {
            var events = await _competitionEventsCollection.Find(item => user.AssignedEventIds.Contains(item.CompetitionEventId) && item.Status == "Active").ToListAsync();

            if (!events.Any())
                throw new ApplicationException($"No any event assigned to user");

            centers = events.SelectMany(c => c.Centers).ToList();
            if (!centers.Any())
                throw new ApplicationException($"No any event center assigned to user");
        }

        var model = user.Map<AdhiveshanPortalUserModel>(mapper);

        return model;
    }

    public async Task<string> GetParticipantStatus(string bapsId)
    {
        var participant = await _participantsCollection.Find(item => item.BAPSId == bapsId).FirstOrDefaultAsync();

        if (participant == null)
            throw new ApplicationException($"Participant not found for the given BAPS Id: {bapsId}");

        return $"Status: Pending";
    }
}