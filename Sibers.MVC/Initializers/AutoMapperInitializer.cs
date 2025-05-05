using AutoMapper;
using Sibers.Core.Entities;
using Sibers.MVC.ViewModels.Projects;
using Sibers.MVC.ViewModels.Users;
using Sibers.Services.DTO;

namespace Sibers.MVC.Initializers;

public class AutoMapperInitializer : Profile
{
    public AutoMapperInitializer()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
            .ReverseMap()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        CreateMap<UserDTO, CreateUserViewModel>()
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<UserDTO, LoginViewModel>().ReverseMap();
        CreateMap<UserDTO, UserViewModel>()
            .ReverseMap()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<Project, ProjectDTO>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager))
            .ReverseMap();

        CreateMap<ProjectDTO, ProjectViewModel>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager))
            .ReverseMap();
    }
}
