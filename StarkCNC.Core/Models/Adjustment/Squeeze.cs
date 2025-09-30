using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Squeeze : ICloneable
{
    private string _turnOnRequestString = string.Empty;
    private string _frontPositionLimitationRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public bool TurnOn { get; set; }
    public double FrontPositionLimitation { get; set; }
    public double SpeedCoefficient { get; set; }

    public Squeeze(
        bool turnOn,
        string turnOnRequestString,
        double frontPositionLimitation,
        string frontPositionLimitationRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        TurnOn = turnOn;
        _turnOnRequestString = turnOnRequestString;
        FrontPositionLimitation = frontPositionLimitation;
        _frontPositionLimitationRequestString = frontPositionLimitationRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Squeeze other)
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

    public static Squeeze ReadConfiguration(IConfigurationSection section)
    {
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

        return new Squeeze(
            turnOnDefault,
            turnOnRequestString,
            frontPositionLimitationDefault,
            frontPositionLimitationRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
