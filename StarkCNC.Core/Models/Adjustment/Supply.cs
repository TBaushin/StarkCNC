namespace StarkCNC.Core.Models.Adjustment;

public class Supply : ICloneable
{
    public Guid Id { get; set; }
    public double PressZonePosition { get; set; }
    public double ForwardDangerZonePosition { get; set; }
    public double ColletJawsDepth { get; set; }
    public double SpeedCoefficient { get; set; }

    public Supply(
        double pressZonePosition,
        double forwardDangerZonePosition,
        double colletJawsDepth,
        double speedCoefficient)
    {
        PressZonePosition = pressZonePosition;
        ForwardDangerZonePosition = forwardDangerZonePosition;
        ColletJawsDepth = colletJawsDepth;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Supply other)
            return false;

        return
            other.PressZonePosition == PressZonePosition &&
            other.ForwardDangerZonePosition == ForwardDangerZonePosition &&
            other.ColletJawsDepth == ColletJawsDepth &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            PressZonePosition,
            ForwardDangerZonePosition,
            ColletJawsDepth,
            SpeedCoefficient);
}
