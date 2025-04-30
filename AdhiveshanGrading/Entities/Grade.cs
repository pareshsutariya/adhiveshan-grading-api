namespace AdhiveshanGrading.Entities;

public class Grade : MongoDBEntity
{
    public int GradingId { get; set; }

    public int MISId { get; set; }
    public int GradingTopicId { get; set; }
    public decimal Score { get; set; }

    public int ProctorUserId { get; set; }
}