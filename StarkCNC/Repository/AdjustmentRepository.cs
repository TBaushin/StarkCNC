using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;

namespace StarkCNC.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private AppJsonContext _context;

    public AdjustmentRepository(AppJsonContext context)
    {
        _context = context;
    }

    public async Task AddElementAsync(AdjustmentParameters adjustment)
    {
        await _context.AddAsync(adjustment).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RemoveElementAsync(Guid id)
    {
        var item = _context.Adjustments.FirstOrDefault(a => a.Id == id);
        if (item is not null)
        {
            _context.Adjustments.Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public async Task RemoveElementAsync(AdjustmentParameters adjustment)
    {
        _context.Adjustments.Remove(adjustment);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateElementAsync(AdjustmentParameters adjustment)
    {
        var local = await FindByIdAsync(adjustment.Id).ConfigureAwait(false);
        if (local is not null)
            _context.Entry(local).CurrentValues.SetValues(adjustment);
        else
            _context.Entry(adjustment).State = EntityState.Modified;

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<AdjustmentParameters>> FindByNameAsync(string name) =>
        await _context.Adjustments.Where(a => a.Name.Contains(name, StringComparison.CurrentCulture)).ToListAsync().ConfigureAwait(false);

    public async Task<AdjustmentParameters?> FindByIdAsync(Guid id) =>
        await _context.Adjustments.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);

    public int Count() => _context.Adjustments.Count();

    public async Task<IEnumerable<AdjustmentParameters>> GetAllAsync() => await _context.Adjustments.ToListAsync().ConfigureAwait(false);

    public async Task SetLevelAsync(Guid id, int level)
    {
        var item = await _context.Adjustments.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        if (item is null)
            return;

        item.InstalledLevel = level;
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<AdjustmentParameters?> GetAdjustmentWithLevelAsync(int level) =>
        await _context.Adjustments.FirstOrDefaultAsync(a => a.InstalledLevel == level).ConfigureAwait(false);

    public async Task<IEnumerable<AdjustmentParameters>> GetAdjustmentsWithLevelAsync() =>
        await _context.Adjustments.Where(a => a.InstalledLevel > 0).ToListAsync().ConfigureAwait(false);
}
