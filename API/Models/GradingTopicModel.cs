namespace AdhiveshanGrading.Models;

public class GradingCriteriaModel : ModelBase
{
    public int GradingCriteriaId { get; set; }

    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public decimal MaximumMarks { get; set; }

    public string Status { get; set; }

    // ------------
    public string Skill { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string SkillWithCategory => $"{Skill} : {Category}";
}

public class GradingCriteriaCreateModel : ModelBase
{
    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public decimal MaximumMarks { get; set; }

    public string Status { get; set; }
}

public class GradingCriteriaUpdateModel : ModelBase
{
    public int GradingCriteriaId { get; set; }

    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public decimal MaximumMarks { get; set; }

    public string Status { get; set; }
}
