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
    public async Task<List<SkillCategoryModel>> GetSkillCategories() => await _service.GetSkillCategories();

    [HttpGet]
    public async Task<List<GradingCriteriaModel>> Get() => await _service.Get();

    [HttpGet("GetBySkillCategory/{skillCategory}")]
    public async Task<List<GradingCriteriaModel>> GetBySkillCategory(string skillCategory)
        => await _service.GetBySkillCategory(skillCategory);

    [HttpGet("{id}")]
    public async Task<GradingCriteriaModel> Get(int id) => await _service.Get(id);

    [HttpPost]
    public ActionResult<GradingCriteriaModel> Create(GradingCriteriaCreateModel item)
    {
        var result = _service.Create(item);

        return CreatedAtRoute("", new { id = result.GradingCriteriaId }, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, GradingCriteriaUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return NoContent();
    }
}
