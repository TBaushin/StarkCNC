using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using System.IO;
using System.Text.Json;

namespace StarkCNC.Database;

public class AdjustmentDbHelper : IDbHelper
{
    private readonly string _savePath;
    private readonly AppJsonContext _context;

    public AdjustmentDbHelper(string savePath, AppJsonContext context)
    {
        _savePath = savePath;
        _context = context;
    }

    public async void Read()
    {
        try
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
                    await _context.Adjustments.AddAsync(adjustment).ConfigureAwait(false);
            }
        
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Ignore
        }
    }

    public async Task Save()
    {
        var toSave = await GetToSave().ConfigureAwait(false);

        foreach (var item in toSave)
        {
            var currentSavePath = _savePath + "\\Adjustments";
            if (!Directory.Exists(currentSavePath))
                Directory.CreateDirectory(currentSavePath);

            currentSavePath += "\\" + item.Name + ".json";
            await Save(item, currentSavePath).ConfigureAwait(false);
        }
    }

    public async Task Delete()
    {
        var toDelete = await GetToDelete().ConfigureAwait(false);

        var currentSavePath = _savePath + "\\Adjustments";
        foreach (var item in toDelete)
        {
            var filePath = currentSavePath + "\\" + item.Name + ".json";
            if (File.Exists(filePath))
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    // Ignore
                }
        }
    }

    private async Task<IEnumerable<AdjustmentParameters>> GetToSave() =>
        (await _context.Adjustments.ToListAsync().ConfigureAwait(false))
            .Where(a => _context.Entry(a).State != EntityState.Deleted);

    private static async Task Save(AdjustmentParameters adjustment, string currentSavePath)
    {
        var json = JsonSerializer.Serialize(adjustment, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            await File.WriteAllTextAsync(currentSavePath, json).ConfigureAwait(false);
        }
        catch
        {
            // Ignore
        }
    }

    private async Task<IEnumerable<AdjustmentParameters>> GetToDelete() =>
        (await _context.Adjustments.ToListAsync().ConfigureAwait(false))
            .Where(a => _context.Entry(a).State == EntityState.Deleted);
}
