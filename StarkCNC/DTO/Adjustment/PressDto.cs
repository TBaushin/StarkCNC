using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class PressDto : ObservableObject, ICloneable
{
    private string _dangerZoneCoordinateRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backwardPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _dangerZoneCoordinate;

    [ObservableProperty]
    private float _length;

    [ObservableProperty]
    private float _forwardPosition;

    [ObservableProperty]
    private float _middlePosition;

    [ObservableProperty]
    private float _backwardPosition;

    [ObservableProperty]
    private float _speedCoefficient;

    public PressDto(
        string dangerZoneCoordinateRequestString,
        string lengthRequestString,
        string forwardPositionRequestString,
        string middlePositionRequestString,
        string backwardPositionRequestString,
        string speedCoefficientRequestString)
    {
        _dangerZoneCoordinateRequestString = dangerZoneCoordinateRequestString;
        _lengthRequestString = lengthRequestString;
        _forwardPositionRequestString = forwardPositionRequestString;
        _middlePositionRequestString = middlePositionRequestString;
        _backwardPositionRequestString = backwardPositionRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Press Parse(Guid? id) =>
        new Press(DangerZoneCoordinate, Length, ForwardPosition, MiddlePosition, BackwardPosition, SpeedCoefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not PressDto other)
            return false;

        return
            Id == other.Id &&
            DangerZoneCoordinate == other.DangerZoneCoordinate &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
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

    public static PressDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var pressSection = section.GetSection("Press");

        var dangerZoneCoordinateSection = pressSection.GetSection("DangerZoneCoordinate");
        var dangerZoneCoordinateDefault = dangerZoneCoordinateSection.GetSection("Default").Get<float>();
        var dangerZoneCoordinateRequestString = dangerZoneCoordinateSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = pressSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<float>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardPositionSection = pressSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<float>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = pressSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backwardPositionSection = pressSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<float>();
        var backwardPositionRequestString = backwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = pressSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new PressDto(
            dangerZoneCoordinateRequestString,
            lengthRequestString,
            forwardPositionRequestString,
            middlePositionRequestString,
            backwardPositionRequestString,
            speedCoefficientRequestString)
        {
            DangerZoneCoordinate = dangerZoneCoordinateDefault,
            Length = lengthDefault,
            ForwardPosition = forwardPositionDefault,
            MiddlePosition = middlePositionDefault,
            BackwardPosition = backwardPositionDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}