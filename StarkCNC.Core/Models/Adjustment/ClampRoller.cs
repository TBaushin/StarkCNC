using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class ClampRoller
{
    private string _outerRadiusRequesString = string.Empty;
    private string _innerRadiusRequestString = string.Empty;

    public double OuterRadius { get; set; }
    public double InnerRadius { get; set; }

    public ClampRoller(
        double outerRadius,
        string outerRadiusRequestString,
        double innerRadius,
        string innerRadiusRequestString)
    {
        OuterRadius = outerRadius;
        _outerRadiusRequesString = outerRadiusRequestString;
        InnerRadius = innerRadius;
        _innerRadiusRequestString = innerRadiusRequestString;
    }

    public ClampRoller Copy() =>
        new ClampRoller(OuterRadius, _outerRadiusRequesString, InnerRadius, _innerRadiusRequestString);

    public override bool Equals(object? obj)
    {
        var other = obj as ClampRoller;
        if (other is null)
            return false;

        return OuterRadius == other.OuterRadius && InnerRadius == other.InnerRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(OuterRadius, _outerRadiusRequesString, InnerRadius, _innerRadiusRequestString);

    public static ClampRoller ReadConfiguration(IConfigurationSection section)
    {
        var clampRollerSection = section.GetSection("ClampRoller");

        var outerRadiusSection = clampRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<double>();
        var outerRadiusRequestString = outerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var innerRadiusSection = clampRollerSection.GetSection("InnerRadius");
        var innerRadiusDefault = innerRadiusSection.GetSection("Default").Get<double>();
        var innerRadiusRequestString = innerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ClampRoller(
            outerRadiusDefault,
            outerRadiusRequestString,
            innerRadiusDefault,
            innerRadiusRequestString);
    }
}
