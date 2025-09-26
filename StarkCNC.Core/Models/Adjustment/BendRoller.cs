using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class BendRoller
{
    private string _radiusRequestString = string.Empty;
    private string _outerRadiusRequestString = string.Empty;

    public double Radius { get; set; }
    public double OuterRadius { get; set; }

    public BendRoller(double radius, string radiusRequestString, double outerRadius, string outerRadiusRequestString)
    {
        Radius = radius;
        _radiusRequestString = radiusRequestString;
        OuterRadius = outerRadius;
        _outerRadiusRequestString = outerRadiusRequestString;
    }

    public BendRoller Copy() =>
        new BendRoller(Radius, _radiusRequestString, OuterRadius, _outerRadiusRequestString);

    public override bool Equals(object? obj)
    {
        var other = obj as BendRoller;
        if (other is null)
            return false;

        return Radius == other.Radius && OuterRadius == other.OuterRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Radius, _radiusRequestString, OuterRadius, _outerRadiusRequestString);

    public static BendRoller ReadConfiguration(IConfigurationSection section)
    {
        var bendRollerSection = section.GetSection("BendRoller");

        var radiusSection = bendRollerSection.GetSection("Radius");
        var radiusDefault = radiusSection.GetSection("Default").Get<double>();
        var radiusRequestString = radiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var outerRadiusSection = bendRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<double>();
        var outerRadiusRequestString = outerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new BendRoller(radiusDefault, radiusRequestString, outerRadiusDefault, outerRadiusRequestString);
    }
}
