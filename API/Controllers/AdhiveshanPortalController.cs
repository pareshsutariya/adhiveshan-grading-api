
[Route("api/[controller]")]
[ApiController]
public class AdhiveshanPortalController : ControllerBase
{
    private readonly IAdhiveshanPortalService _service;

    public AdhiveshanPortalController(IAdhiveshanPortalService service)
    {
        _service = service;
    }

    [HttpGet("GetUserForBapsId/{bapsId}")]
    public async Task<IActionResult> GetUserForBapsId(string bapsId)
        => Ok(ServiceResponse.Success(await _service.GetUserForBapsId(bapsId)));
}