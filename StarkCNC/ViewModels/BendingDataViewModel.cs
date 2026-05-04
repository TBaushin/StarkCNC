using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Models;

namespace StarkCNC.ViewModels;

public partial class BendingDataViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    /// <summary>
    /// Длина трубы
    /// </summary>
    [ObservableProperty]
    private float _pipeLength;

    /// <summary>
    /// Установка Y
    /// </summary>
    [ObservableProperty]
    private float _ySetup;

    /// <summary>
    /// Длина уезда цанги
    /// </summary>
    [ObservableProperty]
    private float _colletOffsetLength;

    /// <summary>
    /// Подача Y
    /// </summary>
    [ObservableProperty]
    private float _supply;

    /// <summary>
    /// Подача скорость Ys
    /// </summary>
    [ObservableProperty]
    private float _supplySpeed = 100;

    /// <summary>
    /// Отвод Y1
    /// </summary>
    [ObservableProperty]
    private float _offset;

    /// <summary>
    /// Отвод скорость Y1b
    /// </summary>
    [ObservableProperty]
    private float _offsetSpeed;

    /// <summary>
    /// Отвод коэффициент Y2
    /// </summary>
    [ObservableProperty]
    private float _offsetCoefficient;

    /// <summary>
    /// Гиб угол C
    /// </summary>
    [ObservableProperty]
    private float _bendingAngle;

    /// <summary>
    /// Гиб скорость Cs
    /// </summary>
    [ObservableProperty]
    private float _bendingAngleSpeed = 100;

    /// <summary>
    /// Гиб коэффициент Ck
    /// </summary>
    [ObservableProperty]
    private float _bendingAngleCoefficient;

    /// <summary>
    /// Радиус гиба R
    /// </summary>
    [ObservableProperty]
    private float _bendingRadius;

    /// <summary>
    /// Радиус гиба режим M
    /// </summary>
    [ObservableProperty]
    private string _bendingRadiusMode = "Гибка";

    /// <summary>
    /// Поворот угол B
    /// </summary>
    [ObservableProperty]
    private float _rotationAngle;

    /// <summary>
    /// Поворот скорость Bs
    /// </summary>
    [ObservableProperty]
    private float _rotationSpeed = 100;

    [ObservableProperty]
    private bool _isCuted = false;

    public BendingDataViewModel() { }

    public BendingDataViewModel(BendingData data)
    {
        if (data is not null)
        {
            PipeLength = data.PipeLength;
            YSetup = data.YSetup;
            Supply = data.Supply;
            SupplySpeed = data.SupplySpeed;
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
