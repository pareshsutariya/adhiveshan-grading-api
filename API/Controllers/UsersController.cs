namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service)
    {
        _service = service;
    }

    // [HttpGet]
    // public async Task<IActionResult> Get() => await _service.GetUsersForLoginUser();

    [HttpGet("GetUsersForLoginUser/{loginUserBapsId}")]
    public async Task<IActionResult> GetUsersForLoginUser(string loginUserBapsId)
        => Ok(ServiceResponse.Success(await _service.GetUsersForLoginUser(loginUserBapsId)));

    [HttpGet("GetByBAPSIdToAddAsUser/{participantBapsId}/{loginUserBapsId}")]
    public async Task<IActionResult> GetByBAPSIdToAddAsUser(string participantBapsId, string loginUserBapsId)
        => Ok(ServiceResponse.Success(await _service.GetByBAPSIdToAddAsUser(participantBapsId, loginUserBapsId)));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(ServiceResponse.Success(await _service.Get(id)));

    [HttpPost]
    public IActionResult Create(UserCreateModel item)
        => Ok(ServiceResponse.Success(_service.Create(item)));

    [HttpPut("{id}")]
    public IActionResult Update(int id, UserUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return Ok(ServiceResponse.Success(NoContent()));
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
        => await _service.Remove(id);

    [HttpPost("JudgesImport/{loginUserBapsId}")]
    public async Task<IActionResult> JudgesImport(string loginUserBapsId, List<UserJudgeImport> models)
        => Ok(ServiceResponse.Success(await _service.JudgesImport(loginUserBapsId, models)));
}
