using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Dorn : ICloneable
{
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Dorn(
        double forwardPosition,
        string forwardPositionRequestString,
        double middlePosition,
        string middlePositionRequestString,
        double backPosition,
        string backPositionRequestString,
        double speedPosition,
        string speedPositionRequestString)
    {
        ForwardPosition = forwardPosition;
        _forwardPositionRequestString = forwardPositionRequestString;
        MiddlePosition = middlePosition;
        _middlePositionRequestString = middlePositionRequestString;
        BackPosition = backPosition;
        _backPositionRequestString = backPositionRequestString;
        SpeedCoefficient = speedPosition;
        _speedCoefficientRequestString = speedPositionRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Dorn other)
            return false;

        return other.ForwardPosition == ForwardPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.BackPosition == BackPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            ForwardPosition,
            _forwardPositionRequestString,
            MiddlePosition,
            _middlePositionRequestString,
            BackPosition,
            _backPositionRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static Dorn ReadConfiguration(IConfigurationSection section)
    {
        var dornSection = section.GetSection("Dorn");

        var forwardPositionSection = dornSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<double>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = dornSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backPositionSection = dornSection.GetSection("BackPosition");
        var backPositionDefault = backPositionSection.GetSection("Default").Get<double>();
        var backPositionRequestString = backPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = dornSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;
        return new Dorn(
            forwardPositionDefault,
            forwardPositionRequestString,
            middlePositionDefault,
            middlePositionRequestString,
            backPositionDefault,
            backPositionRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);

    }
}
