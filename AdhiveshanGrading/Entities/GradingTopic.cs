namespace AdhiveshanGrading.Entities;

public class GradingTopic : MongoDBEntity
{
    public int GradingTopicId { get; set; }
    public int SkillCategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int Sequence { get; set; }

    public decimal MaximumMarks { get; set; }

    public string Status { get; set; }
}