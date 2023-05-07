using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepo
{
    Task<bool> SaveChanges(CancellationToken cancellationToken);

    Task<List<Platform>> GetAllPlatforms(CancellationToken cancellationToken);
    void CreatePlatform(Platform platform);
    Task<bool> PlatformExists(int platformId,
        CancellationToken cancellationToken);
    Task<bool> ExternalPlatformExists(int externalPlatformId,
        CancellationToken cancellationToken);

    Task<List<Command>> GetCommandsForPlatform(int platformId,
        CancellationToken cancellationToken);
    Task<Command?> GetCommand(int platformId, int commandId, CancellationToken cancellationToken);
    void CreateCommand(int platformId, Command command);
}