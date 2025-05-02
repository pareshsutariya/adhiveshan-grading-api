namespace AdhiveshanGrading.Entities;

public class Grade : MongoDBEntity
{
    public int GradeId { get; set; }

    public int MISId { get; set; }
    public int GradingTopicId { get; set; }

    [BsonIgnoreIfNull]
    public decimal? Marks { get; set; }

    [BsonIgnoreIfNull]
    public int? JudgeUserId { get; set; }
}