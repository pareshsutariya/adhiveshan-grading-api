using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IEventSchedulesService
{
    Task<EventSchedule> GetByEventId(int eventId);
    EventSchedule Create(EventSchedule createModel);
}

public class EventSchedulesService : BaseService, IEventSchedulesService
{
    private readonly IMongoCollection<CompetitionEvent> _CompetitionEventsCollection;
    private readonly IMongoCollection<EventSchedule> _EventSchedulesCollection;

    public EventSchedulesService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _EventSchedulesCollection = Database.GetCollection<EventSchedule>(settings.EventSchedulesCollectionName);

        _CompetitionEventsCollection = Database.GetCollection<CompetitionEvent>(settings.CompetitionEventsCollectionName);
    }

    public async Task<EventSchedule> GetByEventId(int eventId)
    {
        var entity = await _EventSchedulesCollection.Find(item => item.EventId == eventId).FirstOrDefaultAsync();

        return entity;
    }

    public EventSchedule Create(EventSchedule entity)
    {
        _EventSchedulesCollection.DeleteOne(c => c.EventId == entity.EventId);

        _EventSchedulesCollection.InsertOne(entity);

        return entity;
    }
}