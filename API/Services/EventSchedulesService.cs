using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IEventSchedulesService
{
    Task<EventSchedule> GetByEventId(int eventId);
    EventSchedule Create(EventSchedule createModel);
    void Update(int id, EventSchedule updateModel);
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
        var maxId = _EventSchedulesCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.EventScheduleId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        entity.EventScheduleId = (maxId.Value + 1);

        _EventSchedulesCollection.InsertOne(entity);

        return entity;
    }

    public void Update(int id, EventSchedule entity)
    {
        _EventSchedulesCollection.ReplaceOne(item => item.EventScheduleId == id, entity);
    }
}