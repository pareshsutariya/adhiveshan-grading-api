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

    [HttpGet("GetForParticipantAndProctor/{misId}/{skillCategory}/{proctorUserId}")]
    public async Task<List<GradeModel>> GetForParticipantAndProctor(int misId, string skillCategory, int proctorUserId)
        => await _service.GetForParticipantAndProctor(misId, skillCategory, proctorUserId);

    [HttpGet("GetGradedParticipantsForProctor/{proctorUserId}")]
    public async Task<List<GradeModel>> GetGradedParticipantsForProctor(int proctorUserId)
        => await _service.GetGradedParticipantsForProctor(proctorUserId);

    [HttpPost]
    public async Task<GradeModel> AddOrUpdateForParticipantAndProctor(GradeUpdateModel model) =>
        await _service.AddOrUpdateForParticipantAndProctor(model);
}
