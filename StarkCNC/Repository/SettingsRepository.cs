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

        var local = await FindByIdAsync(settings.Id).ConfigureAwait(false);
        if (local is not null)
        {
            _context.Entry(local).CurrentValues.SetValues(settings);
            UpdateLocalEntry(settings, local);
        }
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

    private void UpdateLocalEntry(Settings settings, Settings local)
    {
        _context.Entry(local.Bend).CurrentValues.SetValues(settings.Bend);
        _context.Entry(local.Dorn).CurrentValues.SetValues(settings.Dorn);
        _context.Entry(local.Rotation).CurrentValues.SetValues(settings.Rotation);
        _context.Entry(local.Support).CurrentValues.SetValues(settings.Support);
        _context.Entry(local.Supply).CurrentValues.SetValues(settings.Supply);
        _context.Entry(local.Console).CurrentValues.SetValues(settings.Console);
        _context.Entry(local.Pipe).CurrentValues.SetValues(settings.Pipe);
    }
}