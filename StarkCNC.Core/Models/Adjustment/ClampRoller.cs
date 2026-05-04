using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class ClampRoller : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float OuterRadius { get; set; }
    public float InnerRadius { get; set; }

    public ClampRoller(
        float outerRadius,
        float innerRadius)
    {
        OuterRadius = outerRadius;
        InnerRadius = innerRadius;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not ClampRoller other)
            return false;

        return OuterRadius == other.OuterRadius && InnerRadius == other.InnerRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(OuterRadius, InnerRadius);
}