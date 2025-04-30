namespace AdhiveshanGrading.Services;

public interface IGradesService
{
    Task<List<GradeModel>> GetForParticipantAndProctor(int misId, string skillCategory, int proctorUserId);
    Task<GradeModel> AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel);
}

public class GradesService : BaseService, IGradesService
{
    private readonly IMongoCollection<Grade> _GradesCollection;
    private readonly IMongoCollection<GradingTopic> _GradingTopicsCollection;
    private readonly IMongoCollection<SkillCategory> _SkillsCollection;

    public GradesService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _GradesCollection = Database.GetCollection<Grade>(settings.GradesCollectionName);
        _GradingTopicsCollection = Database.GetCollection<GradingTopic>(settings.GradingTopicsCollectionName);
        _SkillsCollection = Database.GetCollection<SkillCategory>(settings.SkillCategoriesCollectionName);
    }

    public async Task<List<GradeModel>> GetForParticipantAndProctor(int misId, string skillCategory, int proctorUserId)
    {
        var result = new List<GradeModel> { };

        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        // Skill Category Entity
        var skillCategoryEntity = await _SkillsCollection.Find(item => item.Skill == skill && item.Category == category).FirstOrDefaultAsync();
        if (skillCategoryEntity == null)
            return result;

        // Grading Topics for Skill Category
        var gradingTopicEntities = await _GradingTopicsCollection.Find(item => item.SkillCategoryId == skillCategoryEntity.SkillCategoryId).ToListAsync();
        if (!gradingTopicEntities.Any())
            return result;

        var gradingTopicModels = gradingTopicEntities.Select(c => c.Map<GradingTopicModel>(mapper)).ToList();
        foreach (var item in gradingTopicModels)
        {
            if (skillCategoryEntity != null)
            {
                item.Skill = skillCategoryEntity?.Skill;
                item.Category = skillCategoryEntity?.Category;
                item.Color = skillCategoryEntity?.Color;
            }
        }

        // Participant Grades for the Skill Category
        var entities = await _GradesCollection.Find(item => item.MISId == misId && item.ProctorUserId == proctorUserId).ToListAsync();
        // if (!entities.Any())
        //     return result;

        //var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();

        foreach (var topic in gradingTopicModels)
        {
            var model = new GradeModel
            {
                MISId = misId,
                GradingTopicId = topic.GradingTopicId,
                // ----
                TopicName = topic.Name,
                Sequence = topic.Sequence,
                WeightageOptions = topic.WeightageOptions,

                Skill = skillCategoryEntity?.Skill,
                Category = skillCategoryEntity?.Category,
                Color = skillCategoryEntity?.Color,
            };

            var entity = entities.FirstOrDefault(c => c.GradingTopicId == topic.GradingTopicId);
            if (entity != null)
            {
                model.GradeId = entity.GradeId;
                model.Score = entity.Score;
                model.ProctorUserId = entity.ProctorUserId;
            }

            result.Add(model);
        }

        return result.OrderBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
    }

    public async Task<GradeModel> AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel)
    {
        var entity = await _GradesCollection.Find(item =>
                item.MISId == updateModel.MISId &&
                item.GradingTopicId == updateModel.GradingTopicId &&
                item.ProctorUserId == updateModel.ProctorUserId)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            var maxId = _GradesCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.GradeId;
            maxId = maxId.HasValue == false ? 0 : maxId.Value;

            entity = new Grade
            {
                GradeId = (maxId.Value + 1),
                MISId = updateModel.MISId,
                GradingTopicId = updateModel.GradingTopicId,
                ProctorUserId = updateModel.ProctorUserId,
                Score = updateModel.Score
            };

            _GradesCollection.InsertOne(entity);
        }
        else
        {
            entity.Score = updateModel.Score;
            _GradesCollection.ReplaceOne(item => item.GradeId == entity.GradeId, entity);
        }

        return entity.Map<GradeModel>(mapper);
    }
}
