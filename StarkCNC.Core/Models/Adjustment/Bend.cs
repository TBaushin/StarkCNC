namespace StarkCNC.Core.Models.Adjustment;

public class Bend : ICloneable
{
    public Guid Id { get; set; }
    public double ForwardPositionLimitation { get; set; }
    public double SpeedCoefficient { get; set; }
    public double SlowdownSpeed { get; set; }

    public Bend(
        double forwardPositionLimitation,
        double speedCoefficient,
        double slowdownSpeed)
    {
        ForwardPositionLimitation = forwardPositionLimitation;
        SpeedCoefficient = speedCoefficient;
        SlowdownSpeed = slowdownSpeed;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Bend other)
            return false;

        return other.ForwardPositionLimitation == ForwardPositionLimitation &&
            other.SpeedCoefficient == SpeedCoefficient &&
            other.SlowdownSpeed == SlowdownSpeed;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            ForwardPositionLimitation,
            SpeedCoefficient,
            SlowdownSpeed);
}
