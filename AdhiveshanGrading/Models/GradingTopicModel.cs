namespace AdhiveshanGrading.Models;

public class GradingTopicModel : ModelBase
{
    public int GradingTopicId { get; set; }

    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public int Round { get; set; }

    public List<decimal> WeightageOptions { get; set; }
    public int RequiredProctors { get; set; }

    public string Status { get; set; }

    // ------------
    public string Skill { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string SkillWithCategory => $"{Skill} : {Category}";
}

public class GradingTopicCreateModel : ModelBase
{
    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public int Round { get; set; }

    public List<decimal> WeightageOptions { get; set; }
    public int RequiredProctors { get; set; }

    public string Status { get; set; }
}

public class GradingTopicUpdateModel : ModelBase
{
    public int GradingTopicId { get; set; }

    public int SkillCategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Sequence { get; set; }

    public int Round { get; set; }

    public List<decimal> WeightageOptions { get; set; }
    public int RequiredProctors { get; set; }

    public string Status { get; set; }
}
