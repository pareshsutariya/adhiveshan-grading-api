namespace AdhiveshanGrading.Models;

public class ParticipantGradesModel : ParticipantModel
{
    public List<GradeModel> PravachanGrades { get; set; } = new();
    public List<GradeModel> EmceeGrades { get; set; } = new();

    public string JudgeName { get; set; }
    public string JudgeBAPSId { get; set; }
}