namespace AdhiveshanGrading.Models;

public class ParticipantModel : ModelBase
{
    public int MISId { get; set; }
    public string BAPSId { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string HostCenter { get; set; }
    public string Zone { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string Mandal { get; set; }
    public string Category { get; set; }
    public string RegistrationStatus { get; set; }
    public string Language_For_Skill_Competitions { get; set; }
    public string Speech_Pravachan { get; set; }
    public string Speech_Pravachan_Category { get; set; }
    public string Emcee { get; set; }
    public string Emcee_Category { get; set; }

    // public string SkilledCompetitions { get; set; }
    // public string Satsang_Mukhpath { get; set; }
    // public int? Completed_Pushpo { get; set; }
    // public string Sampark { get; set; }
    // public string Sampark_Category { get; set; }
    // public string Skit_Writing_Samvad_Lekhan { get; set; }
    // public string Article_Writing { get; set; }
    // public string Speech_Writing_Pravachan_Lekhan { get; set; }
    // public string Vyaktigat_Kirtan_Gaan { get; set; }
    // public string Vyaktigat_Kirtan_Gaan_Category { get; set; }

    public string FullName => $"{FirstName} {MiddleName} {LastName}";
    public DateTime? CheckInAtUtc { get; set; }
    public int? CheckedInByUserId { get; set; }
}
