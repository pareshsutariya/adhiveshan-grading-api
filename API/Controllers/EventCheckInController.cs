namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class EventCheckInController : ControllerBase
{
    private readonly IEventCheckInService _service;

    public EventCheckInController(IEventCheckInService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CheckIn(EventCheckInCreateModel model)
        => Ok(ServiceResponse.Success(_service.CheckIn(model)));
}
