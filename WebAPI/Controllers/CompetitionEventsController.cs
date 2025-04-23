namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetitionEventsController : ControllerBase
{
    private readonly ICompetitionEventsService _service;

    public CompetitionEventsController(ICompetitionEventsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<CompetitionEventModel>> Get() => await _service.Get();

    [HttpGet("{id}")]
    public async Task<CompetitionEventModel> Get(int id) => await _service.Get(id);

    [HttpPost]
    public ActionResult<CompetitionEventModel> Create(CompetitionEventCreateModel item)
    {
        var result = _service.Create(item);

        return CreatedAtRoute("", new { id = result.CompetitionEventId }, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CompetitionEventUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return NoContent();
    }
}
