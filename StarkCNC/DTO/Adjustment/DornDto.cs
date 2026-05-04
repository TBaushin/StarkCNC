using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class DornDto : ObservableObject, ICloneable
{
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _forwardPosition;

    [ObservableProperty]
    private float _middlePosition;

    [ObservableProperty]
    private float _backwardPosition;

    [ObservableProperty]
    private float _speedCoefficient;

    public DornDto(
        string forwardPositionRequestString,
        string middlePositionRequestString,
        string backPositionRequestString,
        string speedCoefficientRequestString)
    {
        _forwardPositionRequestString = forwardPositionRequestString;
        _middlePositionRequestString = middlePositionRequestString;
        _backPositionRequestString = backPositionRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Dorn Parse(Guid? id) =>
        new Dorn(ForwardPosition, MiddlePosition, BackwardPosition, SpeedCoefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not DornDto other)
            return false;

        return
            other.Id == Id &&
            other.ForwardPosition == ForwardPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.BackwardPosition == BackwardPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            ForwardPosition,
            _forwardPositionRequestString,
            MiddlePosition,
            _middlePositionRequestString,
            BackwardPosition,
            _backPositionRequestString,
            HashCode.Combine(
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static DornDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var dornSection = section.GetSection("Dorn");

        var forwardPositionSection = dornSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<float>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = dornSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backwardPositionSection = dornSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<float>();
        var backwardPositionRequestString = backwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = dornSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new DornDto(
            forwardPositionRequestString,
            middlePositionRequestString,
            backwardPositionRequestString,
            speedCoefficientRequestString)
        {
            ForwardPosition = forwardPositionDefault,
            MiddlePosition = middlePositionDefault,
            BackwardPosition = backwardPositionDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}