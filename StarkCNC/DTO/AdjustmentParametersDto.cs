using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Adjustment;
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

    public double ForwardDangerZoneCoordinate
    {
        get => _adjustment.ForwardDangerZoneCoordinate;
        set
        {
            _adjustment.ForwardDangerZoneCoordinate = value;
            OnPropertyChanged(nameof(ForwardDangerZoneCoordinate));
        }
    }

    public double DistanceFromCenter
    {
        get => _adjustment.DistanceFromCenter;
        set
        {
            _adjustment.DistanceFromCenter = value;
            OnPropertyChanged(nameof(DistanceFromCenter));
        }
    }

    public BendRoller BendRoller
    {
        get => _adjustment.BendRoller;
        set
        {
            _adjustment.BendRoller = value;
            OnPropertyChanged(nameof(BendRoller));
        }
    }

    public Clamp Clamp
    {
        get => _adjustment.Clamp;
        set
        {
            _adjustment.Clamp = value;
            OnPropertyChanged(nameof(Clamp));
        }
    }

    public ClampRoller ClampRoller
    {
        get => _adjustment.ClampRoller;
        set
        {
            _adjustment.ClampRoller = value;
            OnPropertyChanged(nameof(ClampRoller));
        }
    }

    public StarkCNC.Core.Models.Adjustment.Console Console
    {
        get => _adjustment.Console;
        set
        {
            _adjustment.Console = value;
            OnPropertyChanged(nameof(Console));
        }
    }

    public Press Press
    {
        get => _adjustment.Press;
        set
        {
            _adjustment.Press = value;
            OnPropertyChanged(nameof(Press));
        }
    }

    public Squeeze Squeeze
    {
        get => _adjustment.Squeeze;
        set
        {
            _adjustment.Squeeze = value;
            OnPropertyChanged(nameof(Squeeze));
        }
    }

    public AdjustmentParametersDto(AdjustmentParameters adjustment)
    {
        _adjustment = adjustment;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public override bool Equals(object? obj)
    {
        var adjustment = obj as AdjustmentParametersDto;
        if (adjustment is null)
            return false;

        return _adjustment.Equals(adjustment.Cast());
    }

    public override int GetHashCode() =>
        _adjustment.GetHashCode();

    public AdjustmentParameters Cast() => _adjustment;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
