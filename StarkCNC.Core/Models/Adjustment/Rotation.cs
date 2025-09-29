using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Rotation : ICloneable
{
    private string _offsetAfterZeroSearchRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double OffsetAfterZeroSearch { get; set; }
    public double SpeedCoefficient { get; set; }

    public Rotation(
        double offsetAfterZeroSearch,
        string offsetAfterZeroSearchRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        OffsetAfterZeroSearch = offsetAfterZeroSearch;
        _offsetAfterZeroSearchRequestString = offsetAfterZeroSearchRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Rotation other)
            return false;

        return other.SpeedCoefficient == SpeedCoefficient &&
            other.OffsetAfterZeroSearch == OffsetAfterZeroSearch;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            OffsetAfterZeroSearch,
            _offsetAfterZeroSearchRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static Rotation ReadConfiguration(IConfigurationSection section)
    {
        var rotationSection = section.GetSection("Rotation");

        var offsetAfterZeroSearchSection = rotationSection.GetSection("OffsetAfterZeroSearch");
        var offsetAfterZeroSearchDefault = offsetAfterZeroSearchSection.GetSection("Default").Get<double>();
        var offsetAfterZeroSearchRequestString = offsetAfterZeroSearchSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = rotationSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Rotation(
            offsetAfterZeroSearchDefault,
            offsetAfterZeroSearchRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
