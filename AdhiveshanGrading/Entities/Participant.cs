namespace AdhiveshanGrading.Entities;

public class Participant : MongoDBEntity
{
    public int MISId { get; set; }

    [BsonIgnoreIfNull]
    public string BAPSId { get; set; }

    [BsonIgnoreIfNull]
    public string Region { get; set; }

    [BsonIgnoreIfNull]
    public string Center { get; set; }

    [BsonIgnoreIfNull]
    public string HostCenter { get; set; }

    [BsonIgnoreIfNull]
    public string Zone { get; set; }

    [BsonIgnoreIfNull]
    public string FirstName { get; set; }

    [BsonIgnoreIfNull]
    public string MiddleName { get; set; }

    [BsonIgnoreIfNull]
    public string LastName { get; set; }

    [BsonIgnoreIfNull]
    public string Gender { get; set; }

    [BsonIgnoreIfNull]
    public string Mandal { get; set; }

    [BsonIgnoreIfNull]
    public string Category { get; set; }

    [BsonIgnoreIfNull]
    public string RegistrationStatus { get; set; }

    [BsonIgnoreIfNull]
    public string Language_For_Skill_Competitions { get; set; }

    [BsonIgnoreIfNull]
    public string Speech_Pravachan { get; set; }

    [BsonIgnoreIfNull]
    public string Speech_Pravachan_Category { get; set; }

    [BsonIgnoreIfNull]
    public string Emcee { get; set; }

    [BsonIgnoreIfNull]
    public string Emcee_Category { get; set; }

    // [BsonIgnoreIfNull]
    // public string SkilledCompetitions { get; set; }

    // [BsonIgnoreIfNull]
    // public string Satsang_Mukhpath { get; set; }

    // [BsonIgnoreIfNull]
    // public int? Completed_Pushpo { get; set; }

    // [BsonIgnoreIfNull]
    // public string Sampark { get; set; }

    // [BsonIgnoreIfNull]
    // public string Sampark_Category { get; set; }

    // [BsonIgnoreIfNull]
    // public string Skit_Writing_Samvad_Lekhan { get; set; }

    // [BsonIgnoreIfNull]
    // public string Article_Writing { get; set; }

    // [BsonIgnoreIfNull]
    // public string Speech_Writing_Pravachan_Lekhan { get; set; }

    // [BsonIgnoreIfNull]
    // public string Vyaktigat_Kirtan_Gaan { get; set; }

    // [BsonIgnoreIfNull]
    // public string Vyaktigat_Kirtan_Gaan_Category { get; set; }

}