using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;

namespace StarkCNC.Tests.Fakes.Repositories;

internal class FakeAdjustmentRepository : IAdjustmentRepository
{
    private List<AdjustmentParameters> _db = new List<AdjustmentParameters>();

    public Task<AdjustmentParameters?> AddElementAsync(AdjustmentParameters adjustment)
    {
        if (adjustment.Id == Guid.Empty)
            adjustment.Id = Guid.NewGuid();
        _db.Add(adjustment);
        return Task.FromResult(adjustment);
    }

    public int Count() => _db.Count();

    public Task<AdjustmentParameters?> FindByIdAsync(Guid id)
    {
        var item = _db.Find(a => a.Id == id);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<AdjustmentParameters>> FindByNameAsync(string name)
    {
        IEnumerable<AdjustmentParameters> items = _db.FindAll(a => a.Name == name);
        return Task.FromResult(items);
    }

    public Task<IEnumerable<AdjustmentParameters>> GetAdjustmentsWithLevelAsync()
    {
        IEnumerable<AdjustmentParameters> items = _db.FindAll(a => a.InstalledLevel > 0);
        return Task.FromResult(items);
    }

    public Task<AdjustmentParameters?> GetAdjustmentWithLevelAsync(int level)
    {
        var item = _db.Find(a => a.InstalledLevel == level);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<AdjustmentParameters>> GetAllAsync()
    {
        IEnumerable<AdjustmentParameters> items = _db;
        return Task.FromResult(items);
    }

    public Task RemoveElementAsync(Guid id)
    {
        var item = _db.Find(a => a.Id == id);
        if (item is not null)
            _db.Remove(item);

        return Task.CompletedTask;
    }

    public Task RemoveElementAsync(AdjustmentParameters adjustment)
    {
        _db.Remove(adjustment);
        return Task.CompletedTask;
    }

    public Task UpdateElementAsync(AdjustmentParameters adjustment)
    {
        return Task.CompletedTask;
    }
}
