using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface IAdjustmentRepository
{
    Task<AdjustmentParameters> AddElementAsync(AdjustmentParameters adjustment);

    Task RemoveElementAsync(Guid id);

    Task RemoveElementAsync(AdjustmentParameters adjustment);

    Task UpdateElementAsync(AdjustmentParameters adjustment);

    Task<IEnumerable<AdjustmentParameters>> FindByNameAsync(string name);

    Task<AdjustmentParameters?> FindByIdAsync(Guid id);

    int Count();

    Task<IEnumerable<AdjustmentParameters>> GetAllAsync();

    Task SetLevelAsync(Guid id, int level);

    Task<AdjustmentParameters?> GetAdjustmentWithLevelAsync(int level);

    Task<IEnumerable<AdjustmentParameters>> GetAdjustmentsWithLevelAsync();
}
