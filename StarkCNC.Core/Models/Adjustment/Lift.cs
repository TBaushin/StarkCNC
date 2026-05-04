using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Lift : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float UpperPosition { get; set; }
    public float MiddlePosition { get; set; }
    public float LowerPosition { get; set; }
    public float SpeedCoefficient { get; set; }

    public Lift(
        float upperPosition,
        float middlePosition,
        float lowerPosition,
        float speedCoefficient)
    {
        UpperPosition = upperPosition;
        MiddlePosition = middlePosition;
        LowerPosition = lowerPosition;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Lift other)
            return false;

        return
            other.UpperPosition == UpperPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.LowerPosition == LowerPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            UpperPosition,
            MiddlePosition,
            LowerPosition,
            SpeedCoefficient);
}