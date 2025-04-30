namespace AdhiveshanGrading.Services;

public static class RolePermissionsService
{
    public const string Users_NationalAdmins_View = "Users: National Admins: View";
    public const string Users_NationalAdmins_Add = "Users: National Admins: Add";
    public const string Users_NationalAdmins_Update = "Users: National Admins: Update";

    public const string Users_RegionalAdmins_View = "Users: Regional Admins: View";
    public const string Users_RegionalAdmins_Add = "Users: Regional Admins: Add";
    public const string Users_RegionalAdmins_Update = "Users: Regional Admins: Update";

    public const string Users_Proctors_View = "Users: Proctors: View";
    public const string Users_Proctors_Add = "Users: Proctors: Add";
    public const string Users_Proctors_Update = "Users: Proctors: Update";

    public const string Grading_Questions_View = "Grading Questions: View";
    public const string Grading_Questions_Add = "Grading Questions: Add";
    public const string Grading_Questions_Update = "Grading Questions: Update";

    public const string Skill_Categories_View = "Skill Categories: View";

    public const string Participants_View = "Participants: View";
    public const string Participants_Import = "Participants: Import";

    public const string CheckIn_View = "Check In: View";
    public const string CheckIn_Update = "Check In: Update";

    public const string Events_View = "Events: View";
    public const string Events_Add = "Events: Add";
    public const string Events_Update = "Events: Update";

    public const string Grading_Participants_Search_Participants = "Grading Participants: Search Participants";
    public const string Grading_Participants_Grade_Participants = "Grading Participants: Grade Participants";
    public const string Grading_Participants_View_Participants_Grade = "Grading Participants: View Participants Grade";

    public const string Reports_Check_In_Report = "Reports: Check In Report";
    public const string Reports_Grading_Report = "Reports: Grading Report";

    public static List<RolePermissionsModel> GetRolePermissions()
    {
        var nationalAdminRole = new RolePermissionsModel
        {
            RoleName = "National Admin",
            Icon = "fa-solid fa-user-shield",
            Color = "brown",
            Permissions = new List<string>
            {
                Participants_View,
                Participants_Import,

                Skill_Categories_View,

                Grading_Questions_View,
                Grading_Questions_Add,
                Grading_Questions_Update,

                Events_View,
                Events_Add,
                Events_Update,

                Reports_Check_In_Report,
                Reports_Grading_Report,

                Users_NationalAdmins_View,
                Users_NationalAdmins_Add,
                Users_NationalAdmins_Update,

                Users_RegionalAdmins_View,
                Users_RegionalAdmins_Add,
                Users_RegionalAdmins_Update,

                Users_Proctors_View,
                Users_Proctors_Add,
                Users_Proctors_Update,
            }
        };

        var regionalAdminRole = new RolePermissionsModel
        {
            RoleName = "Regional Admin",
            Icon = "fa-solid fa-user-tag",
            Color = "blue",
            Permissions = new List<string>
            {
                Participants_View,
                Skill_Categories_View,

                Grading_Questions_View,

                Events_View,
                Events_Update,

                Users_Proctors_View,
                Users_Proctors_Add,
                Users_Proctors_Update,
            }
        };

        var proctorRole = new RolePermissionsModel
        {
            RoleName = "Proctor",
            Icon = "fa-solid fa-user-clock",
            Color = "black",
            Permissions = new List<string>
            {
                Skill_Categories_View,
                Grading_Participants_Search_Participants,
                Grading_Participants_Grade_Participants,
                Grading_Participants_View_Participants_Grade
            }
        };

        var checkInRole = new RolePermissionsModel
        {
            RoleName = "Check In",
            Icon = "fa-solid fa-user-check",
            Color = "green",
            Permissions = new List<string>
            {
                CheckIn_View,
                CheckIn_Update,
            }
        };

        var roles = new List<RolePermissionsModel>();
        roles.Add(nationalAdminRole);
        roles.Add(regionalAdminRole);
        roles.Add(proctorRole);
        roles.Add(checkInRole);

        return roles;
    }

    public static List<RolePermissionsPivotModel> GetRolePermissionsPivot()
    {
        var models = GetRolePermissions();
        var pivotModels = new List<RolePermissionsPivotModel> { };

        foreach (var model in models)
        {
            foreach (var per in model.Permissions)
            {
                var pivot = pivotModels.FirstOrDefault(c => c.Permission == per);
                if (pivot == null)
                {
                    pivot = new RolePermissionsPivotModel { Permission = per };
                    pivotModels.Add(pivot);
                }

                if (model.RoleName == "National Admin")
                {
                    pivot.NationalAdmin = true;
                    pivot.NationalAdminColor = model.Color;
                    pivot.NationalAdminIcon = model.Icon;
                }
                else if (model.RoleName == "Regional Admin")
                {
                    pivot.RegionalAdmin = true;
                    pivot.RegionalAdminColor = model.Color;
                    pivot.RegionalAdminIcon = model.Icon;
                }
                else if (model.RoleName == "Proctor")
                {
                    pivot.Proctor = true;
                    pivot.ProctorColor = model.Color;
                    pivot.ProctorIcon = model.Icon;
                }
                else if (model.RoleName == "Check In")
                {
                    pivot.CheckIn = true;
                    pivot.CheckInColor = model.Color;
                    pivot.CheckInIcon = model.Icon;
                }
            }
        }

        return pivotModels;
    }
}