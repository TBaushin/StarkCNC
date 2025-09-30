using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Clamp : ICloneable
{
    private string _deepRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backwardPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double Deep { get; set; }
    public double Length { get; set; }
    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackwardPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Clamp(
        double deep,
        string deepRequestString,
        double length,
        string lengthRequestString,
        double forwardPosition,
        string forwardPositionRequestString,
        double middlePosition,
        string middlePositionRequestString,
        double backwardPosition,
        string backwardPositionRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        Deep = deep;
        _deepRequestString = deepRequestString;
        Length = length;
        _lengthRequestString = lengthRequestString;
        ForwardPosition = forwardPosition;
        _forwardPositionRequestString = forwardPositionRequestString;
        MiddlePosition = middlePosition;
        _middlePositionRequestString = middlePositionRequestString;
        BackwardPosition = backwardPosition;
        _backwardPositionRequestString = backwardPositionRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Clamp other)
            return false;

        return
            Deep == other.Deep &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Deep,
            _deepRequestString,
            Length,
            _lengthRequestString,
            HashCode.Combine(
                ForwardPosition,
                _forwardPositionRequestString,
                MiddlePosition,
                _middlePositionRequestString,
                BackwardPosition,
                _backwardPositionRequestString,
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static Clamp ReadConfiguration(IConfigurationSection section)
    {
        var clampSection = section.GetSection("Clamp");

        var deepSection = clampSection.GetSection("Deep");
        var deepDefault = deepSection.GetSection("Default").Get<double>();
        var deepRequestString = deepSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = clampSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<double>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardPositionSection = clampSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<double>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = clampSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backwardPositionSection = clampSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<double>();
        var backwardPositionRequestString = backwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = clampSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Clamp(
            deepDefault,
            deepRequestString,
            lengthDefault,
            lengthRequestString,
            forwardPositionDefault,
            forwardPositionRequestString,
            middlePositionDefault,
            middlePositionRequestString,
            backwardPositionDefault,
            backwardPositionRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
