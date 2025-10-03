using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class ClampRoller : ICloneable
{
    public double OuterRadius { get; set; }
    public double InnerRadius { get; set; }

    public ClampRoller(
        double outerRadius,
        double innerRadius)
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
