namespace AdhiveshanGrading.Models;

public class ParticipantGradesModel
{
    public ParticipantModel Participant { get; set; }
    public List<GradeModel> PravachanGrades { get; set; } = new();
    public List<GradeModel> EmceeGrades { get; set; } = new();

    public string PravachanSkill { get; set; }
    public string PravachanCategory { get; set; }
    public string EmceeSkill { get; set; }
    public string EmceeCategory { get; set; }

    public string EmceeSkillCategory { get; set; }
    public string JudgeName { get; set; }
    public string JudgeBAPSId { get; set; }
}