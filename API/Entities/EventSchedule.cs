namespace AdhiveshanGrading.Entities;

public class EventSchedule : MongoDBEntity
{
    public int EventScheduleId { get; set; }
    public int EventId { get; set; }
    public List<Room> Rooms { get; set; }
    public List<ParticipantForSchedule> Participants { get; set; }

}

[BsonIgnoreExtraElements(Inherited = true)]
public class Room
{
    public int RoomNumber { get; set; }
    public string RoomName { get; set; }
    public string Skill { get; set; }
    public int Duration { get; set; }

    public string RoomStartTime { get; set; }
    public string RoomEndTime { get; set; }

    public DateTime? RoomStartMoment { get; set; }
    public DateTime? RoomEndMoment { get; set; }

    public List<TimeSlice> TimeSlices { get; set; }
    public string Color { get; set; }
    public Boolean DefaultRoom { get; set; }
}

[BsonIgnoreExtraElements(Inherited = true)]
public class ParticipantForSchedule
{
    public int? MisId { get; set; }
    public string BapsId { get; set; }
    //------
    public List<ParticipatingSkill> ParticipatingSkills { get; set; }
    public int? Priority { get; set; }
    public bool? HasHostCenter { get; set; }
    public bool? PendingAssignment { get; set; }
}

[BsonIgnoreExtraElements(Inherited = true)]
public class TimeSlice
{
    public string Skill { get; set; }
    public int SliceNumber { get; set; }
    public string SliceStart { get; set; }
    public string SliceEnd { get; set; }
    public DateTime? SliceStartMoment { get; set; }
    public DateTime? SliceEndMoment { get; set; }

    public bool? IsBreakTime { get; set; }
    public bool? IsLunchBreak { get; set; }
    public bool? IsTeaBreak { get; set; }
    public bool? IsSpillOver { get; set; }
    public int? MisId { get; set; }
}

[BsonIgnoreExtraElements(Inherited = true)]
public class ParticipatingSkill
{
    public string Name { get; set; }
    public double Duration { get; set; }
    public string Color { get; set; }

    public DateTime? SliceStartMoment { get; set; }
    public DateTime? SliceEndMoment { get; set; }

    public string SliceStart { get; set; }
    public string SliceEnd { get; set; }
    public int? SliceNumber { get; set; }
}