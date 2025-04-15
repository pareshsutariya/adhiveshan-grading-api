namespace AdhiveshanGrading.Entities;

public class Participant : MongoDBEntity
{
    public int ParticipantId { get; set; }

    [BsonIgnoreIfNull]
    public string Region { get; set; }

    [BsonIgnoreIfNull]
    public string Center { get; set; }

    [BsonIgnoreIfNull]
    public string Zone { get; set; }

    [BsonIgnoreIfNull]
    public string Mandal { get; set; }

    [BsonIgnoreIfNull]
    public string FirstLastName_MISID { get; set; }

    [BsonIgnoreIfNull]
    public int? MISId { get; set; }

    [BsonIgnoreIfNull]
    public string SkilledCompetitions { get; set; }

    [BsonIgnoreIfNull]
    public string Satsang_Mukhpath { get; set; }

    [BsonIgnoreIfNull]
    public int? Completed_Pushpo { get; set; }

    [BsonIgnoreIfNull]
    public string Sampark { get; set; }

    [BsonIgnoreIfNull]
    public string Sampark_Category { get; set; }

    [BsonIgnoreIfNull]
    public string Speech_Pravachan { get; set; }

    [BsonIgnoreIfNull]
    public string Speech_Pravachan_Category { get; set; }

    [BsonIgnoreIfNull]
    public string Emcee { get; set; }

    [BsonIgnoreIfNull]
    public string Emcee_Category { get; set; }

    [BsonIgnoreIfNull]
    public string Skit_Writing_Samvad_Lekhan { get; set; }

    [BsonIgnoreIfNull]
    public string Article_Writing { get; set; }

    [BsonIgnoreIfNull]
    public string Speech_Writing_Pravachan_Lekhan { get; set; }

    [BsonIgnoreIfNull]
    public string Vyaktigat_Kirtan_Gaan { get; set; }

    [BsonIgnoreIfNull]
    public string Vyaktigat_Kirtan_Gaan_Category { get; set; }

    [BsonIgnoreIfNull]
    public string Gender { get; set; }
}