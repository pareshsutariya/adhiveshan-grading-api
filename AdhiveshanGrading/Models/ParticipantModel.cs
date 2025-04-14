using System;
using System.Text.Json.Serialization;

namespace AdhiveshanGrading.Models;

public class ParticipantModel : ModelBase
{
    public int ParticipantId { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Zone { get; set; }
    public string Mandal { get; set; }
    public string FirstLastName_MISID { get; set; }
    public string SkilledCompetitions { get; set; }
    public string Satsang_Mukhpath { get; set; }
    public int? Completed_Pushpo { get; set; }
    public string Sampark { get; set; }
    public string Sampark_Category { get; set; }
    public string Speech_Pravachan { get; set; }
    public string Speech_Pravachan_Category { get; set; }
    public string Emcee { get; set; }
    public string Emcee_Category { get; set; }
    public string Skit_Writing_Samvad_Lekhan { get; set; }
    public string Article_Writing { get; set; }
    public string Speech_Writing_Pravachan_Lekhan { get; set; }
    public string Vyaktigat_Kirtan_Gaan { get; set; }
    public string Vyaktigat_Kirtan_Gaan_Category { get; set; }
    public string Gender { get; set; }

    public int? MISId
    {
        get
        {
            int? misId = null;

            if (string.IsNullOrEmpty(FirstLastName_MISID))
                return misId;

            if (FirstLastName_MISID.IndexOf("-") >= 0)
            {
                int tmp;

                if (int.TryParse(FirstLastName_MISID.Split(new[] { '-' })[0].Trim(), out tmp))
                    misId = tmp;
            }

            return misId;
        }
    }

    public string FullName
    {
        get
        {
            if (string.IsNullOrEmpty(FirstLastName_MISID))
                return null;

            if (FirstLastName_MISID.IndexOf("-") >= 0)
                return FirstLastName_MISID.Split(new[] { '-' })[0];

            return FirstLastName_MISID;
        }
    }

    public string Completed_Pushpo_Range_Title
    {
        get
        {
            if (Completed_Pushpo.GetValueOrDefault() == 0)
                return "Completed Pushpa: None";

            if (Completed_Pushpo.GetValueOrDefault() >= 20)
                return "Completed Pushpa: 20 (All)";

            if (Completed_Pushpo.GetValueOrDefault() >= 1 && Completed_Pushpo.GetValueOrDefault() <= 5)
                return "Completed Pushpa: 01-05";

            if (Completed_Pushpo.GetValueOrDefault() >= 6 && Completed_Pushpo.GetValueOrDefault() <= 10)
                return "Completed Pushpa: 06-10";

            if (Completed_Pushpo.GetValueOrDefault() >= 11 && Completed_Pushpo.GetValueOrDefault() <= 15)
                return "Completed Pushpa: 11-15";

            if (Completed_Pushpo.GetValueOrDefault() >= 16 && Completed_Pushpo.GetValueOrDefault() <= 19)
                return "Completed Pushpa: 16-19";

            return null;
        }
    }
}
