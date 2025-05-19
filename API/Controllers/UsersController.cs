namespace AdhiveshanGradingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service)
    {
        _service = service;
    }

    // [HttpGet]
    // public async Task<List<UserModel>> Get() => await _service.GetUsersForLoginUser();

    [HttpGet("GetUsersForLoginUser/{loginUserBapsId}")]
    public async Task<List<UserModel>> GetUsersForLoginUser(string loginUserBapsId) => await _service.GetUsersForLoginUser(loginUserBapsId);

    [HttpGet("{id}")]
    public async Task<UserModel> Get(int id) => await _service.Get(id);

    [HttpPost]
    public ActionResult<UserModel> Create(UserCreateModel item)
    {
        var result = _service.Create(item);

        return CreatedAtRoute("", new { id = result.UserId }, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UserUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id) => await _service.Remove(id);

    [HttpGet("GetUserByUsernameAndPassword/{username}/{password}")]
    public async Task<UserModel> GetUserByUsernameAndPassword(string username, string password) => await _service.GetUserByUsernameAndPassword(username, password);

    [HttpPost("JudgesImport/{loginUserBapsId}")]
    public async Task<ActionResult<bool>> JudgesImport(string loginUserBapsId, List<UserJudgeImport> models)
        => await _service.JudgesImport(loginUserBapsId, models);
}
