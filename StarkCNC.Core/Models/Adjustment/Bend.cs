using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Bend : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float ForwardPositionLimitation { get; set; }
    public float SpeedCoefficient { get; set; }
    public float SlowdownSpeed { get; set; }

    public Bend(
        float forwardPositionLimitation,
        float speedCoefficient,
        float slowdownSpeed)
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