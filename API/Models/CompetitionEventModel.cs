namespace AdhiveshanGrading.Models;

public class CompetitionEventModel : ModelBase
{
    public int CompetitionEventId { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    public string HostCenter { get; set; }
    public List<string> Centers { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Status { get; set; }
    //------
    public string RegionalHostCenterDate => $"{Region} : {HostCenter} : {StartDate.ToShortDateString()}";
}

public class CompetitionEventCreateModel
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string HostCenter { get; set; }
    public List<string> Centers { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Status { get; set; }
}

public class CompetitionEventUpdateModel : ModelBase
{
    public int CompetitionEventId { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    public string HostCenter { get; set; }
    public List<string> Centers { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Status { get; set; }
}