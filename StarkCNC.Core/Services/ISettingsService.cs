using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services
{
    public interface ISettingsService
    {
        IReadOnlyCollection<FloorType> FloorTypes { get; }

        FloorType? SelectedFloorType { get; set; }

        bool IsElectricBendingDrive { get; set; }

        bool IsPunchingCylinder { get; set; }

        bool IsElectricMachine { get; set; }
    }
}
