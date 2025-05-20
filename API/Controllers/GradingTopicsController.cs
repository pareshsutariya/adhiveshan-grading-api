namespace AdhiveshanGrading.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradingCriteriasController : ControllerBase
{
    private readonly IGradingCriteriasService _service;

    public GradingCriteriasController(IGradingCriteriasService service)
    {
        _service = service;
    }

    [HttpGet("GetSkillCategories")]
    public async Task<IActionResult> GetSkillCategories()
        => Ok(ServiceResponse.Success(await _service.GetSkillCategories()));

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(ServiceResponse.Success(await _service.Get()));

    [HttpGet("GetBySkillCategory/{skillCategory}")]
    public async Task<IActionResult> GetBySkillCategory(string skillCategory)
        => Ok(ServiceResponse.Success(await _service.GetBySkillCategory(skillCategory)));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(ServiceResponse.Success(await _service.Get(id)));

    [HttpPost]
    public IActionResult Create(GradingCriteriaCreateModel item)
        => Ok(ServiceResponse.Success(_service.Create(item)));

    [HttpPut("{id}")]
    public IActionResult Update(int id, GradingCriteriaUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return Ok(ServiceResponse.Success(NoContent()));
    }
}
