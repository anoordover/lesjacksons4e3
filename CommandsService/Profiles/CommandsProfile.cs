using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId,
                opt => opt.MapFrom(
                    src => src.Id))
            .ForMember(dest => dest.Id,
                opt => opt.Ignore());
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dst => dst.Id,
                opt => opt.Ignore())
            .ForMember(dst => dst.ExternalId,
                opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dst => dst.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Commands,
                opt => opt.Ignore());
    }
}