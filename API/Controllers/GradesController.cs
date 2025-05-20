namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class GradesController : ControllerBase
{
    private readonly IGradesService _service;

    public GradesController(IGradesService service)
    {
        _service = service;
    }

    [HttpGet("GetForParticipantAndJudge/{bapsId}/{skillCategory}/{judgeUserId}")]
    public async Task<IActionResult> GetForParticipantAndJudge(string bapsId, string skillCategory, int judgeUserId)
        => Ok(ServiceResponse.Success(await _service.GetForParticipantAndJudge(bapsId, skillCategory, judgeUserId)));

    [HttpGet("GetGradedParticipantsForJudge/{judgeUserId}")]
    public async Task<IActionResult> GetGradedParticipantsForJudge(int judgeUserId)
        => Ok(ServiceResponse.Success(await _service.GetGradedParticipantsForJudge(judgeUserId)));

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateForParticipantAndJudge(GradeUpdateModel model)
        => Ok(ServiceResponse.Success(await _service.AddOrUpdateForParticipantAndJudge(model)));
}
