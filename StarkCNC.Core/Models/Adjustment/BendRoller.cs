using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class BendRoller : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float Radius { get; set; }
    public float OuterRadius { get; set; }

    public BendRoller(float radius, float outerRadius)
    {
        Radius = radius;
        OuterRadius = outerRadius;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not BendRoller other)
            return false;

        return Radius == other.Radius && OuterRadius == other.OuterRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Radius, OuterRadius);
}