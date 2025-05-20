namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantsService _service;

    public ParticipantsController(IParticipantsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? center = "", [FromQuery] string? mandal = "")
        => Ok(ServiceResponse.Success(await _service.Get(center, mandal)));

    [HttpGet("getByMISId/{misId}")]
    public async Task<IActionResult> GetByMISId(int misId)
        => Ok(ServiceResponse.Success(await _service.GetByMISId(misId)));

    [HttpGet("getByBAPSId/{bapsId}")]
    public async Task<IActionResult> GetByBAPSId(string bapsId)
        => Ok(ServiceResponse.Success(await _service.GetByBAPSId(bapsId)));

    [HttpGet("GetParticipantForJudging/{bapsId}/{judgeUserId}")]
    public async Task<IActionResult> GetParticipantForJudging(string bapsId, int judgeUserId)
        => Ok(ServiceResponse.Success(await _service.GetParticipantForJudging(bapsId, judgeUserId)));

    [HttpPost("updateHostCenter")]
    public async Task<IActionResult> UpdateHostCenter(ParticipantUpdateHostCenterModel model)
        => Ok(ServiceResponse.Success(await _service.UpdateHostCenter(model)));

    [HttpPost("import")]
    public async Task<IActionResult> Import(List<ParticipantModel> models)
        => Ok(ServiceResponse.Success(await _service.Import(models)));
}

