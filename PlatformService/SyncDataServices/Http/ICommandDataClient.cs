using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platformReadDto,
        CancellationToken cancellationToken);
}