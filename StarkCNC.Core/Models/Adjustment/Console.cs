using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Console
{
    private string _bendPositionRequestString = string.Empty;
    private string _secondFloorPositionRequestString = string.Empty;
    private string _secondFloorIntermediatePositionRequestString = string.Empty;

    public double BendPosition { get; set; }
    public double SecondFloorPosition { get; set; }
    public double SecondFloorIntermediatePosition { get; set; }

    public Console(
        double bendPosition,
        string bendPositionRequestString,
        double secondFloorPosition,
        string secondFloorPositionRequestString,
        double secondFloorIntermediatePosition,
        string secondFloorIntermediatePositionRequestString)
    {
        BendPosition = bendPosition;
        _bendPositionRequestString = bendPositionRequestString;
        SecondFloorPosition = secondFloorPosition;
        _secondFloorPositionRequestString = secondFloorPositionRequestString;
        SecondFloorIntermediatePosition = secondFloorIntermediatePosition;
        _secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionRequestString;
    }

    public Console Copy() =>
        new Console(
            BendPosition,
            _bendPositionRequestString,
            SecondFloorPosition,
            _secondFloorPositionRequestString,
            SecondFloorIntermediatePosition,
            _secondFloorIntermediatePositionRequestString);

    public override bool Equals(object? obj)
    {
        var other = obj as Console;
        if (other is null)
            return false;

        return
            BendPosition == other.BendPosition &&
            SecondFloorPosition == other.SecondFloorPosition &&
            SecondFloorIntermediatePosition == other.SecondFloorIntermediatePosition;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            BendPosition,
            _bendPositionRequestString,
            SecondFloorPosition,
            _secondFloorPositionRequestString,
            SecondFloorIntermediatePosition,
            _secondFloorIntermediatePositionRequestString);

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

        return new Console(
            bendPositionDefault,
            bendPositionRequestString,
            secondFloorPositionDefault,
            secondFloorPositionRequestString,
            secondFloorIntermediatePositionDefault,
            secondFloorIntermediatePositionRequestString);
    }
}
