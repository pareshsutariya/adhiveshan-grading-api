namespace AdhiveshanGrading.Entities;

public class EventCheckIn : MongoDBEntity
{
    public int EventCheckInId { get; set; }

    public int EventId { get; set; }

    public string ParticipantBAPSId { get; set; }

    public DateTime? CheckedInAtUtc { get; set; }

    public int CheckedInByUserId { get; set; }
}