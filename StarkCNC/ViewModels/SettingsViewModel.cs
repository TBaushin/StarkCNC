using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.ComponentModel;

namespace StarkCNC.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;

    public bool IsElectricBendingDrive
    {
        get => _settingsService.IsElectricBendingDrive;
        set
        {
            _settingsService.IsElectricBendingDrive = value;
            OnPropertyChanged(nameof(IsElectricBendingDrive));
        }
    }

    public IReadOnlyCollection<FloorType> Floors { get => _settingsService.FloorTypes; }

    public FloorType? SelectedFloorType
    {
        get => _settingsService.SelectedFloorType;
        set
        {
            _settingsService.SelectedFloorType = value;
            OnPropertyChanged(nameof(SelectedFloorType));
        }
    }

    public bool IsPunchingCylinder
    {
        get => _settingsService.IsPunchingCylinder;
        set
        {
            _settingsService.IsPunchingCylinder = value;
            OnPropertyChanged(nameof(IsPunchingCylinder));
        }
    }

    public bool IsElectricMachine
    {
        get => _settingsService.IsElectricMachine;
        set
        {
            _settingsService.IsElectricMachine = value;
            OnPropertyChanged(nameof(IsElectricMachine));
        }
    }

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
