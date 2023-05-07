using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        
        var grpcClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
        var platforms = grpcClient.ReturnAllPlatforms();
        SeedData(scope.ServiceProvider.GetRequiredService<ICommandRepo>(),
            platforms).Wait();
    }

    private static async Task SeedData(ICommandRepo repo,
        IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new platforms");
        foreach (var platform in platforms)
        {
            if (!await repo.ExternalPlatformExists(platform.ExternalId, CancellationToken.None))
            {
                repo.CreatePlatform(platform);
            }
            await repo.SaveChanges(CancellationToken.None);
        }
    }
}