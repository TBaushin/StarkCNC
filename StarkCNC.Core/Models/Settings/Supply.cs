using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Supply
{
    private string _resetedOffsetRequestString = string.Empty;
    private string _startRollingSpeedRequestString = string.Empty;
    private string _coefficientRequestString = string.Empty;

    public double ResetedOffset { get; set; }
    public double StartRollingSpeed { get; set; }
    public double Coefficient { get; set; }

    public Supply(
        double resetedOffset,
        string resetedOffsetRequestString,
        double startRollingSpeed,
        string startRollingSpeedRequestString,
        double coefficient,
        string coefficientRequestString)
    {
        ResetedOffset = resetedOffset;
        _resetedOffsetRequestString = resetedOffsetRequestString;
        StartRollingSpeed = startRollingSpeed;
        _startRollingSpeedRequestString = startRollingSpeedRequestString;
        Coefficient = coefficient;
        _coefficientRequestString = coefficientRequestString;
    }

    public static Supply ReadConfiguration(IConfigurationSection section)
    {
        var supplySection = section.GetSection("Rotation");

        var resetedOffsetSection = supplySection.GetSection("ResetedOffset");
        var resetedOffsetDefault = resetedOffsetSection.GetSection("Default").Get<double>();
        var resetedOffsetRequestString = resetedOffsetSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var startRollingSpeedSection = supplySection.GetSection("StartRollingSpeed");
        var startRollingSpeedDefault = startRollingSpeedSection.GetSection("Default").Get<double>();
        var startRollingSpeedRequestString = startRollingSpeedSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var coefficientSection = supplySection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Supply(
            resetedOffsetDefault,
            resetedOffsetRequestString,
            startRollingSpeedDefault,
            startRollingSpeedRequestString,
            coefficientDefault,
            coefficientRequestString);
    }
}
