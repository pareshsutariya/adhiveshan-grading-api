namespace AdhiveshanGrading.Entities;

public class SkillCategory : MongoDBEntity
{
    public int SkillCategoryId { get; set; }
    public string Skill { get; set; }
    public string Category { get; set; }
    public string Status { get; set; }
}