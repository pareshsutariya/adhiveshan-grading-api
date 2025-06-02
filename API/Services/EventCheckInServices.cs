using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IEventCheckInService
{
    Task<EventCheckIn> CheckIn(EventCheckInCreateModel model);
    Task<List<ParticipantModel>> GetCheckedInParticipants(int eventId);
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
        // Validate the event exists
        var checkInExists = await _EventCheckInCollection.Find(e => e.EventId == model.EventId && e.ParticipantBAPSId == model.ParticipantBAPSId).AnyAsync();
        if (checkInExists)
            throw new ApplicationException("Participant already checked in for this event.");

        var maxId = _EventCheckInCollection.Find(c => true).SortByDescending(c => c.EventCheckInId).FirstOrDefault()?.EventCheckInId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var loginUser = await _UsersCollection.Find(u => u.UserId == model.LoginUserId).FirstOrDefaultAsync();

        var checkIn = new EventCheckIn
        {
            EventCheckInId = (maxId.Value + 1),
            EventId = model.EventId,
            ParticipantBAPSId = model.ParticipantBAPSId,
            CheckedInByUserId = model.LoginUserId,
            CheckedInByBAPSId = loginUser.BAPSId,
            CheckedInAtUtc = DateTime.UtcNow
        };

        await _EventCheckInCollection.InsertOneAsync(checkIn);
        return checkIn;
    }

    public async Task<List<ParticipantModel>> GetCheckedInParticipants(int eventId)
    {
        // Fetch the check-ins for the event
        var checkIns = await _EventCheckInCollection.Find(e => e.EventId == eventId).ToListAsync();

        // Fetch participants based on the check-ins
        var participantBapsIds = checkIns.Select(c => c.ParticipantBAPSId).ToList();
        var participants = await _ParticipantsCollection.Find(p => participantBapsIds.Contains(p.BAPSId)).ToListAsync();

        // Map to ParticipantModel
        var participantModels = participants.Select(c => c.Map<ParticipantModel>(mapper)).ToList();

        // Set CheckInAtUtc and CheckedInByUserId for each participant
        foreach (var participant in participantModels)
        {
            var checkIn = checkIns.FirstOrDefault(c => c.ParticipantBAPSId == participant.BAPSId);
            if (checkIn != null)
            {
                participant.CheckInAtUtc = checkIn.CheckedInAtUtc;
                participant.CheckedInByUserId = checkIn.CheckedInByUserId;
                participant.CheckedInByBAPSId = checkIn.CheckedInByBAPSId;
            }
        }

        return participantModels;
    }
}