using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;

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
        var local = FindById(adjustment.Id);
        if (local is not null)
            _context.Entry(local).CurrentValues.SetValues(adjustment);
        else
            _context.Entry(adjustment).State = EntityState.Modified;

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public IEnumerable<AdjustmentParameters> FindByName(string name) =>
        _context.Adjustments.Where(a => a.Name.Contains(name, StringComparison.CurrentCulture));

    public AdjustmentParameters? FindById(Guid id) =>
        _context.Adjustments.FirstOrDefault(a => a.Id == id);

    public int Count() => _context.Adjustments.Count();

    public IEnumerable<AdjustmentParameters> GetAll() => _context.Adjustments.ToList();

    public async Task SetLevelAsync(Guid id, int level)
    {
        var item = _context.Adjustments.FirstOrDefault(a => a.Id == id);
        if (item is null)
            return;

        item.InstalledLevel = level;
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public AdjustmentParameters? GetAdjustmentWithLevel(int level) =>
        _context.Adjustments.FirstOrDefault(a => a.InstalledLevel == level);

    public IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel() =>
        _context.Adjustments.Where(a => a.InstalledLevel > 0);
}
