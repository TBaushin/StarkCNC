using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.DTO;

public class BendingDataDto : INotifyPropertyChanged
{
    private BendingData _bendingData;
    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public double StraightLength
    {
        get => _bendingData.StraightLength;
        set
        {
            _bendingData.StraightLength = value;
            OnPropertyChanged(nameof(StraightLength));
        }
    }

    public double BendingAngle
    {
        get => _bendingData.BendingAngle;
        set
        {
            _bendingData.BendingAngle = value;
            OnPropertyChanged(nameof(BendingAngle));
        }
    }

    public double BendingRadius
    {
        get => _bendingData.BendingRadius;
        set
        {
            _bendingData.BendingRadius = value;
            OnPropertyChanged(nameof(BendingRadius));
        }
    }

    public double RotationAngle
    {
        get => _bendingData.RotationAngle;
        set
        {
            _bendingData.RotationAngle = value;
            OnPropertyChanged(nameof(RotationAngle));
        }
    }

    public BendingDataDto()
    {
        _bendingData = new BendingData();
    }

    public BendingDataDto(int id, BendingData bendingData)
    {
        Id = id;
        _bendingData = bendingData;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public BendingData Cast() => _bendingData;

    public override bool Equals(object? obj)
    {
        var data = obj as BendingDataDto;
        if (data is null)
            return false;

        return _bendingData.Equals(data.Cast());
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
