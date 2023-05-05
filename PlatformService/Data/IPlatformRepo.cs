using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepo
{
    Task<bool> SaveChanges(CancellationToken cancellationToken);

    Task<List<Platform>> getAllPlatforms(CancellationToken cancellationToken);

    Task<Platform?> GetPlatformById(int id, CancellationToken cancellationToken);

    Task CreatePlatform(Platform platform, CancellationToken cancellationToken);
}