using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Clamp : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public double Deep { get; set; }
    public double Length { get; set; }
    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackwardPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Clamp(
        double deep,
        double length,
        double forwardPosition,
        double middlePosition,
        double backwardPosition,
        double speedCoefficient)
    {
        Deep = deep;
        Length = length;
        ForwardPosition = forwardPosition;
        MiddlePosition = middlePosition;
        BackwardPosition = backwardPosition;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Clamp other)
            return false;

        return
            Deep == other.Deep &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Deep,
            Length,
            ForwardPosition,
            MiddlePosition,
            BackwardPosition,
            SpeedCoefficient);
}