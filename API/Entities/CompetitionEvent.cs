namespace AdhiveshanGrading.Entities;

public class CompetitionEvent : MongoDBEntity
{
    public int CompetitionEventId { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    public string HostCenter { get; set; }

    [BsonIgnoreIfNull]
    public List<string> Centers { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Status { get; set; }
}