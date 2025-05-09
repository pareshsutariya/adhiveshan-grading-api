namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantsController : ControllerBase
{
    private readonly IParticipantsService _service;

    public ParticipantsController(IParticipantsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<ParticipantModel>> Get([FromQuery] string? center = "", [FromQuery] string? mandal = "") => await _service.Get(center, mandal);

    [HttpGet("getByMISId/{misId}")]
    public async Task<ParticipantModel> GetByMISId(int misId) => await _service.GetByMISId(misId);

    [HttpGet("GetParticipantForJudging/{bapsId}/{judgeUserId}")]
    public async Task<ParticipantModel> GetParticipantForJudging(string bapsId, int judgeUserId)
        => await _service.GetParticipantForJudging(bapsId, judgeUserId);

    [HttpPost("updateHostCenter")]
    public async Task<ActionResult<ParticipantModel>> UpdateHostCenter(ParticipantUpdateHostCenterModel model)
    {
        var result = await _service.UpdateHostCenter(model);

        return result;
    }

    [HttpPost("import")]
    public async Task<ActionResult<List<ParticipantModel>>> Import(List<ParticipantModel> models)
    {
        var result = await _service.Import(models);

        return result;
    }
}

