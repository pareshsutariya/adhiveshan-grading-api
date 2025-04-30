namespace AdhiveshanGrading.Services;

public interface IGradesService
{
    Task<List<GradeModel>> GetForParticipantAndProctor(int misId, int proctorUserId);
    //Task<List<GradeModel>> AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel);
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

    public async Task<List<GradeModel>> GetForParticipantAndProctor(int misId, int proctorUserId)
    {
        var entities = await _GradesCollection.Find(item => item.MISId == misId && item.ProctorUserId == proctorUserId).ToListAsync();
        var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();

        if (!models.Any())
            return models;

        var gradingTopics = await _GradingTopicsCollection.Find(item => models.Select(c => c.GradingTopicId).Contains(item.GradingTopicId)).ToListAsync();
        var skillCategories = await _SkillsCollection.Find(item => true).ToListAsync();

        foreach (var item in models)
        {
            var gradingTopic = gradingTopics.FirstOrDefault(c => c.GradingTopicId == item.GradingTopicId);
            if (gradingTopic != null)
            {
                var skill = skillCategories.FirstOrDefault(c => c.SkillCategoryId == gradingTopic.SkillCategoryId);

                if (skill != null)
                {
                    item.TopicName = gradingTopic.Name;
                    item.Sequence = gradingTopic.Sequence;
                    item.Skill = skill?.Skill;
                    item.Category = skill?.Category;
                    item.Color = skill?.Color;
                }
            }
        }

        return models.OrderBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
    }
}
