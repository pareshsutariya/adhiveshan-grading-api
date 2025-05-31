namespace AdhiveshanGrading.Models;

public class UserModel : ModelBase
{
    public int UserId { get; set; }
    public string BAPSId { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Status { get; set; }
    public List<string> AssignedRoles { get; set; }

    public List<int> AssignedEventIds { get; set; }
    public List<string> AssignedGenders { get; set; }
    public List<string> AssignedSkillCategories { get; set; }
    public DateTime? CheckedIn { get; set; }

    public List<CompetitionEventModel> AssignedEvents { get; set; } = new();

    // ----
    public List<string> AssignedPermissions { get; set; } = new();
}

public class UserCreateModel
{
    public string BAPSId { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Status { get; set; }
    public List<string> AssignedRoles { get; set; } = new();

    public List<int> AssignedEventIds { get; set; }
    public List<string> AssignedGenders { get; set; }
    public List<string> AssignedSkillCategories { get; set; }
    public DateTime? CheckedIn { get; set; }
}

public class UserUpdateModel : ModelBase
{
    public int UserId { get; set; }
    public string BAPSId { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Status { get; set; }
    public List<string> AssignedRoles { get; set; } = new();

    public List<int> AssignedEventIds { get; set; }
    public List<string> AssignedGenders { get; set; }
    public List<string> AssignedSkillCategories { get; set; }
    public DateTime? CheckedIn { get; set; }
}


public class AdhiveshanPortalUserModel : ModelBase
{
    public string BAPSId { get; set; }
    public string FullName { get; set; }
    public string Region { get; set; }
    public string Center { get; set; }
    public string Status { get; set; }
}
