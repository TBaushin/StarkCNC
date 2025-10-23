using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;

namespace StarkCNC.Database;

public class AppJsonContext : DbContext
{
    private readonly string _savePath;
    private readonly string _defaultFileName;

    private readonly List<IDbHelper> _helpers;

    public DbSet<AdjustmentParameters> Adjustments { get; set; }
    public DbSet<Settings> Settings { get; set; }

    public AppJsonContext(DbContextOptions<AppJsonContext> options, IConfiguration configuration) : base(options)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        var savePath = string.Empty;
        try
        {
            savePath = GetSavePath(configuration);
        }
        catch (ArgumentNullException)
        {
            // Ignore
        }
        if (string.IsNullOrEmpty(savePath))
            _savePath = AppDomain.CurrentDomain.BaseDirectory;
        else
            _savePath = savePath;

        var defaultFileName = string.Empty;
        try
        {
            defaultFileName = GetDefaultFileName(configuration);
        }
        catch (ArgumentNullException)
        {
            // Ignore
        }
        if (string.IsNullOrEmpty(defaultFileName))
            _defaultFileName = "settings.json";
        else
            _defaultFileName = defaultFileName;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder));

        modelBuilder.Entity<AdjustmentParameters>(entity =>
        {
            entity.HasOne(a => a.Bend).WithMany().HasForeignKey(a => a.BendId);
            entity.HasOne(a => a.BendRoller).WithMany().HasForeignKey(a => a.BendRollerId);
            entity.HasOne(a => a.Clamp).WithMany().HasForeignKey(a => a.ClampId);
            entity.HasOne(a => a.ClampRoller).WithMany().HasForeignKey(a => a.ClampRollerId);
            entity.HasOne(a => a.Console).WithMany().HasForeignKey(a => a.ConsoleId);
            entity.HasOne(a => a.Dorn).WithMany().HasForeignKey(a => a.DornId);
            entity.HasOne(a => a.Lift).WithMany().HasForeignKey(a => a.LiftId);
            entity.HasOne(a => a.Press).WithMany().HasForeignKey(a => a.PressId);
            entity.HasOne(a => a.Rotation).WithMany().HasForeignKey(a => a.RotationId);
            entity.HasOne(a => a.Squeeze).WithMany().HasForeignKey(a => a.SqueezeId);
            entity.HasOne(a => a.Supply).WithMany().HasForeignKey(a => a.SupplyId);
        });
    }

    private static string? GetSavePath(IConfiguration configuration)
    {
        var section = configuration.GetSection("SaveParameters");
        if (section is null)
            throw new ArgumentNullException(nameof(configuration));

        return section.GetSection("Path").Get<string>();
    }

    private static string? GetDefaultFileName(IConfiguration configuration)
    {
        var section = configuration.GetSection("SaveParameters");
        if (section is null)
            throw new ArgumentNullException(nameof(configuration));

        return section.GetSection("DefaultFileName").Get<string>();
    }
}
