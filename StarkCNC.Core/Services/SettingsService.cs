using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Settings;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public class SettingsService : ISettingsService // Вынести в класс Settings и заменить Service на Repository
{
    private FloorType? _selectedFloorType;
    private bool _isElectricBendingDrive = false;
    private bool _isPunchingCylinder = false;
    private bool _isElectricMachine = false;
    private double _speed;
    private double _synchronizationCoefficient;
    private bool _interceptionMode = false;

    private string _speedRequestString = string.Empty;
    private string _synchronizationCoefficientRequestString = string.Empty;
    private string _interceptionModeRequestString = string.Empty;

    public IReadOnlyCollection<FloorType> FloorTypes { get; } // TODO: ENUM

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

    public double Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            OnPropertyChanged(nameof(Speed));
        }
    }

    public double SynchronizationCoefficient
    {
        get => _synchronizationCoefficient;
        set
        {
            _synchronizationCoefficient = value;
            OnPropertyChanged(nameof(SynchronizationCoefficient));
        }
    }

    public bool InterceptionMode
    {
        get => _interceptionMode;
        set
        {
            _interceptionMode = value;
            OnPropertyChanged(nameof(InterceptionMode));
        }
    }
    
    public Bend Bend { get; }
    public Dorn Dorn { get; }
    public Rotation Rotation { get; }
    public Support Support { get; }
    public Supply Supply { get; }
    public StarkCNC.Core.Models.Settings.Console Console { get; }
    public Pipe Pipe { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsService(IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection("Settings");
        FloorTypes = ReadLevels(settingsSection).ToList();

        ReadSpeed(settingsSection);
        ReadSynchronizationCoefficient(settingsSection);
        ReadInterceptionMode(settingsSection);

        Bend = Bend.ReadConfiguration(settingsSection);
        Dorn = Dorn.ReadConfiguration(settingsSection);
        Rotation = Rotation.ReadConfiguration(settingsSection);
        Support = Support.ReadConfiguration(settingsSection);
        Supply = Supply.ReadConfiguration(settingsSection);
        Console = StarkCNC.Core.Models.Settings.Console.ReadConfiguration(settingsSection);
        Pipe = Pipe.ReadConfiguration(settingsSection);
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public Task ReadAsync()
    {
        throw new NotImplementedException();
    }

    private static ICollection<FloorType> ReadLevels(IConfigurationSection section)
    {
        var floors = section.GetSection("Levels").Get<ICollection<FloorType>>();
        if (floors is null)
            floors = new List<FloorType>()
            {
                new FloorType("Одноуровневый", 1), // TODO: Вынести в Localization
                new FloorType("Двухуровневый", 2), // TODO: Вынести в Localization
                new FloorType("Трёхуровневый", 3) // TODO: Вынести в Localization
            };

        return floors;
    }

    private void ReadSpeed(IConfigurationSection section)
    {
        var s = section.GetSection("Speed");
        _speedRequestString = s.GetSection("RequestString").Get<string>() ?? string.Empty;
        Speed = s.GetSection("Default").Get<double>();
    }

    private void ReadSynchronizationCoefficient(IConfigurationSection section)
    {
        var sc = section.GetSection("SynchronizationCoefficient");
        _synchronizationCoefficientRequestString = sc.GetSection("RequestString").Get<string>() ?? string.Empty;
        SynchronizationCoefficient = sc.GetSection("Default").Get<double>();
    }

    private void ReadInterceptionMode(IConfigurationSection section)
    {
        var im = section.GetSection("InterceptionMode");
        _interceptionModeRequestString = im.GetSection("RequestString").Get<string>() ?? string.Empty;
        _interceptionMode = im.GetSection("Default").Get<bool>();
    }

    private void OnPropertyChanged(string propertyName) // TODO отказаться от PropertyChanged и создать Dto
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
