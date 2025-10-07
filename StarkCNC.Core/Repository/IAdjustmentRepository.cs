using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface IAdjustmentRepository
{
    Task<AdjustmentParameters> AddElementAsync(AdjustmentParameters adjustment);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(AdjustmentParameters adjustment);

    IEnumerable<AdjustmentParameters> FindByName(string name);

    int Count();

    IEnumerable<AdjustmentParameters> GetAll();

    Task SetLevelAsync(Guid id, int level);

    AdjustmentParameters? GetAdjustmentWithLevel(int level);

    IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel();
}
