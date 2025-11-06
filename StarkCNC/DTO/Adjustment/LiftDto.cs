using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class LiftDto : ObservableObject, ICloneable
{
    private string _upperPositionRequestString = string.Empty;
    private string _middlePositionRequestString = string.Empty;
    private string _lowerPositionRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _upperPosition;

    [ObservableProperty]
    private double _middlePosition;

    [ObservableProperty]
    private double _lowerPosition;

    [ObservableProperty]
    private double _speedCoefficient;

    public LiftDto(
        string upperPositionRequestString,
        string middlePositionRequestString,
        string lowerPositionRequestString,
        string speedCoefficientRequestString)
    {
        _upperPositionRequestString = upperPositionRequestString;
        _middlePositionRequestString = middlePositionRequestString;
        _lowerPositionRequestString = lowerPositionRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Lift Parse(Guid? id) =>
        new Lift(UpperPosition, MiddlePosition, LowerPosition, SpeedCoefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not LiftDto other)
            return false;

        return
            other.Id == Id &&
            other.UpperPosition == UpperPosition &&
            other.MiddlePosition == MiddlePosition &&
            other.LowerPosition == LowerPosition &&
            other.SpeedCoefficient == SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            UpperPosition,
            _upperPositionRequestString,
            MiddlePosition,
            _middlePositionRequestString,
            LowerPosition,
            _lowerPositionRequestString,
            HashCode.Combine(
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static LiftDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var liftSection = section.GetSection("Lift");

        var upperPositionSection = liftSection.GetSection("UpperPosition");
        var upperPositionDefault = upperPositionSection.GetSection("Default").Get<double>();
        var upperPositionRequestString = upperPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlePositionSection = liftSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<double>();
        var middlePositionRequestString = middlePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lowerPositionSection = liftSection.GetSection("LowerPosition");
        var lowerPositionDefault = lowerPositionSection.GetSection("Default").Get<double>();
        var lowerPositionRequestString = lowerPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = liftSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new LiftDto(
            upperPositionRequestString,
            middlePositionRequestString,
            lowerPositionRequestString,
            speedCoefficientRequestString)
        {
            UpperPosition = upperPositionDefault,
            MiddlePosition = middlePositionDefault,
            LowerPosition = lowerPositionDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}