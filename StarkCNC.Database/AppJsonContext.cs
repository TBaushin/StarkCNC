using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Database.Helpers;

namespace StarkCNC.Database;

public class AppJsonContext : DbContext
{
    public DbSet<AdjustmentParameters> Adjustments { get; set; }

    public DbSet<Settings> Settings { get; set; }

    public AppJsonContext(DbContextOptions<AppJsonContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}