using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public class SettingsService : ISettingsService
{
    private FloorType? _selectedFloorType;
    private bool _isElectricBendingDrive = false;
    private bool _isPunchingCylinder = false;
    private bool _isElectricMachine = false;

    public IReadOnlyCollection<FloorType> FloorTypes { get; }

    public FloorType? SelectedFloorType
    {
        get => _selectedFloorType;
        set
        {
            _selectedFloorType = value;
            OnPropertyChanged(nameof(SelectedFloorType));
        }
    }

    public bool IsElectricBendingDrive
    {
        get => _isElectricBendingDrive;
        set
        {
            _isElectricBendingDrive = value;
            OnPropertyChanged(nameof(IsElectricBendingDrive));
        }
    }

    public bool IsPunchingCylinder
    {
        get => _isPunchingCylinder;
        set
        {
            _isPunchingCylinder = value;
            OnPropertyChanged(nameof(IsPunchingCylinder));
        }
    }

    public bool IsElectricMachine
    {
        get => _isElectricMachine;
        set
        {
            _isElectricMachine = value;
            OnPropertyChanged(nameof(IsElectricMachine));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsService(IConfiguration configuration)
    {
        FloorTypes = ReadLevels(configuration).ToList();
    }

    private static ICollection<FloorType> ReadLevels(IConfiguration configuration)
    {
        var floors = configuration.GetSection("Settings").GetSection("Levels").Get<ICollection<FloorType>>();
        if (floors is null)
            floors = new List<FloorType>()
            {
                new FloorType("Одноуровневый", 1), // TODO: Вынести в Localization
                new FloorType("Двухуровневый", 2), // TODO: Вынести в Localization
                new FloorType("Трёхуровневый", 3) // TODO: Вынести в Localization
            };

        return floors;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
