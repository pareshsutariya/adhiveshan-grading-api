using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IEventCheckInService
{

}

public class EventCheckInService : BaseService, IEventCheckInService
{
    private readonly IMongoCollection<CompetitionEvent> _CompetitionEventsCollection;
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<Participant> _ParticipantsCollection;

    public EventCheckInService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _ParticipantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
        _CompetitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
    }
}