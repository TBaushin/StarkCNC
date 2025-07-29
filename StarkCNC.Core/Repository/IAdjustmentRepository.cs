using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository
{
    public interface IAdjustmentRepository
    {
        AdjustmentParameters AddElement(string name);

        AdjustmentParameters AddElement(AdjustmentParameters adjustment);

        void RemoveElement(string name);

        void RemoveElement(AdjustmentParameters adjustment);

        IEnumerable<AdjustmentParameters> GetTenElements(int startPostion = 0);

        IEnumerable<AdjustmentParameters> FindByName(string name);

        int Count();

        IEnumerable<AdjustmentParameters> GetAll();
    }
}
