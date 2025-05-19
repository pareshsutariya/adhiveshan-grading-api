using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IGradingCriteriasService
{
    Task<List<SkillCategoryModel>> GetSkillCategories();
    Task<List<GradingCriteriaModel>> Get();
    Task<List<GradingCriteriaModel>> GetBySkillCategory(string skillCategory);
    Task<GradingCriteriaModel> Get(int id);
    GradingCriteriaModel Create(GradingCriteriaCreateModel createModel);
    void Update(int id, GradingCriteriaUpdateModel updateModel);
}

public class GradingCriteriasService : BaseService, IGradingCriteriasService
{
    private readonly IMongoCollection<GradingCriteria> _GradingCriteriasCollection;
    private readonly IMongoCollection<SkillCategory> _SkillsCollection;

    public GradingCriteriasService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _GradingCriteriasCollection = Database.GetCollection<GradingCriteria>(settings.GradingCriteriasCollectionName);
        _SkillsCollection = Database.GetCollection<SkillCategory>(settings.SkillCategoriesCollectionName);
    }

    public async Task<List<SkillCategoryModel>> GetSkillCategories()
    {
        var entities = await _SkillsCollection.Find(item => true).ToListAsync();
        var models = entities.Select(c => c.Map<SkillCategoryModel>(mapper)).OrderBy(c => c.Skill).ThenBy(c => c.Category).ToList();
        return models;
    }

    public async Task<List<GradingCriteriaModel>> Get()
    {
        var entities = await _GradingCriteriasCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<GradingCriteriaModel>(mapper)).ToList();

        var skillCategories = await _SkillsCollection.Find(item => true).ToListAsync();

        foreach (var item in models)
        {
            var skill = skillCategories.FirstOrDefault(c => c.SkillCategoryId == item.SkillCategoryId);

            if (skill != null)
            {
                item.Skill = skill?.Skill;
                item.Category = skill?.Category;
                item.Color = skill?.Color;
            }
        }

        return models.OrderBy(c => c.SkillWithCategory).ThenBy(c => c.Sequence).ThenBy(c => c.Name).ToList();
    }

    public async Task<List<GradingCriteriaModel>> GetBySkillCategory(string skillCategory)
    {
        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        var skillCategoryEntity = await _SkillsCollection.Find(item => item.Skill == skill && item.Category == category).FirstOrDefaultAsync();

        if (skillCategoryEntity == null)
            throw new ApplicationException($"Skill Category not found");

        var entities = await _GradingCriteriasCollection.Find(item => item.SkillCategoryId == skillCategoryEntity.SkillCategoryId && item.Status == "Active").ToListAsync();

        var models = entities.Select(c => c.Map<GradingCriteriaModel>(mapper)).ToList();

        var skillCategories = await _SkillsCollection.Find(item => true).ToListAsync();

        foreach (var item in models)
        {
            var skillCat = skillCategories.FirstOrDefault(c => c.SkillCategoryId == item.SkillCategoryId);

            if (skillCat != null)
            {
                item.Skill = skillCat?.Skill;
                item.Category = skillCat?.Category;
                item.Color = skillCat?.Color;
            }
        }

        return models.OrderBy(c => c.SkillWithCategory).ThenBy(c => c.Sequence).ThenBy(c => c.Name).ToList();
    }

    public async Task<GradingCriteriaModel> Get(int id)
    {
        var entity = await _GradingCriteriasCollection.Find<GradingCriteria>(item => item.GradingCriteriaId == id).FirstOrDefaultAsync();

        var model = entity.Map<GradingCriteriaModel>(mapper);

        return model;
    }

    public GradingCriteriaModel Create(GradingCriteriaCreateModel createModel)
    {
        var maxId = _GradingCriteriasCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.GradingCriteriaId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var entity = createModel.Map<GradingCriteria>(mapper);
        entity.GradingCriteriaId = (maxId.Value + 1);

        _GradingCriteriasCollection.InsertOne(entity);

        var model = entity.Map<GradingCriteriaModel>(mapper);

        return model;
    }

    public void Update(int id, GradingCriteriaUpdateModel updateModel)
    {
        _GradingCriteriasCollection.ReplaceOne(item => item.GradingCriteriaId == id, updateModel.Map<GradingCriteria>(mapper));
    }
}
