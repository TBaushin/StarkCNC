using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class BendRoller : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public double Radius { get; set; }
    public double OuterRadius { get; set; }

    public BendRoller(double radius, double outerRadius)
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