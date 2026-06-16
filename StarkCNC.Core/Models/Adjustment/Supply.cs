using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Supply : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float PressZonePosition { get; set; }
    public float ForwardDangerZonePosition { get; set; }
    public float ColletJawsDepth { get; set; }

    public Supply(
        float pressZonePosition,
        float forwardDangerZonePosition,
        float colletJawsDepth)
    {
        PressZonePosition = pressZonePosition;
        ForwardDangerZonePosition = forwardDangerZonePosition;
        ColletJawsDepth = colletJawsDepth;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Supply other)
            return false;

        return
            other.PressZonePosition == PressZonePosition &&
            other.ForwardDangerZonePosition == ForwardDangerZonePosition &&
            other.ColletJawsDepth == ColletJawsDepth;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            PressZonePosition,
            ForwardDangerZonePosition,
            ColletJawsDepth);
}