namespace StarkCNC.Core.Models.Adjustment;

public class Lift : ICloneable
{
    public Guid Id { get; set; }
    public double UpperPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double LowerPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Lift(
        double upperPosition,
        double middlePosition,
        double lowerPosition,
        double speedCoefficient)
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
