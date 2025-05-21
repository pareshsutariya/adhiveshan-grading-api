namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class EventSchedulesController : ControllerBase
{
    private readonly IEventSchedulesService _service;

    public EventSchedulesController(IEventSchedulesService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByEventId(int eventId)
        => Ok(ServiceResponse.Success(await _service.GetByEventId(eventId)));

    [HttpPost]
    public IActionResult Create(EventSchedule item)
        => Ok(ServiceResponse.Success(_service.Create(item)));

    [HttpPut("{id}")]
    public IActionResult Update(int id, EventSchedule model)
    {
        _service.Update(id, model);

        return Ok(ServiceResponse.Success(NoContent()));
    }
}
