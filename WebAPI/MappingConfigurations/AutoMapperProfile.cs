namespace AdhiveshanGrading.WebAPI;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Adhiveshan
        CreateMap<Entities.User, UserModel>().ReverseMap();
        CreateMap<Entities.User, UserCreateModel>().ReverseMap();
        CreateMap<Entities.User, UserUpdateModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationCreateModel>().ReverseMap();
        CreateMap<Entities.Configuration, ConfigurationUpdateModel>().ReverseMap();
    }
}