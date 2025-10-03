using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class RotationDto : ObservableObject, ICloneable
{
    private string _offsetAfterZeroSearchRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private double _offsetAfterZeroSearch;

    [ObservableProperty]
    private double _speedCoefficient;

    public RotationDto(
        string offsetAfterZeroSearchRequestString,
        string speedCoefficientRequestString)
    {
        _offsetAfterZeroSearchRequestString = offsetAfterZeroSearchRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Rotation Parse() =>
        new Rotation(OffsetAfterZeroSearch, SpeedCoefficient);

    public override bool Equals(object? obj)
    {
        if (obj is not RotationDto other)
            return false;

        return
            other.SpeedCoefficient == SpeedCoefficient &&
            other.OffsetAfterZeroSearch == OffsetAfterZeroSearch;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            OffsetAfterZeroSearch,
            _offsetAfterZeroSearchRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static RotationDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var rotationSection = section.GetSection("Rotation");

        var offsetAfterZeroSearchSection = rotationSection.GetSection("OffsetAfterZeroSearch");
        var offsetAfterZeroSearchDefault = offsetAfterZeroSearchSection.GetSection("Default").Get<double>();
        var offsetAfterZeroSearchRequestString = offsetAfterZeroSearchSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = rotationSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new RotationDto(
            offsetAfterZeroSearchRequestString,
            speedCoefficientRequestString)
        {
            OffsetAfterZeroSearch = offsetAfterZeroSearchDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}
