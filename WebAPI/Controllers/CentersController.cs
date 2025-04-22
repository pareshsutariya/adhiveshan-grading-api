namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CentersController : ControllerBase
{
    private readonly ICentersService _service;

    public CentersController(ICentersService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<List<CenterModel>> Get() => await _service.Get();

}
