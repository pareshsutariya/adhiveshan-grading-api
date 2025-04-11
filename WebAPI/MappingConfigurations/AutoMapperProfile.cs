using System;
using AutoMapper;


namespace AdhiveshanGrading.WebAPI;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Adhiveshan
        CreateMap<AdhiveshanGrading.Entities.User, AdhiveshanGrading.Models.UserModel>().ReverseMap();
        CreateMap<AdhiveshanGrading.Entities.User, AdhiveshanGrading.Models.UserCreateModel>().ReverseMap();
        CreateMap<AdhiveshanGrading.Entities.User, AdhiveshanGrading.Models.UserUpdateModel>().ReverseMap();
        CreateMap<AdhiveshanGrading.Entities.Configuration, AdhiveshanGrading.Models.ConfigurationModel>().ReverseMap();
        CreateMap<AdhiveshanGrading.Entities.Configuration, AdhiveshanGrading.Models.ConfigurationCreateModel>().ReverseMap();
        CreateMap<AdhiveshanGrading.Entities.Configuration, AdhiveshanGrading.Models.ConfigurationUpdateModel>().ReverseMap();
    }
}