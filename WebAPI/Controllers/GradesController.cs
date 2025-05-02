namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradesController : ControllerBase
{
    private readonly IGradesService _service;

    public GradesController(IGradesService service)
    {
        _service = service;
    }

    [HttpGet("GetForParticipantAndJudge/{misId}/{skillCategory}/{judgeUserId}")]
    public async Task<List<GradeModel>> GetForParticipantAndJudge(int misId, string skillCategory, int judgeUserId)
        => await _service.GetForParticipantAndJudge(misId, skillCategory, judgeUserId);

    [HttpGet("GetGradedParticipantsForJudge/{judgeUserId}")]
    public async Task<List<GradeModel>> GetGradedParticipantsForJudge(int judgeUserId)
        => await _service.GetGradedParticipantsForJudge(judgeUserId);

    [HttpPost]
    public async Task<GradeModel> AddOrUpdateForParticipantAndJudge(GradeUpdateModel model) =>
        await _service.AddOrUpdateForParticipantAndJudge(model);
}
