namespace AdhiveshanGrading.Services;

public interface IGradingTopicsService
{
    Task<List<SkillCategoryModel>> GetSkillCategories();
    Task<List<GradingTopicModel>> Get();
    Task<GradingTopicModel> Get(int id);
    GradingTopicModel Create(GradingTopicCreateModel createModel);
    void Update(int id, GradingTopicUpdateModel updateModel);
}

public class GradingTopicsService : BaseService, IGradingTopicsService
{
    private readonly IMongoCollection<GradingTopic> _GradingTopicsCollection;
    private readonly IMongoCollection<SkillCategory> _SkillsCollection;

    public GradingTopicsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _GradingTopicsCollection = Database.GetCollection<GradingTopic>(settings.GradingTopicsCollectionName);
        _SkillsCollection = Database.GetCollection<SkillCategory>(settings.SkillCategoriesCollectionName);
    }

    public async Task<List<SkillCategoryModel>> GetSkillCategories()
    {
        var entities = await _SkillsCollection.Find(item => true).ToListAsync();
        var models = entities.Select(c => c.Map<SkillCategoryModel>(mapper)).OrderBy(c => c.Skill).ThenBy(c => c.Category).ToList();
        return models;
    }

    public async Task<List<GradingTopicModel>> Get()
    {
        var entities = await _GradingTopicsCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<GradingTopicModel>(mapper)).ToList();

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

    public async Task<GradingTopicModel> Get(int id)
    {
        var entity = await _GradingTopicsCollection.Find<GradingTopic>(item => item.GradingTopicId == id).FirstOrDefaultAsync();

        var model = entity.Map<GradingTopicModel>(mapper);

        return model;
    }

    public GradingTopicModel Create(GradingTopicCreateModel createModel)
    {
        var maxId = _GradingTopicsCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.GradingTopicId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var entity = createModel.Map<GradingTopic>(mapper);
        entity.GradingTopicId = (maxId.Value + 1);

        _GradingTopicsCollection.InsertOne(entity);

        var model = entity.Map<GradingTopicModel>(mapper);

        return model;
    }

    public void Update(int id, GradingTopicUpdateModel updateModel)
    {
        _GradingTopicsCollection.ReplaceOne(item => item.GradingTopicId == id, updateModel.Map<GradingTopic>(mapper));
    }
}
