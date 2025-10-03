using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class ClampRollerDto : ObservableObject, ICloneable
{
    private string _outerRadiusRequesString = string.Empty;
    private string _innerRadiusRequestString = string.Empty;

    [ObservableProperty]
    private double _outerRadius;

    [ObservableProperty]
    private double _innerRadius;

    public ClampRollerDto(
        string outerRadiusRequestString,
        string innerRadiusRequestString)
    {
        _outerRadiusRequesString = outerRadiusRequestString;
        _innerRadiusRequestString = innerRadiusRequestString;
    }

    public object Clone() => MemberwiseClone();

    public ClampRoller Parse() =>
        new ClampRoller(OuterRadius, InnerRadius);

    public override bool Equals(object? obj)
    {
        if (obj is not ClampRollerDto other)
            return false;

        return OuterRadius == other.OuterRadius && InnerRadius == other.InnerRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(OuterRadius, _outerRadiusRequesString, InnerRadius, _innerRadiusRequestString);

    public static ClampRollerDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var clampRollerSection = section.GetSection("ClampRoller");

        var outerRadiusSection = clampRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<double>();
        var outerRadiusRequestString = outerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var innerRadiusSection = clampRollerSection.GetSection("InnerRadius");
        var innerRadiusDefault = innerRadiusSection.GetSection("Default").Get<double>();
        var innerRadiusRequestString = innerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ClampRollerDto(
            outerRadiusRequestString,
            innerRadiusRequestString)
        {
            OuterRadius = outerRadiusDefault,
            InnerRadius = innerRadiusDefault
        };
    }
}
