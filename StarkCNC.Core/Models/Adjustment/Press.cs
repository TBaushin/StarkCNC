using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Press : ICloneable
{
    private string _dangerZoneCoordinateRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;

    public double DangerZoneCoordinate { get; set; }
    public double Length { get; set; }

    public Press(
        double dangerZoneCoordinate,
        string dangerZoneCoordinateRequestString,
        double length,
        string lengthRequestString)
    {
        DangerZoneCoordinate = dangerZoneCoordinate;
        _dangerZoneCoordinateRequestString = dangerZoneCoordinateRequestString;
        Length = length;
        _lengthRequestString = lengthRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Press other)
            return false;

        return DangerZoneCoordinate == other.DangerZoneCoordinate && Length == other.Length;
    }

    public override int GetHashCode() =>
        HashCode.Combine(DangerZoneCoordinate, _dangerZoneCoordinateRequestString, Length, _lengthRequestString);

    public static Press ReadConfiguration(IConfigurationSection section)
    {
        var pressSection = section.GetSection("Press");

        var dangerZoneCoordinateSection = pressSection.GetSection("DangerZoneCoordinate");
        var dangerZoneCoordinateDefault = dangerZoneCoordinateSection.GetSection("Default").Get<double>();
        var dangerZoneCoordinateRequestString = dangerZoneCoordinateSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = pressSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<double>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Press(dangerZoneCoordinateDefault, dangerZoneCoordinateRequestString, lengthDefault, lengthRequestString);
    }
}
