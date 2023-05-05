using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task<List<Platform>> getAllPlatforms(CancellationToken cancellationToken)
    {
        return _context.Platforms.ToListAsync(cancellationToken);
    }

    public Task<Platform?> GetPlatformById(int id, CancellationToken cancellationToken)
    {
        return _context.Platforms.FirstOrDefaultAsync(
            p => p.Id == id, cancellationToken);
    }

    public async Task CreatePlatform(Platform platform, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(platform));
        await _context.Platforms.AddAsync(platform, cancellationToken);
    }
}