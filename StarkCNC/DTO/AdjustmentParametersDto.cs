using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.DTO;

public class AdjustmentParametersDto : INotifyPropertyChanged
{
    private readonly AdjustmentParameters _adjustment;

    public string Name
    {
        get => _adjustment.Name;
        set
        {
            _adjustment.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public double PipeDiameter
    {
        get => _adjustment.PipeDiameter;
        set
        {
            _adjustment.PipeDiameter = value;
            OnPropertyChanged(nameof(PipeDiameter));
        }
    }

    public double Radius
    {
        get => _adjustment.Radius;
        set
        {
            _adjustment.Radius = value;
            OnPropertyChanged(nameof(Radius));
        }
    }

    public AdjustmentType Type
    {
        get => _adjustment.Type;
        set
        {
            _adjustment.Type = value;
            OnPropertyChanged(nameof(Type));
        }
    }

    public int InstalledLevel
    {
        get => _adjustment.InstalledLevel;
    }

    public double ClampLength
    {
        get => _adjustment.ClampLength;
        set
        {
            _adjustment.ClampLength = value;
            OnPropertyChanged(nameof(ClampLength));
        }
    }

    public double PressLength
    {
        get => _adjustment.PressLength;
        set
        {
            _adjustment.PressLength = value;
            OnPropertyChanged(nameof(PressLength));
        }
    }

    public AdjustmentParametersDto(AdjustmentParameters adjustment)
    {
        _adjustment = adjustment;
    }

    public AdjustmentParametersDto(string name, AdjustmentType type)
    {
        _adjustment = new AdjustmentParameters(name, type);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AdjustmentParameters Cast() => _adjustment;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
