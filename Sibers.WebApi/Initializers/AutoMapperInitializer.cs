using AutoMapper;
using Sibers.Core.Entities;
using Sibers.Services.DTO;
using Sibers.WebApi.Contracts;
using Sibers.WebApi.ViewModels;

namespace Sibers.WebApi.Initializers;

public class AutoMapperInitializer : Profile
{
    public AutoMapperInitializer()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
            .ReverseMap()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<Project, ProjectDTO>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager))
            .ReverseMap();

        CreateMap<UserDTO, UserResponse>()
            .ReverseMap();

        CreateMap<UserDTO, LoginRequest>().ReverseMap();
    }
}
