using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository;

public interface IAdjustmentRepository
{
    AdjustmentParameters AddElement(AdjustmentParameters adjustment);

    void RemoveElement(Guid id);

    void RemoveElement(AdjustmentParameters adjustment);

    IEnumerable<AdjustmentParameters> FindByName(string name);

    int Count();

    IEnumerable<AdjustmentParameters> GetAll();

    void SetLevel(Guid id, int level);

    AdjustmentParameters? GetAdjustmentWithLevel(int level);

    IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel();
}
