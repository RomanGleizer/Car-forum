using AutoMapper;
using CarForum.Models;
using CarForum.ViewModels;

namespace CarForum.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(c => c.Phone));

        CreateMap<LoginViewModel, User>();
    }
}
