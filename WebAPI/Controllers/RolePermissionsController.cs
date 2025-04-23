namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionsController : ControllerBase
{

    public RolePermissionsController()
    {
    }

    [HttpGet]
    public async Task<List<RolePermissionsModel>> Get() => RolePermissionsService.GetRolePermissions();
}
