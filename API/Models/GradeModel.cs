namespace AdhiveshanGrading.Models;

public class GradeModel : ModelBase
{
    public int GradeId { get; set; }
    public string BAPSId { get; set; }
    public int GradingCriteriaId { get; set; }
    public decimal? Marks { get; set; }
    public int? JudgeUserId { get; set; }

    // ------------
    //public ParticipantModel Participant { get; set; }

    public string SectionName { get; set; }
    public string TopicName { get; set; }
    public int Sequence { get; set; }
    public decimal MaximumMarks { get; set; }

    public string Skill { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string SkillWithCategory => $"{Skill} : {Category}";
}

public class GradeUpdateModel : ModelBase
{
    public string BAPSId { get; set; }
    public int GradingCriteriaId { get; set; }
    public decimal? Marks { get; set; }
    public int? JudgeUserId { get; set; }
}
