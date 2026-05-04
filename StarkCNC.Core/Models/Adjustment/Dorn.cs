using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Dorn : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float ForwardPosition { get; set; }
    public float MiddlePosition { get; set; }
    public float BackwardPosition { get; set; }
    public float SpeedCoefficient { get; set; }

    public Dorn(
        float forwardPosition,
        float middlePosition,
        float backwardPosition,
        float speedCoefficient)
    {
        ForwardPosition = forwardPosition;
        MiddlePosition = middlePosition;
        BackwardPosition = backwardPosition;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Dorn other)
            return false;

        return other.ForwardPosition == ForwardPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.BackwardPosition == BackwardPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            ForwardPosition,
            MiddlePosition,
            BackwardPosition,
            SpeedCoefficient);
}