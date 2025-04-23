namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{

    public PermissionsController()
    {
    }

    [HttpGet]
    public async Task<List<RolePermissionsModel>> Get() => PermissionsService.GetRolePermissions();
}
