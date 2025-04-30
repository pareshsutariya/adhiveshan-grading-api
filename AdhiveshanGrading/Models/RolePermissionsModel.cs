namespace AdhiveshanGrading.Models;

public class RolePermissionsModel
{
    public string RoleName { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public List<string> Permissions { get; set; }
}

public class RolePermissionsPivotModel
{
    public string Permission { get; set; }

    public bool NationalAdmin { get; set; }
    public bool RegionalAdmin { get; set; }
    public bool Proctor { get; set; }
    public bool CheckIn { get; set; }

    public string NationalAdminIcon { get; set; }
    public string NationalAdminColor { get; set; }

    public string RegionalAdminIcon { get; set; }
    public string RegionalAdminColor { get; set; }

    public string ProctorIcon { get; set; }
    public string ProctorColor { get; set; }

    public string CheckInIcon { get; set; }
    public string CheckInColor { get; set; }
}