using StarkCNC.Core;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;

namespace StarkCNC.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private AppJsonContext _context;
    private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

    public AdjustmentRepository(AppJsonContext context)
    {
        _context = context;
    }

    public async Task<AdjustmentParameters> AddElementAsync(AdjustmentParameters adjustment)
    {
        var a = _context.Add(adjustment);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return a;
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

    public IEnumerable<AdjustmentParameters> FindByName(string name) =>
        _context.Adjustments.Where(a => a.Name.Contains(name, StringComparison.CurrentCulture));

    public int Count() => _adjustments.Count;

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
