namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequestModel loginRequestModel)
        => Ok(ServiceResponse.Success(await _service.Authenticate(loginRequestModel)));

    [HttpPost("login-adhivehshan-portal")]
    public async Task<IActionResult> LoginAdhiveshanPortal(AuthRequestModel loginRequestModel)
        => Ok(ServiceResponse.Success(await _service.Authenticate(loginRequestModel, true)));
}
