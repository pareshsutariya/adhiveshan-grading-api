namespace AdhiveshanGrading.Entities;

public class User : MongoDBEntity
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string MISId { get; set; }
    public string Password { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Status { get; set; }
    public List<string> AssignedRoles { get; set; }

    [BsonIgnoreIfNull]
    public List<int> AssignedEventIds { get; set; }

    [BsonIgnoreIfNull]
    public List<string> AssignedMandals { get; set; }

    [BsonIgnoreIfNull]
    public List<int> AssignedSkillCategoryIds { get; set; }

    [BsonIgnoreIfNull]
    public DateTime? CheckedIn { get; set; }
}