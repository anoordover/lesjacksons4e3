using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlatformService.Models;

namespace PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Platform> Platforms { get; set; }
}