using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public interface ISettingsService : INotifyPropertyChanged
{
    IReadOnlyCollection<FloorType> FloorTypes { get; }

    FloorType? SelectedFloorType { get; set; }

    bool IsElectricBendingDrive { get; set; }

    bool IsPunchingCylinder { get; set; }

    bool IsElectricMachine { get; set; }

    new event PropertyChangedEventHandler? PropertyChanged;
}
