using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Squeeze : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public bool TurnOn { get; set; }
    public float FrontPositionLimitation { get; set; }
    public float SpeedCoefficient { get; set; }

    public Squeeze(
        bool turnOn,
        float frontPositionLimitation,
        float speedCoefficient)
    {
        TurnOn = turnOn;
        FrontPositionLimitation = frontPositionLimitation;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Squeeze other)
            return false;

        return
            TurnOn == other.TurnOn &&
            FrontPositionLimitation == other.FrontPositionLimitation &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            TurnOn,
            FrontPositionLimitation,
            SpeedCoefficient);
}