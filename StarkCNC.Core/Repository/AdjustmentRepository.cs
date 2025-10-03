using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

    public AdjustmentRepository() { }

    public AdjustmentParameters AddElement(AdjustmentParameters adjustment)
    {
        _adjustments.Add(adjustment);
        return _adjustments.Last();
    }

    public void RemoveElement(Guid id)
    {
        var item = _adjustments.FirstOrDefault(a => a.Id == id);
        if (item is not null)
            _adjustments.Remove(item);
    }

    public void RemoveElement(AdjustmentParameters adjustment) => _adjustments.Remove(adjustment);

    public IEnumerable<AdjustmentParameters> FindByName(string name) =>
        _adjustments.Where(a => a.Name.Contains(name));

    public int Count() => _adjustments.Count;

    public IEnumerable<AdjustmentParameters> GetAll() => _adjustments;

    public void SetLevel(Guid id, int level)
    {
        var item = _adjustments.FirstOrDefault(a => a.Id == id);
        if (item is null)
            return;

        item.InstalledLevel = level;
    }

    public AdjustmentParameters? GetAdjustmentWithLevel(int level) =>
        _adjustments.FirstOrDefault(a => a.InstalledLevel == level);

    public IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel() =>
        _adjustments.Where(a => a.InstalledLevel > 0);
}
