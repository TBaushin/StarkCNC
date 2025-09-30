using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Press : ICloneable
{
    private string _dangerZoneCoordinateRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backwardPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double DangerZoneCoordinate { get; set; }
    public double Length { get; set; }
    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackwardPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Press(
        double dangerZoneCoordinate,
        string dangerZoneCoordinateRequestString,
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
        DangerZoneCoordinate = dangerZoneCoordinate;
        _dangerZoneCoordinateRequestString = dangerZoneCoordinateRequestString;
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
        if (obj is not Press other)
            return false;

        return
            DangerZoneCoordinate == other.DangerZoneCoordinate &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            DangerZoneCoordinate,
            _dangerZoneCoordinateRequestString,
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

    public static Press ReadConfiguration(IConfigurationSection section)
    {
        var pressSection = section.GetSection("Press");

        var dangerZoneCoordinateSection = pressSection.GetSection("DangerZoneCoordinate");
        var dangerZoneCoordinateDefault = dangerZoneCoordinateSection.GetSection("Default").Get<double>();
        var dangerZoneCoordinateRequestString = dangerZoneCoordinateSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = pressSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<double>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardPositionSection = pressSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<double>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = pressSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backwardPositionSection = pressSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<double>();
        var backwardPositionRequestString = backwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = pressSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Press(
            dangerZoneCoordinateDefault,
            dangerZoneCoordinateRequestString,
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
