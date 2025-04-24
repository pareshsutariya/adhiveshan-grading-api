namespace AdhiveshanGrading.Models;

public class SkillCategoryModel : ModelBase
{
    public int SkillCategoryId { get; set; }
    public string Skill { get; set; }
    public string Category { get; set; }
    public string Status { get; set; }
}