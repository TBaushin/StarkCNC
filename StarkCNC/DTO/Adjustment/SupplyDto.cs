using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class SupplyDto : ObservableObject, ICloneable
{
    private string _pressZonePositionRequestString = string.Empty;
    private string _forwardDangerZonePositionRequestString = string.Empty;
    private string _colletJawsDepthRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _pressZonePosition;

    [ObservableProperty]
    private float _forwardDangerZonePosition;

    [ObservableProperty]
    private float _colletJawsDepth;

    public SupplyDto(
        string pressZonePositionRequestString,
        string forwardDangerZonePositionRequestString,
        string colletJawsDepthRequestString)
    {
        _pressZonePositionRequestString = pressZonePositionRequestString;
        _forwardDangerZonePositionRequestString = forwardDangerZonePositionRequestString;
        _colletJawsDepthRequestString = colletJawsDepthRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Supply Parse(Guid? id) =>
        new Supply(PressZonePosition, ForwardDangerZonePosition, ColletJawsDepth) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not SupplyDto other)
            return false;

        return
            other.Id == Id &&
            other.PressZonePosition == PressZonePosition &&
            other.ForwardDangerZonePosition == ForwardDangerZonePosition &&
            other.ColletJawsDepth == ColletJawsDepth;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            PressZonePosition,
            _pressZonePositionRequestString,
            ForwardDangerZonePosition,
            _forwardDangerZonePositionRequestString,
            ColletJawsDepth,
            _colletJawsDepthRequestString);

    public static SupplyDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var supplySection = section.GetSection("Supply");

        var pressZonePositionSection = supplySection.GetSection("PressZonePosition");
        var pressZonePositionDefault = pressZonePositionSection.GetSection("Default").Get<float>();
        var pressZonePositionRequestString = pressZonePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardDangerZonePositionSection = supplySection.GetSection("ForwardDangerZonePosition");
        var forwardDangerZonePositionDefault = forwardDangerZonePositionSection.GetSection("Default").Get<float>();
        var forwardDangerZonePositionRequestString = forwardDangerZonePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var colletJawsDepthSection = supplySection.GetSection("ColletJawsDepth");
        var colletJawsDepthDefault = colletJawsDepthSection.GetSection("Default").Get<float>();
        var colletJawsDepthRequestString = colletJawsDepthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = supplySection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new SupplyDto(
            pressZonePositionRequestString,
            forwardDangerZonePositionRequestString,
            colletJawsDepthRequestString)
        {
            PressZonePosition = pressZonePositionDefault,
            ForwardDangerZonePosition = forwardDangerZonePositionDefault,
            ColletJawsDepth = colletJawsDepthDefault,
        };
    }
}