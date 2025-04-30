namespace AdhiveshanGrading.Services;

public interface IGradesService
{
    Task<List<GradeModel>> GetForParticipantAndProctor(int misId, string skillCategory, int proctorUserId);
    Task AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel);
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
        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        // Skill Category Entity
        var skillCategoryEntity = await _SkillsCollection.Find(item => item.Skill == skill && item.Category == category).FirstOrDefaultAsync();
        if (skillCategoryEntity == null)
            return Enumerable.Empty<GradeModel>().ToList();

        // Grading Topics for Skill Category
        var gradingTopics = await _GradingTopicsCollection.Find(item => item.SkillCategoryId == skillCategoryEntity.SkillCategoryId).ToListAsync();
        if (!gradingTopics.Any())
            return Enumerable.Empty<GradeModel>().ToList();

        // Participant Grades for the Skill Category
        var entities = await _GradesCollection.Find(item => item.MISId == misId && item.ProctorUserId == proctorUserId).ToListAsync();
        if (!entities.Any())
            return Enumerable.Empty<GradeModel>().ToList();

        var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();

        foreach (var item in models)
        {
            var gradingTopic = gradingTopics.FirstOrDefault(c => c.GradingTopicId == item.GradingTopicId);
            if (gradingTopic != null)
            {
                if (skillCategoryEntity != null)
                {
                    item.TopicName = gradingTopic.Name;
                    item.Sequence = gradingTopic.Sequence;
                    item.Skill = skillCategoryEntity?.Skill;
                    item.Category = skillCategoryEntity?.Category;
                    item.Color = skillCategoryEntity?.Color;
                }
            }
        }

        return models.OrderBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
    }

    public async Task AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel)
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
    }
}
