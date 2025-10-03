using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Squeeze : ICloneable
{
    public bool TurnOn { get; set; }
    public double FrontPositionLimitation { get; set; }
    public double SpeedCoefficient { get; set; }

    public Squeeze(
        bool turnOn,
        double frontPositionLimitation,
        double speedCoefficient)
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
