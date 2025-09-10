using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Core.Repository;

public interface IAdjustmentRepository
{
    event PropertyChangedEventHandler PropertyChanged;

    AdjustmentParameters AddElement(string name, AdjustmentType type);

    AdjustmentParameters AddElement(AdjustmentParameters adjustment);

    void RemoveElement(string name);

    void RemoveElement(AdjustmentParameters adjustment);

    IEnumerable<AdjustmentParameters> GetTenElements(int startPostion = 0);

    IEnumerable<AdjustmentParameters> FindByName(string name);

    int Count();

    IEnumerable<AdjustmentParameters> GetAll();

    IEnumerable<AdjustmentType> GetTypes();

    void SetLevel(AdjustmentParameters adjustment, int level);

    AdjustmentParameters? GetAdjustmentWithLevel(int level);

    IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel();
}
