using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class SqueezeDto : ObservableObject, ICloneable
{
    private string _turnOnRequestString = string.Empty;
    private string _frontPositionLimitationRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private bool _turnOn;

    [ObservableProperty]
    private double _frontPositionLimitation;

    [ObservableProperty]
    private double _speedCoefficient;

    public SqueezeDto(
        string turnOnRequestString,
        string frontPositionLimitationRequestString,
        string speedCoefficientRequestString)
    {
        _turnOnRequestString = turnOnRequestString;
        _frontPositionLimitationRequestString = frontPositionLimitationRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Squeeze Parse() =>
        new Squeeze(TurnOn, FrontPositionLimitation, SpeedCoefficient);

    public override bool Equals(object? obj)
    {
        if (obj is not SqueezeDto other)
            return false;

        return
            TurnOn == other.TurnOn &&
            FrontPositionLimitation == other.FrontPositionLimitation &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            TurnOn,
            _turnOnRequestString,
            FrontPositionLimitation,
            _frontPositionLimitationRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static SqueezeDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var squeezeSection = section.GetSection("Squeeze");

        var turnOnSection = squeezeSection.GetSection("TurnOn");
        var turnOnDefault = turnOnSection.GetSection("Default").Get<bool>();
        var turnOnRequestString = turnOnSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var frontPositionLimitationSection = squeezeSection.GetSection("FrontPositionLimitation");
        var frontPositionLimitationDefault = frontPositionLimitationSection.GetSection("Default").Get<double>();
        var frontPositionLimitationRequestString = frontPositionLimitationSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = squeezeSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new SqueezeDto(
            turnOnRequestString,
            frontPositionLimitationRequestString,
            speedCoefficientRequestString)
        {
            TurnOn = turnOnDefault,
            FrontPositionLimitation = frontPositionLimitationDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}
