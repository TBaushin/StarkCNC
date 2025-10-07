using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class SupplyDto : ObservableObject, ICloneable
{
    private string _pressZonePositionRequestString = string.Empty;
    private string _forwardDangerZonePositionRequestString = string.Empty;
    private string _colletJawsDepthRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _pressZonePosition;

    [ObservableProperty]
    private double _forwardDangerZonePosition;

    [ObservableProperty]
    private double _colletJawsDepth;

    [ObservableProperty]
    private double _speedCoefficient;

    public SupplyDto(
        string pressZonePositionRequestString,
        string forwardDangerZonePositionRequestString,
        string colletJawsDepthRequestString,
        string speedCoefficientRequestString)
    {
        _pressZonePositionRequestString = pressZonePositionRequestString;
        _forwardDangerZonePositionRequestString = forwardDangerZonePositionRequestString;
        _colletJawsDepthRequestString = colletJawsDepthRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Supply Parse(Guid? id) =>
        new Supply(PressZonePosition, ForwardDangerZonePosition, ColletJawsDepth, SpeedCoefficient) { Id = id ?? Guid.NewGuid() };

    public override bool Equals(object? obj)
    {
        if (obj is not SupplyDto other)
            return false;

        return
            other.Id == Id &&
            other.PressZonePosition == PressZonePosition &&
            other.ForwardDangerZonePosition == ForwardDangerZonePosition &&
            other.ColletJawsDepth == ColletJawsDepth &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            PressZonePosition,
            _pressZonePositionRequestString,
            ForwardDangerZonePosition,
            _forwardDangerZonePositionRequestString,
            ColletJawsDepth,
            _colletJawsDepthRequestString,
            HashCode.Combine(
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static SupplyDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

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

        return new SupplyDto(
            pressZonePositionRequestString,
            forwardDangerZonePositionRequestString,
            colletJawsDepthRequestString,
            speedCoefficientRequestString)
        {
            PressZonePosition = pressZonePositionDefault,
            ForwardDangerZonePosition = forwardDangerZonePositionDefault,
            ColletJawsDepth = colletJawsDepthDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}
