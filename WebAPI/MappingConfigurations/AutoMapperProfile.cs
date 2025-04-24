namespace AdhiveshanGrading.WebAPI;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Adhiveshan
        CreateMap<Entities.SkillCategory, SkillCategoryModel>().ReverseMap();

        CreateMap<Entities.User, UserModel>().ReverseMap();
        CreateMap<Entities.User, UserCreateModel>().ReverseMap();
        CreateMap<Entities.User, UserUpdateModel>().ReverseMap();

        CreateMap<Entities.GradingTopic, GradingTopicModel>().ReverseMap();
        CreateMap<Entities.GradingTopic, GradingTopicCreateModel>().ReverseMap();
        CreateMap<Entities.GradingTopic, GradingTopicUpdateModel>().ReverseMap();

        CreateMap<Entities.CompetitionEvent, CompetitionEventModel>().ReverseMap();
        CreateMap<Entities.CompetitionEvent, CompetitionEventCreateModel>().ReverseMap();
        CreateMap<Entities.CompetitionEvent, CompetitionEventUpdateModel>().ReverseMap();

        CreateMap<Entities.Participant, ParticipantModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationCreateModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationUpdateModel>().ReverseMap();
    }
}