using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;

namespace StarkCNC.Database;

public class AppJsonContext : DbContext
{
    private readonly string _savePath;

    private readonly List<IDbHelper> _helpers;

    public DbSet<AdjustmentParameters> Adjustments { get; set; }
    public DbSet<Settings> Settings { get; set; }

    public AppJsonContext(DbContextOptions<AppJsonContext> options, IConfiguration configuration) : base(options)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        _savePath = GetSavePath(configuration);

        Database.EnsureCreated();

        _helpers = new List<IDbHelper>() { new AdjustmentDbHelper(_savePath, this), new SettingsDbHelper(_savePath, this) };

        try
        {
            _helpers.ForEach(e => e.Read());
        }
        catch
        {
            // Ignore
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _helpers.ForEach(async e => await e.Delete().ConfigureAwait(false));
        var r = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _helpers.ForEach(async e => await e.Save().ConfigureAwait(false));

        return r;
    }

    private static string GetSavePath(IConfiguration configuration)
    {
        var section = configuration.GetSection("SaveParameters");
        if (section is null)
            return AppDomain.CurrentDomain.BaseDirectory;

        var savePath = string.Empty;
        savePath = section.GetSection("Path").Get<string>();

        if (string.IsNullOrEmpty(savePath))
            return AppDomain.CurrentDomain.BaseDirectory;
        else
            return savePath;
    }
}
