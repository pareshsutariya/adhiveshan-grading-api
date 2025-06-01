using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IEventCheckInService
{
    Task<EventCheckIn> CheckIn(EventCheckInCreateModel model);
}

public class EventCheckInService : BaseService, IEventCheckInService
{
    private readonly IMongoCollection<CompetitionEvent> _CompetitionEventsCollection;
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<Participant> _ParticipantsCollection;
    private readonly IMongoCollection<EventCheckIn> _EventCheckInCollection;

    public EventCheckInService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _ParticipantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
        _CompetitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
        _EventCheckInCollection = Database.GetCollection<EventCheckIn>(settings.EventCheckInCollectionName); // Add this line
    }

    // Adds a check-in record for the given event, bapsId, and loginUserId
    public async Task<EventCheckIn> CheckIn(EventCheckInCreateModel model)
    {
        var maxId = _EventCheckInCollection.Find(c => true).SortByDescending(c => c.EventCheckInId).FirstOrDefault()?.EventCheckInId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var checkIn = new EventCheckIn
        {
            EventCheckInId = (maxId.Value + 1),
            EventId = model.EventId,
            ParticipantBAPSId = model.ParticipantBAPSId,
            CheckedInByUserId = model.LoginUserId,
            CheckedInAtUtc = DateTime.UtcNow
        };

        await _EventCheckInCollection.InsertOneAsync(checkIn);
        return checkIn;
    }
}