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

    [HttpGet("GetParticipantsForLoginUser/{loginUserBapsId}")]
    public async Task<IActionResult> GetParticipantsForLoginUser(string loginUserBapsId)
        => Ok(ServiceResponse.Success(await _service.GetParticipantsForLoginUser(loginUserBapsId)));

    [HttpGet("getByMISId/{misId}")]
    public async Task<IActionResult> GetByMISId(int misId)
        => Ok(ServiceResponse.Success(await _service.GetByMISId(misId)));

    [HttpGet("getByBAPSId/{bapsId}")]
    public async Task<IActionResult> GetByBAPSId(string bapsId)
        => Ok(ServiceResponse.Success(await _service.GetByBAPSId(bapsId)));

    [HttpGet("GetParticipantsForEvent/{eventId}/{gender}")]
    public async Task<IActionResult> GetParticipantsForEvent(int eventId, string gender)
        => Ok(ServiceResponse.Success(await _service.GetParticipantsForEvent(eventId, gender)));

    [HttpGet("GetParticipantForJudging/{bapsId}/{judgeUserId}")]
    public async Task<IActionResult> GetParticipantForJudging(string bapsId, int judgeUserId)
        => Ok(ServiceResponse.Success(await _service.GetParticipantForJudging(bapsId, judgeUserId)));

    [HttpGet("GetParticipantForCheckIn/{bapsId}/{eventId}/{loginUserId}")]
    public async Task<IActionResult> GetParticipantForCheckIn(string bapsId, int eventId, int loginUserId)
        => Ok(ServiceResponse.Success(await _service.GetParticipantForCheckIn(bapsId, eventId, loginUserId)));

    [HttpPost("updateHostCenter")]
    public async Task<IActionResult> UpdateHostCenter(ParticipantUpdateHostCenterModel model)
        => Ok(ServiceResponse.Success(await _service.UpdateHostCenter(model)));

    [HttpPost("import")]
    public async Task<IActionResult> Import(List<ParticipantModel> models)
        => Ok(ServiceResponse.Success(await _service.Import(models)));
}

