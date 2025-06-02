namespace AdhiveshanGrading.Services;

public static class RolePermissionsService
{
    public const string Users_NationalAdmins_View = "Users: National Admins: View";
    public const string Users_NationalAdmins_Add = "Users: National Admins: Add";
    public const string Users_NationalAdmins_Update = "Users: National Admins: Update";

    public const string Users_RegionalAdmins_View = "Users: Regional Admins: View";
    public const string Users_RegionalAdmins_Add = "Users: Regional Admins: Add";
    public const string Users_RegionalAdmins_Update = "Users: Regional Admins: Update";

    public const string Users_Judges_View = "Users: Judges: View";
    public const string Users_Judges_Add = "Users: Judges: Add";
    public const string Users_Judges_Update = "Users: Judges: Update";

    public const string Users_CheckIns_View = "Users: Check Ins: View";
    public const string Users_CheckIns_Add = "Users: Check Ins: Add";
    public const string Users_CheckIns_Update = "Users: Check Ins: Update";

    public const string Grading_Questions_View = "Grading Questions: View";
    public const string Grading_Questions_Add = "Grading Questions: Add";
    public const string Grading_Questions_Update = "Grading Questions: Update";

    public const string Skill_Categories_View = "Skill Categories: View";
    public const string Regions_Centers_View = "Regions And Centers: View";

    public const string Participants_View = "Participants: View";
    public const string Participants_Import = "Participants: Import";

    public const string CheckIn_View = "Check In: View";
    public const string CheckIn_Update = "Check In: Update";

    public const string Events_View = "Events: View";
    public const string Events_Add = "Events: Add";
    public const string Events_Update = "Events: Update";

    public const string Schedules_View = "Schedules: View";
    public const string Schedules_Add = "Schedules: Add";
    public const string Schedules_Update = "Schedules: Update";

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
                Regions_Centers_View,
                Skill_Categories_View,

                Skill_Categories_View,

                Grading_Questions_View,
                Grading_Questions_Add,
                Grading_Questions_Update,

                Events_View,
                Events_Add,
                Events_Update,

                Schedules_View,
                Schedules_Add,
                Schedules_Update,

                Reports_Check_In_Report,
                Reports_Grading_Report,

                Users_NationalAdmins_View,
                Users_NationalAdmins_Add,
                Users_NationalAdmins_Update,

                Users_RegionalAdmins_View,
                Users_RegionalAdmins_Add,
                Users_RegionalAdmins_Update,

                Users_Judges_View,
                Users_Judges_Add,
                Users_Judges_Update,

                Users_CheckIns_View,
                Users_CheckIns_Add,
                Users_CheckIns_Update,
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

                Grading_Questions_View,

                Events_View,
                Events_Update,

                Events_View,
                Events_Add,
                Events_Update,

                Users_Judges_View,
                Users_Judges_Add,
                Users_Judges_Update,

                Users_CheckIns_View,
                Users_CheckIns_Add,
                Users_CheckIns_Update,
            }
        };

        var judgeRole = new RolePermissionsModel
        {
            RoleName = "Judge",
            Icon = "fa-solid fa-user-clock",
            Color = "Chocolate",
            Permissions = new List<string>
            {
                Grading_Participants_Search_Participants,
                Grading_Participants_Grade_Participants,
                Grading_Participants_View_Participants_Grade
            }
        };

        var resultCommitteeRole = new RolePermissionsModel
        {
            RoleName = "Result Committee",
            Icon = "fa-solid fa-square-poll-vertical",
            Color = "purple",
            Permissions = new List<string>
            {
                Reports_Grading_Report,
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
                Reports_Check_In_Report
            }
        };

        var roles = new List<RolePermissionsModel>();
        roles.Add(nationalAdminRole);
        roles.Add(regionalAdminRole);
        roles.Add(judgeRole);
        roles.Add(resultCommitteeRole);
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
                else if (model.RoleName == "Judge")
                {
                    pivot.Judge = true;
                    pivot.JudgeColor = model.Color;
                    pivot.JudgeIcon = model.Icon;
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