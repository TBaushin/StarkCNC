using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using System.IO;
using System.Text.Json;

namespace StarkCNC.Database.Helpers;

public class UsersDbHelper : IDbHelper
{
    private readonly string _savePath;
    private readonly AppJsonContext _context;

    public UsersDbHelper(string savePath, AppJsonContext context)
    {
        _savePath = savePath;
        _context = context;
    }

    public async void Read()
    {
        try
        {
            var currentSavePath = $"{_savePath}\\Users";

            if (!Directory.Exists(currentSavePath))
                return;

            var files = Directory.GetFiles(currentSavePath);

            foreach (var file in files)
            {
                if (!file.EndsWith(".json", StringComparison.CurrentCulture))
                    continue;

                var json = File.ReadAllText(file);
                var user = JsonSerializer.Deserialize<User>(json);
                if (user is not null)
                    await _context.Users.AddAsync(user).ConfigureAwait(false);
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

        var currentSavePath = $"{_savePath}\\Users";
        if (!Directory.Exists(currentSavePath))
            Directory.CreateDirectory(currentSavePath);

        foreach (var item in toSave)
        {
            var filePath = $"{currentSavePath}\\{item.Id}.json";
            await Save(item, filePath).ConfigureAwait(false);
        }
    }

    public async Task Delete()
    {
        var toDelete = await GetToDelete().ConfigureAwait(false);

        var currentSavePath = $"{_savePath}\\Users";
        foreach (var item in toDelete)
        {
            var filePath = $"{currentSavePath}\\{item.Id}.json";
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

    private async Task<IEnumerable<User>> GetToSave() =>
        (await _context.Users.ToListAsync().ConfigureAwait(false))
            .Where(u => _context.Entry(u).State != EntityState.Deleted);

    private static async Task Save(User user, string currentSavePath)
    {
        var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            await File.WriteAllTextAsync(currentSavePath, json).ConfigureAwait(false);
        }
        catch
        {
            // Ignore
        }
    }

    private async Task<IEnumerable<User>> GetToDelete() =>
        (await _context.Users.ToListAsync().ConfigureAwait(false))
            .Where(u => _context.Entry(u).State == EntityState.Deleted);
}
