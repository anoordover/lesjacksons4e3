using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var platformItems = await _repository.getAllPlatforms(context.CancellationToken);
        var response = new PlatformResponse();
        foreach (var platformItem in platformItems)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platformItem));
        }
        return response;
    }
}