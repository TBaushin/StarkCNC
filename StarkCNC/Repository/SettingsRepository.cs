using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;

namespace StarkCNC.Repository;

public class SettingsRepository : ISettingsRepository
{
    private AppJsonContext _context;

    public SettingsRepository(AppJsonContext context)
    {
        _context = context;
    }

    public async Task<Settings?> AddElementAsync(Settings settings)
    {
        var canBeSave = await _context.Settings.AnyAsync().ConfigureAwait(false);
        if (canBeSave)
            return null;

        await _context.AddAsync(settings).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return settings;
    }

    public async Task UpdateElementAsync(Settings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        _context.Entry(settings).State = EntityState.Modified;

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Settings?> GetAsync() =>
        await _context.Settings.FirstOrDefaultAsync().ConfigureAwait(false);

    public async Task<Settings?> FindByIdAsync(Guid id) =>
        await _context.Settings.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);

    public int Count() =>
        _context.Settings.Count();
}