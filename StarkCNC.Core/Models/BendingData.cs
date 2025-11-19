namespace StarkCNC.Core.Models;

public class BendingData
{
    /// <summary>
    /// Длина прямого участка Y
    /// </summary>
    public double StraightLength { get; set; }

    /// <summary>
    /// Длина прямого участка скорость Ys
    /// </summary>
    public double StraightSpeed { get; set; }

    /// <summary>
    /// Отвод Y1
    /// </summary>
    public double Offset { get; set; }

    /// <summary>
    /// Отвод скорость Y1b
    /// </summary>
    public double OffsetSpeed { get; set; }

    /// <summary>
    /// Отвод коэффициент Y2
    /// </summary>
    public double OffsetCoefficient;

    /// <summary>
    /// Угол гиба C
    /// </summary>
    public double BendingAngle { get; set; }

    /// <summary>
    /// Гиб скорость Cs
    /// </summary>
    public double BendingAngleSpeed { get; set; }

    /// <summary>
    /// Гиб коэффициент Ck
    /// </summary>
    public double BendingAngleCoefficient { get; set; }

    /// <summary>
    /// Радиус гиба R
    /// </summary>
    public double BendingRadius { get; set; }

    /// <summary>
    /// Радиус гиба режим M
    /// </summary>
    public string BendingRadiusMode { get; set; } = string.Empty;

    /// <summary>
    /// Угол поворота B
    /// </summary>
    public double RotationAngle { get; set; }

    /// <summary>
    /// Поворот скорость Bs
    /// </summary>
    public double RotationSpeed { get; set; }

    public BendingData Copy() =>
        new BendingData
        {
            StraightLength = StraightLength,
            StraightSpeed = StraightSpeed,
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
