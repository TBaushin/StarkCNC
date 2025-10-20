using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface IAdjustmentRepository
{
    Task AddElementAsync(AdjustmentParameters adjustment);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(AdjustmentParameters adjustment);

    Task UpdateElementAsync(AdjustmentParameters adjustment);

    IEnumerable<AdjustmentParameters> FindByName(string name);

    AdjustmentParameters? FindById(Guid id);

    int Count();

    IEnumerable<AdjustmentParameters> GetAll();

    Task SetLevelAsync(Guid id, int level);

    AdjustmentParameters? GetAdjustmentWithLevel(int level);

    IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel();
}
