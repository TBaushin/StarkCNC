namespace StarkCNC.Core.Models;

public class BendingData
{
    /// <summary>
    /// Длина трубы L
    /// </summary>
    public float PipeLength { get; set; }

    /// <summary>
    /// Установка Y Y0
    /// </summary>
    public float YSetup { get; set; }

    /// <summary>
    /// Длина уезда цанги
    /// </summary>
    public float ColletOffsetLength { get; set; }

    /// <summary>
    /// Подача Y
    /// </summary>
    public float Supply { get; set; }

    /// <summary>
    /// Подача скорость Ys
    /// </summary>
    public float SupplySpeed { get; set; }

    /// <summary>
    /// Отвод Y1
    /// </summary>
    public float Offset { get; set; }

    /// <summary>
    /// Отвод скорость Y1b
    /// </summary>
    public float OffsetSpeed { get; set; }

    /// <summary>
    /// Отвод коэффициент Y2
    /// </summary>
    public float OffsetCoefficient { get; set; }

    /// <summary>
    /// Угол гиба C
    /// </summary>
    public float BendingAngle { get; set; }

    /// <summary>
    /// Гиб скорость Cs
    /// </summary>
    public float BendingAngleSpeed { get; set; }

    /// <summary>
    /// Гиб коэффициент Ck
    /// </summary>
    public float BendingAngleCoefficient { get; set; }

    /// <summary>
    /// Радиус гиба R
    /// </summary>
    public float BendingRadius { get; set; }

    /// <summary>
    /// Радиус гиба режим M
    /// </summary>
    public string BendingRadiusMode { get; set; } = string.Empty;

    /// <summary>
    /// Угол поворота B
    /// </summary>
    public float RotationAngle { get; set; }

    /// <summary>
    /// Поворот скорость Bs
    /// </summary>
    public float RotationSpeed { get; set; }

    public BendingData Copy() =>
        new BendingData
        {
            PipeLength = PipeLength,
            YSetup = YSetup,
            Supply = Supply,
            SupplySpeed = SupplySpeed,
            Offset = Offset,
            OffsetSpeed = OffsetSpeed,
            OffsetCoefficient = OffsetCoefficient,
            BendingAngle = BendingAngle,
            BendingAngleSpeed = BendingAngleSpeed,
            BendingAngleCoefficient = BendingAngleCoefficient,
            BendingRadius = BendingRadius,
            BendingRadiusMode = BendingRadiusMode,
            RotationAngle = RotationAngle,
            RotationSpeed = RotationSpeed
        };
}
