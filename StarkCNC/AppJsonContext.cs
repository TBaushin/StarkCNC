using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace StarkCNC;

public class AppJsonContext : DbContext
{
    private readonly string _savePath;
    private readonly string _defaultFileName;

    public DbSet<AdjustmentParameters> Adjustments { get; set; }

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

        ReadAdjustments();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DeleteAdjsutments();
        var r = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await SaveAdjustments().ConfigureAwait(false); // Записывает новые

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

    [SuppressMessage("Usage", "CA1031", Justification = "Плохое решение, но по другому нельзя")]
    private void ReadAdjustments()
    {
        var currentSavePath = _savePath + "\\Adjustments";

        if (!Directory.Exists(currentSavePath))
            return;

        var files = Directory.GetFiles(currentSavePath);

        foreach (var file in files)
        {
            if (!file.EndsWith(".json", StringComparison.CurrentCulture))
                continue;

            var json = File.ReadAllText(file);
            var adjustment = JsonSerializer.Deserialize<AdjustmentParameters>(json);
            if (adjustment is not null)
                Adjustments.Add(adjustment);
        }

        try
        {
            SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Ignore
        }
    }

    private async Task SaveAdjustments()
    {
        var toSave = GetToSaveAdjustments();

        foreach (var item in toSave)
        {
            var currentSavePath = _savePath + "\\Adjustments";
            if (!Directory.Exists(currentSavePath))
                Directory.CreateDirectory(currentSavePath);

            currentSavePath += "\\" + item.Name + ".json";
            await SaveAdjustment(item, currentSavePath).ConfigureAwait(false);
        }
    }

    private IEnumerable<AdjustmentParameters> GetToSaveAdjustments()
    {
        var toSave = new List<AdjustmentParameters>();

        foreach (var a in Adjustments)
        {
            if (Entry(a).State != EntityState.Deleted)
                toSave.Add(a);
        }

        return toSave;
    }

    private static async Task SaveAdjustment(AdjustmentParameters adjustment, string currentSavePath)
    {
        var json = JsonSerializer.Serialize(adjustment, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(currentSavePath, json).ConfigureAwait(false);
    }

    private void DeleteAdjsutments()
    {
        var toDelete = GetToDeleteAdjustments();

        foreach (var item in toDelete)
        {
            var currentSavePath = _savePath + "\\Adjustments";
            var filePath = currentSavePath + "\\" + item.Name + ".json";
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    private IEnumerable<AdjustmentParameters> GetToDeleteAdjustments()
    {
        var toDelete = new List<AdjustmentParameters>();

        foreach (var a in Adjustments)
        {
            if (Entry(a).State == EntityState.Deleted)
                toDelete.Add(a);
        }

        return toDelete;
    }
}
