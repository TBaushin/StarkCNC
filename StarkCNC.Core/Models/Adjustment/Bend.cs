using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Bend : ICloneable
{
    private string _forwardPositionLimitationRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;
    private string _slowdownSpeedRequestString = string.Empty;

    public double ForwardPositionLimitation { get; set; }
    public double SpeedCoefficient { get; set; }
    public double SlowdownSpeed { get; set; }

    public Bend(
        double forwardPositionLimitation,
        string forwardPositionLimitationRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString,
        double slowdownSpeed,
        string slowdownSpeedRequestString)
    {
        ForwardPositionLimitation = forwardPositionLimitation;
        _forwardPositionLimitationRequestString = forwardPositionLimitationRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
        SlowdownSpeed = slowdownSpeed;
        _slowdownSpeedRequestString = slowdownSpeedRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Bend other)
            return false;

        return other.ForwardPositionLimitation == ForwardPositionLimitation &&
            other.SpeedCoefficient == SpeedCoefficient &&
            other.SlowdownSpeed == SlowdownSpeed;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            ForwardPositionLimitation,
            _forwardPositionLimitationRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString,
            SlowdownSpeed,
            _slowdownSpeedRequestString);

    public static Bend ReadConfiguration(IConfigurationSection section)
    {
        var bendSection = section.GetSection("Bend");

        var forwardPositionLimitationSection = bendSection.GetSection("ForwardPositionLimitation");
        var forwardPositionLimitationDefault = forwardPositionLimitationSection.GetSection("Default").Get<double>();
        var forwardPositionLimitationRequestString = forwardPositionLimitationSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = bendSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var slowdownSpeedSection = bendSection.GetSection("SlowdownSpeed");
        var slowdownSpeedDefault = slowdownSpeedSection.GetSection("Default").Get<double>();
        var slowndownSpeedRequestString = slowdownSpeedSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Bend(
            forwardPositionLimitationDefault,
            forwardPositionLimitationRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString,
            slowdownSpeedDefault,
            slowndownSpeedRequestString);
    }
}
