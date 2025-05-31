namespace AdhiveshanGrading.API;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Adhiveshan
        _ = CreateMap<SkillCategory, SkillCategoryModel>().ReverseMap();

        _ = CreateMap<User, UserModel>().ReverseMap();
        _ = CreateMap<User, UserCreateModel>().ReverseMap();
        _ = CreateMap<User, UserUpdateModel>().ReverseMap();
        _ = CreateMap<User, AdhiveshanPortalUserModel>().ReverseMap();

        _ = CreateMap<GradingCriteria, GradingCriteriaModel>().ReverseMap();
        _ = CreateMap<GradingCriteria, GradingCriteriaCreateModel>().ReverseMap();
        _ = CreateMap<GradingCriteria, GradingCriteriaUpdateModel>().ReverseMap();

        _ = CreateMap<Grade, GradeModel>().ReverseMap();
        _ = CreateMap<Grade, GradeUpdateModel>().ReverseMap();

        _ = CreateMap<CompetitionEvent, CompetitionEventModel>().ReverseMap();
        _ = CreateMap<CompetitionEvent, CompetitionEventCreateModel>().ReverseMap();
        _ = CreateMap<CompetitionEvent, CompetitionEventUpdateModel>().ReverseMap();

        _ = CreateMap<Participant, ParticipantModel>().ReverseMap();
        _ = CreateMap<Configuration, ConfigurationModel>().ReverseMap();
        _ = CreateMap<Configuration, ConfigurationCreateModel>().ReverseMap();
        _ = CreateMap<Configuration, ConfigurationUpdateModel>().ReverseMap();
    }
}