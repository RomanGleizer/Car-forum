using AutoMapper;
using CarForum.Models;
using CarForum.ViewModels;

namespace CarForum.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));

        CreateMap<LoginViewModel, User>();
    }
}
