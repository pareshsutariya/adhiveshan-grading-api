namespace AdhiveshanGrading.Services;

public static class RolePermissionsService
{
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

    public const string Events_View = "Events: View";
    public const string Events_Add = "Events: Add";
    public const string Events_Update = "Events: Update";

    public const string Grading_Participants_Search_Participants = "Grading Participants: Search Participants";
    public const string Grading_Participants_Grade_Participants = "Grading Participants: Grade Participants";
    public const string Grading_Participants_View_Participant_Grades = "Grading Participants: View Participant Grades";

    public static List<RolePermissionsModel> GetRolePermissions()
    {
        var nationalAdminRole = new RolePermissionsModel
        {
            RoleName = "National Admin",
            Permissions = new List<string>
            {
                Users_RegionalAdmins_View,
                Users_RegionalAdmins_Add,
                Users_RegionalAdmins_Update,

                Users_Proctors_View,
                Users_Proctors_Add,
                Users_Proctors_Update,

                Grading_Questions_View,
                Grading_Questions_Add,
                Grading_Questions_Update,

                Skill_Categories_View,

                Participants_View,
                Participants_Import,

                Events_View,
                Events_Add,
                Events_Update,
            }
        };

        var regionalAdminRole = new RolePermissionsModel
        {
            RoleName = "Regional Admin",
            Permissions = new List<string>
            {
                Users_Proctors_View,
                Users_Proctors_Add,
                Users_Proctors_Update,

                Grading_Questions_View,

                Skill_Categories_View,

                Participants_View,

                Events_View,
                Events_Update,
            }
        };

        var proctorRole = new RolePermissionsModel
        {
            RoleName = "Proctor",
            Permissions = new List<string>
            {
                Grading_Questions_View,
                Skill_Categories_View,
                Grading_Participants_Search_Participants,
                Grading_Participants_Grade_Participants,
                Grading_Participants_View_Participant_Grades
            }
        };

        var roles = new List<RolePermissionsModel>();
        roles.Add(nationalAdminRole);
        roles.Add(regionalAdminRole);
        roles.Add(proctorRole);

        return roles;
    }
}