using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class ClampDto : ObservableObject, ICloneable
{
    private string _deepRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;
    private string _forwardPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _backwardPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _deep;

    [ObservableProperty]
    private double _length;

    [ObservableProperty]
    private double _forwardPosition;

    [ObservableProperty]
    private double _middlePosition;

    [ObservableProperty]
    private double _backwardPosition;

    [ObservableProperty]
    private double _speedCoefficient;

    public ClampDto(
        string deepRequestString,
        string lengthRequestString,
        string forwardPositionRequestString,
        string middlePositionRequestString,
        string backwardPositionRequestString,
        string speedCoefficientRequestString)
    {
        _deepRequestString = deepRequestString;
        _lengthRequestString = lengthRequestString;
        _forwardPositionRequestString = forwardPositionRequestString;
        _middlePositionRequestString = middlePositionRequestString;
        _backwardPositionRequestString = backwardPositionRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Clamp Parse(Guid? id) =>
        new Clamp(Deep, Length, ForwardPosition, MiddlePosition, BackwardPosition, SpeedCoefficient) { Id = id ?? Guid.NewGuid() };

    public override bool Equals(object? obj)
    {
        if (obj is not ClampDto other)
            return false;

        return
            Id == other.Id &&
            Deep == other.Deep &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Deep,
            _deepRequestString,
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

    public static ClampDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var clampSection = section.GetSection("Clamp");

        var deepSection = clampSection.GetSection("Deep");
        var deepDefault = deepSection.GetSection("Default").Get<double>();
        var deepRequestString = deepSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = clampSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<double>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var forwardPositionSection = clampSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<double>();
        var forwardPositionRequestString = forwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = clampSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backwardPositionSection = clampSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<double>();
        var backwardPositionRequestString = backwardPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = clampSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ClampDto(
            deepRequestString,
            lengthRequestString,
            forwardPositionRequestString,
            middlePositionRequestString,
            backwardPositionRequestString,
            speedCoefficientRequestString)
        {
            Deep = deepDefault,
            Length = lengthDefault,
            ForwardPosition = forwardPositionDefault,
            MiddlePosition = middlePositionDefault,
            BackwardPosition = backwardPositionDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}
