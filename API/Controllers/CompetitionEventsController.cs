namespace AdhiveshanGrading.Controllers;

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
    public async Task<IActionResult> Get()
        => Ok(ServiceResponse.Success(await _service.Get()));

    [HttpGet("GetEventsForLoginUser/{loginUserBapsId}")]
    public async Task<IActionResult> GetEventsForLoginUser(string loginUserBapsId)
        => Ok(ServiceResponse.Success(await _service.GetEventsForLoginUser(loginUserBapsId)));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(ServiceResponse.Success(await _service.Get(id)));

    [HttpPost]
    public IActionResult Create(CompetitionEventCreateModel item)
        => Ok(ServiceResponse.Success(_service.Create(item)));

    [HttpPut("{id}")]
    public IActionResult Update(int id, CompetitionEventUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return Ok(ServiceResponse.Success(NoContent()));
    }
}
