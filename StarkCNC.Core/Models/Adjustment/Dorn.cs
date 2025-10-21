namespace StarkCNC.Core.Models.Adjustment;

public class Dorn : ICloneable
{
    public Guid Id { get; set; }
    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackwardPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Dorn(
        double forwardPosition,
        double middlePosition,
        double backwardPosition,
        double speedCoefficient)
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