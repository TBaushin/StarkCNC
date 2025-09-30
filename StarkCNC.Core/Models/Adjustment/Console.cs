using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Console : ICloneable
{
    private string _bendPositionRequestString = string.Empty;
    private string _secondFloorPositionRequestString = string.Empty;
    private string _secondFloorIntermediatePositionRequestString = string.Empty;
    private string _thirdFloorPositionRequestString = string.Empty;
    private string _pipeRotationDepartureDistance = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double BendPosition { get; set; }
    public double SecondFloorPosition { get; set; }
    public double SecondFloorIntermediatePosition { get; set; }
    public double ThirdFloorPosition { get; set; }
    public double PipeRotationDepartureDistance { get; set; }
    public double SpeedCoefficient { get; set; }

    public Console(
        double bendPosition,
        string bendPositionRequestString,
        double secondFloorPosition,
        string secondFloorPositionRequestString,
        double secondFloorIntermediatePosition,
        string secondFloorIntermediatePositionRequestString,
        double thirdFloorPosition,
        string thirdFloorPositionRequestString,
        double pipeRotationDepartureDistance,
        string pipeRotationDepartureDistanceRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        BendPosition = bendPosition;
        _bendPositionRequestString = bendPositionRequestString;
        SecondFloorPosition = secondFloorPosition;
        _secondFloorPositionRequestString = secondFloorPositionRequestString;
        SecondFloorIntermediatePosition = secondFloorIntermediatePosition;
        _secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionRequestString;
        ThirdFloorPosition = thirdFloorPosition;
        _thirdFloorPositionRequestString = thirdFloorPositionRequestString;
        PipeRotationDepartureDistance = pipeRotationDepartureDistance;
        _pipeRotationDepartureDistance = pipeRotationDepartureDistanceRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Console other)
            return false;

        return
            BendPosition == other.BendPosition &&
            SecondFloorPosition == other.SecondFloorPosition &&
            SecondFloorIntermediatePosition == other.SecondFloorIntermediatePosition &&
            ThirdFloorPosition == other.ThirdFloorPosition &&
            PipeRotationDepartureDistance == other.PipeRotationDepartureDistance &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            BendPosition,
            _bendPositionRequestString,
            SecondFloorPosition,
            _secondFloorPositionRequestString,
            SecondFloorIntermediatePosition,
            _secondFloorIntermediatePositionRequestString,
            HashCode.Combine(
                ThirdFloorPosition,
                _thirdFloorPositionRequestString,
                PipeRotationDepartureDistance,
                _pipeRotationDepartureDistance,
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static Console ReadConfiguration(IConfigurationSection section)
    {
        var consoleSection = section.GetSection("Console");

        var bendPositionSection = consoleSection.GetSection("BendPosition");
        var bendPositionDefault = bendPositionSection.GetSection("Default").Get<double>();
        var bendPositionRequestString = bendPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorPositionSection = consoleSection.GetSection("SecondFloorPosition");
        var secondFloorPositionDefault = secondFloorPositionSection.GetSection("Default").Get<double>();
        var secondFloorPositionRequestString = secondFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorIntermediatePositionSection = consoleSection.GetSection("SecondFloorIntermediatePosition");
        var secondFloorIntermediatePositionDefault = secondFloorIntermediatePositionSection.GetSection("Default").Get<double>();
        var secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var thirdFloorPositionSection = consoleSection.GetSection("ThirdFloorPosition");
        var thirdFloorPositionDefault = thirdFloorPositionSection.GetSection("Default").Get<double>();
        var thirdFloorPositionRequestString = thirdFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeRotationDepartureDistanceSection = consoleSection.GetSection("PipeRotationDepartureDistance");
        var pipeRotationDepartureDistanceDefault = pipeRotationDepartureDistanceSection.GetSection("Default").Get<double>();
        var pipeRotationDepartureDistanceRequestString = pipeRotationDepartureDistanceSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = consoleSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Console(
            bendPositionDefault,
            bendPositionRequestString,
            secondFloorPositionDefault,
            secondFloorPositionRequestString,
            secondFloorIntermediatePositionDefault,
            secondFloorIntermediatePositionRequestString,
            thirdFloorPositionDefault,
            thirdFloorPositionRequestString,
            pipeRotationDepartureDistanceDefault,
            pipeRotationDepartureDistanceRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
