using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Settings;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public interface ISettingsService : INotifyPropertyChanged
{
    IReadOnlyCollection<FloorType> FloorTypes { get; }

    FloorType? SelectedFloorType { get; set; }

    bool IsElectricBendingDrive { get; set; }

    bool IsPunchingCylinder { get; set; }

    bool IsElectricMachine { get; set; }

    /// <summary>
    /// Ползунок скорости
    /// </summary>
    double Speed { get; set; }

    double SynchronizationCoefficient { get; set; }

    bool InterceptionMode { get; set; }

    Bend Bend { get; }

    Dorn Dorn { get; }

    Rotation Rotation { get; }

    Support Support { get; }

    Supply Supply { get; }

    StarkCNC.Core.Models.Settings.Console Console { get; }

    Pipe Pipe { get; }

    Task SaveAsync();

    Task ReadAsync();
}
