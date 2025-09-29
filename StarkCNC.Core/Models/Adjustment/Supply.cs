using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Supply : ICloneable
{
    private string _pressZonePositionRequestString = string.Empty;
    private string _forwardDangerZonePositionRequestString = string.Empty;
    private string _colletJawsDepthRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    public double PressZonePosition { get; set; }
    public double ForwardDangerZonePosition { get; set; }
    public double ColletJawsDepth { get; set; }
    public double SpeedCoefficient { get; set; }

    public Supply(
        double pressZonePosition,
        string pressZonePositionRequestString,
        double forwardDangerZonePosition,
        string forwardDangerZonePositionRequestString,
        double colletJawsDepth,
        string colletJawsDepthRequestString,
        double speedCoefficient,
        string speedCoefficientRequestString)
    {
        PressZonePosition = pressZonePosition;
        _pressZonePositionRequestString = pressZonePositionRequestString;
        ForwardDangerZonePosition = forwardDangerZonePosition;
        _forwardDangerZonePositionRequestString = forwardDangerZonePositionRequestString;
        ColletJawsDepth = colletJawsDepth;
        _colletJawsDepthRequestString = colletJawsDepthRequestString;
        SpeedCoefficient = speedCoefficient;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Supply other)
            return false;

        return other.PressZonePosition == PressZonePosition &&
            other.ForwardDangerZonePosition == ForwardDangerZonePosition &&
            other.ColletJawsDepth == ColletJawsDepth &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            PressZonePosition,
            _pressZonePositionRequestString,
            ForwardDangerZonePosition,
            _forwardDangerZonePositionRequestString,
            ColletJawsDepth,
            _colletJawsDepthRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString);

    public static Supply ReadConfiguration(IConfigurationSection section)
    {
        var supplySection = section.GetSection("Supply");

        var pressZonePositionSection = supplySection.GetSection("PressZonePosition");
        var pressZonePositionDefault = pressZonePositionSection.GetSection("Default").Get<double>();
        var pressZonePositionRequestString = pressZonePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardDangerZonePositionSection = supplySection.GetSection("ForwardDangerZonePosition");
        var forwardDangerZonePositionDefault = forwardDangerZonePositionSection.GetSection("Default").Get<double>();
        var forwardDangerZonePositionRequestString = forwardDangerZonePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var colletJawsDepthSection = supplySection.GetSection("ColletJawsDepth");
        var colletJawsDepthDefault = colletJawsDepthSection.GetSection("Default").Get<double>();
        var colletJawsDepthRequestString = colletJawsDepthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = supplySection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Supply(
            pressZonePositionDefault,
            pressZonePositionRequestString,
            forwardDangerZonePositionDefault,
            forwardDangerZonePositionRequestString,
            colletJawsDepthDefault,
            colletJawsDepthRequestString,
            speedCoefficientDefault,
            speedCoefficientRequestString);
    }
}
