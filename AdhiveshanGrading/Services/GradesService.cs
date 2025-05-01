namespace AdhiveshanGrading.Services;

public interface IGradesService
{
    Task<List<GradeModel>> GetForParticipantAndProctor(int misId, string skillCategory, int proctorUserId);
    Task<List<GradeModel>> GetGradedParticipantsForProctor(int proctorUserId);
    Task<GradeModel> AddOrUpdateForParticipantAndProctor(GradeUpdateModel updateModel);
}

public class GradesService : BaseService, IGradesService
{
    private readonly IMongoCollection<Grade> _GradesCollection;
    private readonly IMongoCollection<GradingTopic> _GradingTopicsCollection;
    private readonly IMongoCollection<SkillCategory> _SkillsCollection;
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<Participant> _participantsCollection;

    public GradesService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
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

    public async Task<List<GradeModel>> GetGradedParticipantsForProctor(int proctorUserId)
    {
        // Proctor
        var proctor = await _UsersCollection.Find<User>(item => item.UserId == proctorUserId).FirstOrDefaultAsync();

        // Graded Participants For proctor
        var entities = await _GradesCollection.Find(item => item.ProctorUserId == proctorUserId).ToListAsync();

        var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();
        if (!models.Any())
            return models;

        // Grading Topics for Skill Category
        var topicIds = entities.Select(e => e.GradingTopicId).ToList();
        var gradingTopicEntities = await _GradingTopicsCollection.Find(item => topicIds.Contains(item.GradingTopicId)).ToListAsync();

        // Skill Category Entity
        var skillCategoryIds = gradingTopicEntities.Select(g => g.SkillCategoryId).ToList();
        var skillCategoryEntities = await _SkillsCollection.Find(item => skillCategoryIds.Contains(item.SkillCategoryId)).ToListAsync();

        foreach (var item in models)
        {
            var topic = gradingTopicEntities.FirstOrDefault(c => c.GradingTopicId == item.GradingTopicId);
            if (topic != null)
            {
                item.TopicName = topic.Name;
                item.Sequence = topic.Sequence;
                item.TopicName = topic.Name;
                var skillCategory = skillCategoryEntities.FirstOrDefault(c => c.SkillCategoryId == topic.SkillCategoryId);

                if (skillCategory != null)
                {
                    item.Skill = skillCategory?.Skill;
                    item.Category = skillCategory?.Category;
                    item.Color = skillCategory?.Color;
                }
            }

            var participant = await _participantsCollection.Find(c => c.MISId == item.MISId).FirstOrDefaultAsync();

            item.Participant = participant?.Map<ParticipantModel>(mapper);

            item.ProctorName = proctor?.FullName;
        }

        return models.OrderBy(c => c.SkillWithCategory).ThenBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
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
