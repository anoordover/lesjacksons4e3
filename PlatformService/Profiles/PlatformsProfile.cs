using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dst => dst.PlatformId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Publisher,
                opt => opt.MapFrom(src => src.Publisher));
    }
}