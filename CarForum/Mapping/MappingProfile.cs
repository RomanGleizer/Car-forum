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

        CreateMap<User, EditProfileViewModel>()
                    .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

        CreateMap<EditProfileViewModel, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));

        CreateMap<LoginViewModel, User>();
        CreateMap<CreateReviewViewModel, Review>();
        CreateMap<EditReviewViewModel, Review>();
        CreateMap<Review, EditReviewViewModel>();
        CreateMap<DeletedReview, Review>();
    }
}
