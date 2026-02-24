using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using System.IO;
using System.Text.Json;

namespace StarkCNC.Database.Helpers;

public class SettingsDbHelper : IDbHelper
{
    private readonly string _savePath;
    private readonly AppJsonContext _context;

    public SettingsDbHelper(string savePath, AppJsonContext context)
    {
        _savePath = savePath;
        _context = context;
    }

    public async void Read()
    {
        try
        {
            var currentSavePath = $"{_savePath}\\Settings";

            if (!Directory.Exists(currentSavePath))
                return;

            var files = Directory.GetFiles(currentSavePath);
            var file = files.FirstOrDefault(e => e.EndsWith(".json", StringComparison.CurrentCulture));

            if (file is null)
                return;

            var json = File.ReadAllText(file);
            var settings = JsonSerializer.Deserialize<Settings>(json);

            if (settings is null)
                return;

            await _context.AddAsync(settings).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch
        {
            // Ignore
        }
    }

    public async Task Save()
    {
        var toSave = await GetToSave().ConfigureAwait(false);

        if (toSave is null)
            return;

        var currentSavePath = $"{_savePath}\\Settings";
        if (!Directory.Exists(currentSavePath))
            Directory.CreateDirectory(currentSavePath);

        currentSavePath += $"\\{toSave.Id}.json";
        await Save(toSave, currentSavePath).ConfigureAwait(false);
    }

    public async Task Delete()
    {
        var toDelete = await GetToDelete().ConfigureAwait(false);

        if (toDelete is null)
            return;

        var currentSavePath = $"{_savePath}\\Settings";
        var filePath = $"{currentSavePath}\\{toDelete.Id}.json";
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    private async Task<Settings?> GetToSave() =>
        (await _context.Settings.ToListAsync().ConfigureAwait(false))
            .FirstOrDefault(e => _context.Entry(e).State != EntityState.Deleted);

    private static async Task Save(Settings settings, string currentSavePath)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(currentSavePath, json).ConfigureAwait(false);
    }

    private async Task<Settings?> GetToDelete() =>
        (await _context.Settings.ToListAsync().ConfigureAwait(false))
            .FirstOrDefault(e => _context.Entry(e).State == EntityState.Deleted);
}