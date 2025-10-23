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

    public async Task AddElementAsync(Settings settings)
    {
        if (!ISettingsRepository.CanBeAdded(await _context.Settings.ToListAsync().ConfigureAwait(false)))
            return;

        await _context.AddAsync(settings).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateElementAsync(Settings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var local = await FindByIdAsync(settings.Id).ConfigureAwait(false);
        if (local is not null)
            _context.Entry(local).CurrentValues.SetValues(settings);
        else
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