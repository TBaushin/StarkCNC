using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Lift : ICloneable
{
    private string _upperPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _lowerPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double UpperPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double LowerPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Lift(
        double upperPosition,
        string upperPositionRequestString,
        double middlePosition,
        string middlePositionRequestString,
        double lowerPosition,
        string lowerPositionRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        UpperPosition = upperPosition;
        _upperPositionRequestString = upperPositionRequestString;
        MiddlePosition = middlePosition;
        _middlePositionRequestString = middlePositionRequestString;
        LowerPosition = lowerPosition;
        _lowerPositionRequestString = lowerPositionRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Lift other)
            return false;

        return other.UpperPosition == UpperPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.LowerPosition == LowerPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            UpperPosition,
            _upperPositionRequestString,
            MiddlePosition,
            _middlePositionRequestString,
            LowerPosition,
            _lowerPositionRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static Lift ReadConfiguration(IConfigurationSection section)
    {
        var liftSection = section.GetSection("Lift");

        var upperPositionSection = liftSection.GetSection("UpperPosition");
        var upperPositionDefault = upperPositionSection.GetSection("Default").Get<double>();
        var upperPositionRequestString = upperPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = liftSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lowerPositionSection = liftSection.GetSection("LowerPosition");
        var lowerPositionDefault = lowerPositionSection.GetSection("Default").Get<double>();
        var lowerPositionRequestString = lowerPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = liftSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Lift(
            upperPositionDefault,
            upperPositionRequestString,
            middlePositionDefault,
            middlePositionRequestString,
            lowerPositionDefault,
            lowerPositionRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
