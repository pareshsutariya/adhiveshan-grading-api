namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionsController : ControllerBase
{
    public RolePermissionsController() { }

    [HttpGet]
    public async Task<List<RolePermissionsModel>> Get() => RolePermissionsService.GetRolePermissions();

    [HttpGet("pivot")]
    public async Task<List<RolePermissionsPivotModel>> GetPivot() => RolePermissionsService.GetRolePermissionsPivot();
}
