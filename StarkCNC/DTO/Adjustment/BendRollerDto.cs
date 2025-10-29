using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class BendRollerDto : ObservableObject, ICloneable
{
    private string _radiusRequestString = string.Empty;
    private string _outerRadiusRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _radius;

    [ObservableProperty]
    private double _outerRadius;

    public BendRollerDto(string radiusRequestString, string outerRadiusRequestString)
    {
        _radiusRequestString = radiusRequestString;
        _outerRadiusRequestString = outerRadiusRequestString;
    }

    public object Clone() => MemberwiseClone();

    public BendRoller Parse(Guid? id) =>
        new BendRoller(Radius, OuterRadius) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not BendRollerDto other)
            return false;

        return
            Id == other.Id &&
            Radius == other.Radius &&
            OuterRadius == other.OuterRadius;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Radius,
            _radiusRequestString,
            OuterRadius,
            _outerRadiusRequestString);

    public static BendRollerDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var bendRollerSection = section.GetSection("BendRoller");

        var radiusSection = bendRollerSection.GetSection("Radius");
        var radiusDefault = radiusSection.GetSection("Default").Get<double>();
        var radiusRequestString = radiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var outerRadiusSection = bendRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<double>();
        var outerRadiusRequestString = outerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new BendRollerDto(radiusRequestString, outerRadiusRequestString)
        {
            Radius = radiusDefault,
            OuterRadius = outerRadiusDefault
        };
    }
}
