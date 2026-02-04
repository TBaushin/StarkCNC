using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public interface IAdjustmentService
{
    AdjustmentParameters? FirstLevelAdjustment { get; }
    AdjustmentParameters? SecondLevelAdjustment { get; }
    AdjustmentParameters? ThirdLevelAdjustment { get; }

    int CurrentLevel { get; }

    Task<AdjustmentParameters?> AddElementAsync(AdjustmentParameters adjustment);

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

    Task AdjustmentStartUp();
    Task AdjustmentStopUp();

    Task AdjustmentStartDown();
    Task AdjustmentStopDown();
}
