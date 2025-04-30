namespace AdhiveshanGrading.Models;

public class GradeModel : ModelBase
{
    public int GradeId { get; set; }
    public int MISId { get; set; }
    public int GradingTopicId { get; set; }
    public decimal? Score { get; set; }
    public int? ProctorUserId { get; set; }

    // ------------
    public string TopicName { get; set; }
    public int Sequence { get; set; }
    public List<decimal> WeightageOptions { get; set; }

    public string Skill { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string SkillWithCategory => $"{Skill} : {Category}";
}

public class GradeUpdateModel : ModelBase
{
    public int MISId { get; set; }
    public int GradingTopicId { get; set; }
    public decimal? Score { get; set; }
    public int? ProctorUserId { get; set; }
}
