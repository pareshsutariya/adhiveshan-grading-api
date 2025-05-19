namespace AdhiveshanGrading.API;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Adhiveshan
        CreateMap<Entities.SkillCategory, SkillCategoryModel>().ReverseMap();

        CreateMap<Entities.User, UserModel>().ReverseMap();
        CreateMap<Entities.User, UserCreateModel>().ReverseMap();
        CreateMap<Entities.User, UserUpdateModel>().ReverseMap();

        CreateMap<Entities.GradingCriteria, GradingCriteriaModel>().ReverseMap();
        CreateMap<Entities.GradingCriteria, GradingCriteriaCreateModel>().ReverseMap();
        CreateMap<Entities.GradingCriteria, GradingCriteriaUpdateModel>().ReverseMap();

        CreateMap<Entities.Grade, GradeModel>().ReverseMap();
        CreateMap<Entities.Grade, GradeUpdateModel>().ReverseMap();

        CreateMap<Entities.CompetitionEvent, CompetitionEventModel>().ReverseMap();
        CreateMap<Entities.CompetitionEvent, CompetitionEventCreateModel>().ReverseMap();
        CreateMap<Entities.CompetitionEvent, CompetitionEventUpdateModel>().ReverseMap();

        CreateMap<Entities.Participant, ParticipantModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationCreateModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationUpdateModel>().ReverseMap();
    }
}