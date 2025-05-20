namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionsController : ControllerBase
{
    public RolePermissionsController() { }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(ServiceResponse.Success(RolePermissionsService.GetRolePermissions()));

    [HttpGet("pivot")]
    public async Task<IActionResult> GetPivot()
        => Ok(ServiceResponse.Success(RolePermissionsService.GetRolePermissionsPivot()));
}
