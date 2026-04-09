using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Database.Helpers;

namespace StarkCNC.Database;

public class AppJsonContext : DbContext
{
    private static readonly IConfiguration _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
        .Build();

    private static readonly string basePath = GetSavePath();

    public DbSet<AdjustmentParameters> Adjustments { get; set; }

    public DbSet<Settings> Settings { get; set; }

    public AppJsonContext(DbContextOptions<AppJsonContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var r = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var adjustments = await Adjustments.AsNoTracking().IncludeAll(this).ToListAsync().ConfigureAwait(false);
        var settings = await Settings.AsNoTracking().IncludeAll(this).ToListAsync().ConfigureAwait(false);

        await IDbHelper.Delete($"{basePath}\\Adjustments", adjustments, e => e.Id).ConfigureAwait(false);
        await IDbHelper.Delete($"{basePath}\\Settings", settings, e => e.Id).ConfigureAwait(false);

        await IDbHelper.Save($"{basePath}\\Adjustments", adjustments).ConfigureAwait(false);
        await IDbHelper.Save($"{basePath}\\Settings", settings).ConfigureAwait(false);

        return r;
    }

    private static string GetSavePath()
    {
        var savePath = _configuration.GetValue<string>("Settings:SaveParameters:Path") ?? string.Empty;
        if (string.IsNullOrEmpty(savePath))
            return AppDomain.CurrentDomain.BaseDirectory;
        return savePath;
    }
}