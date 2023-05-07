using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) >= 0;
    }

    public Task<List<Platform>> GetAllPlatforms(CancellationToken cancellationToken)
    {
        return _context.Platforms
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public void CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        _context.Platforms.Attach(platform);
    }

    public Task<bool> PlatformExists(int platformId, CancellationToken cancellationToken)
    {
        return _context.Platforms
            .AnyAsync(p => p.Id == platformId, cancellationToken);
    }

    public Task<bool> ExternalPlatformExists(int externalPlatformId, CancellationToken cancellationToken)
    {
        return _context.Platforms.AnyAsync(
            p => p.ExternalId == externalPlatformId,
            cancellationToken);
    }

    public Task<List<Command>> GetCommandsForPlatform(int platformId, CancellationToken cancellationToken)
    {
        return _context.Commands
            .Where(c => c.PlatformId == platformId)
            .ToListAsync(cancellationToken);
    }

    public Task<Command?> GetCommand(int platformId, int commandId, CancellationToken cancellationToken)
    {
        return _context.Commands
            .Where(c => c.PlatformId == platformId &&
                        c.Id == commandId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void CreateCommand(int platformId, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        command.PlatformId = platformId;
        _context.Commands.Attach(command);
    }
}