using Microsoft.AspNetCore.Hosting;

namespace AdhiveshanGrading.Services;

public interface IGradesService
{
    Task<List<GradeModel>> GetForParticipantAndJudge(string bapsId, string skillCategory, int judgeUserId);
    Task<List<GradeModel>> GetGradedParticipantsForJudge(int judgeUserId);
    Task<GradeModel> AddOrUpdateForParticipantAndJudge(GradeUpdateModel updateModel);
}

public class GradesService : BaseService, IGradesService
{
    private readonly IMongoCollection<Grade> _GradesCollection;
    private readonly IMongoCollection<GradingCriteria> _GradingCriteriasCollection;
    private readonly IMongoCollection<SkillCategory> _SkillsCollection;
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IMongoCollection<Participant> _participantsCollection;

    public GradesService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment) : base(settings, mapper, hostingEnvironment)
    {
        _participantsCollection = Database.GetCollection<Participant>(settings.ParticipantsCollectionName);
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        _GradesCollection = Database.GetCollection<Grade>(settings.GradesCollectionName);
        _GradingCriteriasCollection = Database.GetCollection<GradingCriteria>(settings.GradingCriteriasCollectionName);
        _SkillsCollection = Database.GetCollection<SkillCategory>(settings.SkillCategoriesCollectionName);
    }

    public async Task<List<GradeModel>> GetForParticipantAndJudge(string bapsId, string skillCategory, int judgeUserId)
    {
        var result = new List<GradeModel> { };

        var skill = skillCategory.Split(":")[0].Trim();
        var category = skillCategory.Split(":")[1].Trim();

        // Skill Category Entity
        var skillCategoryEntity = await _SkillsCollection.Find(item => item.Skill == skill && item.Category == category).FirstOrDefaultAsync();
        if (skillCategoryEntity == null)
            return result;

        // Grading Topics for Skill Category
        var gradingCriteriaEntities = await _GradingCriteriasCollection.Find(item => item.SkillCategoryId == skillCategoryEntity.SkillCategoryId).ToListAsync();
        if (!gradingCriteriaEntities.Any())
            return result;

        var gradingCriteriaModels = gradingCriteriaEntities.Select(c => c.Map<GradingCriteriaModel>(mapper)).ToList();
        foreach (var item in gradingCriteriaModels)
        {
            if (skillCategoryEntity != null)
            {
                item.Skill = skillCategoryEntity?.Skill;
                item.Category = skillCategoryEntity?.Category;
                item.Color = skillCategoryEntity?.Color;
            }
        }

        // Participant Grades for the Skill Category
        var entities = await _GradesCollection.Find(item => item.BAPSId == bapsId && item.JudgeUserId == judgeUserId).ToListAsync();
        // if (!entities.Any())
        //     return result;

        //var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();

        foreach (var topic in gradingCriteriaModels)
        {
            var model = new GradeModel
            {
                BAPSId = bapsId,
                GradingCriteriaId = topic.GradingCriteriaId,
                // ----
                SectionName = topic.Section,
                TopicName = topic.Name,
                Sequence = topic.Sequence,
                MaximumMarks = topic.MaximumMarks,

                Skill = skillCategoryEntity?.Skill,
                Category = skillCategoryEntity?.Category,
                Color = skillCategoryEntity?.Color,
            };

            var entity = entities.FirstOrDefault(c => c.GradingCriteriaId == topic.GradingCriteriaId);
            if (entity != null)
            {
                model.GradeId = entity.GradeId;
                model.Marks = entity.Marks;
                model.JudgeUserId = entity.JudgeUserId;
            }

            result.Add(model);
        }

        return result.OrderBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
    }

    public async Task<List<GradeModel>> GetGradedParticipantsForJudge(int judgeUserId)
    {
        // Judge
        var judge = await _UsersCollection.Find<User>(item => item.UserId == judgeUserId).FirstOrDefaultAsync();

        // Graded Participants For judge
        var entities = await _GradesCollection.Find(item => item.JudgeUserId == judgeUserId).ToListAsync();

        var models = entities.Select(c => c.Map<GradeModel>(mapper)).ToList();
        if (!models.Any())
            return models;

        // Grading Topics for Skill Category
        var topicIds = entities.Select(e => e.GradingCriteriaId).ToList();
        var gradingCriteriaEntities = await _GradingCriteriasCollection.Find(item => topicIds.Contains(item.GradingCriteriaId)).ToListAsync();

        // Skill Category Entity
        var skillCategoryIds = gradingCriteriaEntities.Select(g => g.SkillCategoryId).ToList();
        var skillCategoryEntities = await _SkillsCollection.Find(item => skillCategoryIds.Contains(item.SkillCategoryId)).ToListAsync();

        foreach (var item in models)
        {
            var topic = gradingCriteriaEntities.FirstOrDefault(c => c.GradingCriteriaId == item.GradingCriteriaId);
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

            var participant = await _participantsCollection.Find(c => c.BAPSId == item.BAPSId).FirstOrDefaultAsync();

            item.Participant = participant?.Map<ParticipantModel>(mapper);

            item.JudgeName = judge?.FullName;
        }

        return models.OrderBy(c => c.SkillWithCategory).ThenBy(c => c.Sequence).ThenBy(c => c.TopicName).ToList();
    }

    public async Task<GradeModel> AddOrUpdateForParticipantAndJudge(GradeUpdateModel updateModel)
    {
        var entity = await _GradesCollection.Find(item =>
                item.BAPSId == updateModel.BAPSId &&
                item.GradingCriteriaId == updateModel.GradingCriteriaId &&
                item.JudgeUserId == updateModel.JudgeUserId)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            var maxId = _GradesCollection.Find(c => true).SortByDescending(c => c.Id).FirstOrDefault()?.GradeId;
            maxId = maxId.HasValue == false ? 0 : maxId.Value;

            entity = new Grade
            {
                GradeId = (maxId.Value + 1),
                BAPSId = updateModel.BAPSId,
                GradingCriteriaId = updateModel.GradingCriteriaId,
                JudgeUserId = updateModel.JudgeUserId,
                Marks = updateModel.Marks
            };

            _GradesCollection.InsertOne(entity);
        }
        else
        {
            entity.Marks = updateModel.Marks;
            _GradesCollection.ReplaceOne(item => item.GradeId == entity.GradeId, entity);
        }

        return entity.Map<GradeModel>(mapper);
    }
}
