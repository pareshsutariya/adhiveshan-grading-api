namespace AdhiveshanGradingAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GradingTopicsController : ControllerBase
{
    private readonly IGradingTopicsService _service;

    public GradingTopicsController(IGradingTopicsService service)
    {
        _service = service;
    }

    [HttpGet("GetSkillCategories")]
    public async Task<List<SkillCategoryModel>> GetSkillCategories() => await _service.GetSkillCategories();

    [HttpGet]
    public async Task<List<GradingTopicModel>> Get() => await _service.Get();

    [HttpGet("{id}")]
    public async Task<GradingTopicModel> Get(int id) => await _service.Get(id);

    [HttpPost]
    public ActionResult<GradingTopicModel> Create(GradingTopicCreateModel item)
    {
        var result = _service.Create(item);

        return CreatedAtRoute("", new { id = result.GradingTopicId }, result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, GradingTopicUpdateModel model)
    {
        var item = _service.Get(id);

        if (item == null)
            return NotFound();

        _service.Update(id, model);

        return NoContent();
    }
}
