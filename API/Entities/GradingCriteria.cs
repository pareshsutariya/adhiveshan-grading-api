namespace AdhiveshanGrading.Entities;

public class GradingCriteria : MongoDBEntity
{
    public int GradingCriteriaId { get; set; }
    public int SkillCategoryId { get; set; }

    public string Section { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int Sequence { get; set; }

    public decimal MaximumMarks { get; set; }

    public string Status { get; set; }
}