using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;

namespace StarkCNC.Repository;

public class SettingsRepository : ISettingsRepository
{
    private IDbContextFactory<AppJsonContext> _contextFactory;

    public SettingsRepository(IDbContextFactory<AppJsonContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Settings?> AddElementAsync(Settings settings)
    {
        var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
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

        var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

        context.Entry(settings).State = EntityState.Modified;

        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Settings?> GetAsync()
    {
        var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Settings.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<Settings?> FindByIdAsync(Guid id)
    {
        var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Settings.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
    }

    public int Count()
    {
        var context = _contextFactory.CreateDbContext();
        return context.Settings.Count();
    }
}