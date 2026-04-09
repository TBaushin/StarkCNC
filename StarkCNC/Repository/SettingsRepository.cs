using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;
using StarkCNC.Database.Helpers;

namespace StarkCNC.Repository;

public class SettingsRepository : ISettingsRepository
{
    private IDbContextFactory<AppJsonContext> _contextFactory;
    private readonly string basePath;

    public SettingsRepository(IDbContextFactory<AppJsonContext> contextFactory, IConfiguration configuration)
    {
        _contextFactory = contextFactory;

        basePath = GetSavePath(configuration);
        Initialize();
    }

    private async void Initialize()
    {
        var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var exsistingSettings = context.Settings?.Select(s => s.Id).ToHashSet() ?? new HashSet<Guid>();
        foreach (var element in IDbHelper.Read<Settings>($"{basePath}\\Settings"))
        {
            if (!exsistingSettings.Contains(element.Id))
                context.Settings?.Add(element);
        }

        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    private static string GetSavePath(IConfiguration configuration)
    {
        var savePath = configuration.GetValue<string>("Settings:SaveParameters:Path") ?? string.Empty;
        if (string.IsNullOrEmpty(savePath))
            return AppDomain.CurrentDomain.BaseDirectory;
        return savePath;
    }

    public async Task<Settings?> AddElementAsync(Settings settings)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var canBeSave = await context.Settings.AnyAsync().ConfigureAwait(false);
        if (canBeSave)
            return null;

        await context.AddAsync(settings).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return settings;
    }

    public async Task UpdateElementAsync(Settings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

        var exists = await context.Settings.FirstOrDefaultAsync(s => s.Id == settings.Id).ConfigureAwait(false);
        if (exists is null)
            return;

        context.Entry(exists).CurrentValues.SetValues(settings);

        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Settings?> GetAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
    }

    public async Task<Settings?> FindByIdAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id)
            .ConfigureAwait(false);
    }

    public int Count()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Settings.Count();
    }
}