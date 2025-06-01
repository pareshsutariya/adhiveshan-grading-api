namespace AdhiveshanGrading.Models;

public class EventCheckInCreateModel
{
    public int EventId { get; set; }

    public string ParticipantBAPSId { get; set; }

    public int LoginUserId { get; set; }
}