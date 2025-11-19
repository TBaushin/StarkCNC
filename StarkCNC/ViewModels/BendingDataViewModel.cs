using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Models;

namespace StarkCNC.ViewModels;

public partial class BendingDataViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    /// <summary>
    /// Подача Y
    /// </summary>
    [ObservableProperty]
    private double _straightLength;

    /// <summary>
    /// Подача скорость Ys
    /// </summary>
    [ObservableProperty]
    private double _straightSpeed;

    /// <summary>
    /// Отвод Y1
    /// </summary>
    [ObservableProperty]
    private double _offset;

    /// <summary>
    /// Отвод скорость Y1b
    /// </summary>
    [ObservableProperty]
    private double _offsetSpeed;

    /// <summary>
    /// Отвод коэффициент Y2
    /// </summary>
    [ObservableProperty]
    private double _offsetCoefficient;

    /// <summary>
    /// Гиб угол C
    /// </summary>
    [ObservableProperty]
    private double _bendingAngle;

    /// <summary>
    /// Гиб скорость Cs
    /// </summary>
    [ObservableProperty]
    private double _bendingAngleSpeed;

    /// <summary>
    /// Гиб коэффициент Ck
    /// </summary>
    [ObservableProperty]
    private double _bendingAngleCoefficient;

    /// <summary>
    /// Радиус гиба R
    /// </summary>
    [ObservableProperty]
    private double _bendingRadius;

    /// <summary>
    /// Радиус гиба режим M
    /// </summary>
    [ObservableProperty]
    private string _bendingRadiusMode = string.Empty;

    /// <summary>
    /// Поворот угол B
    /// </summary>
    [ObservableProperty]
    private double _rotationAngle;

    /// <summary>
    /// Поворот скорость Bs
    /// </summary>
    [ObservableProperty]
    private double _rotationSpeed;

    public BendingDataViewModel() { }

    public BendingDataViewModel(int id, BendingData data)
    {
        Id = id;
        if (data is not null)
        {
            StraightLength = data.StraightLength;
            StraightSpeed = data.StraightSpeed;
            Offset = data.Offset;
            OffsetSpeed = data.OffsetSpeed;
            OffsetCoefficient = data.OffsetCoefficient;
            BendingAngle = data.BendingAngle;
            BendingAngleSpeed = data.BendingAngleSpeed;
            BendingAngleCoefficient = data.BendingAngleCoefficient;
            BendingRadius = data.BendingRadius;
            BendingRadiusMode = data.BendingRadiusMode;
            RotationAngle = data.RotationAngle;
            RotationSpeed = data.RotationSpeed;
        }
    }
}
