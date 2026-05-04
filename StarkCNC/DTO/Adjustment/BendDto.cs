using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class BendDto : ObservableObject, ICloneable
{
    private string _forwardPositionLimitationRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;
    private string _slowdownSpeedRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _forwardPositionLimitation;

    [ObservableProperty]
    private float _speedCoefficient;

    [ObservableProperty]
    private float _slowdownSpeed;

    public BendDto(
        string forwardPositionLimitationRequestString,
        string speedCoefficientRequestString,
        string slowdownSpeedRequestString)
    {
        _forwardPositionLimitationRequestString = forwardPositionLimitationRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
        _slowdownSpeedRequestString = slowdownSpeedRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Bend Parse(Guid? id) =>
        new Bend(ForwardPositionLimitation, SpeedCoefficient, SlowdownSpeed) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not BendDto other)
            return false;

        return
            other.Id == Id &&
            other.ForwardPositionLimitation == ForwardPositionLimitation &&
            other.SpeedCoefficient == SpeedCoefficient &&
            other.SlowdownSpeed == SlowdownSpeed;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            ForwardPositionLimitation,
            _forwardPositionLimitationRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString,
            SlowdownSpeed,
            _slowdownSpeedRequestString);

    public static BendDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var bendSection = section.GetSection("Bend");

        var forwardPositionLimitationSection = bendSection.GetSection("ForwardPositionLimitation");
        var forwardPositionLimitationDefault = forwardPositionLimitationSection.GetSection("Default").Get<float>();
        var forwardPositionLimitationRequestString = forwardPositionLimitationSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = bendSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var slowdownSpeedSection = bendSection.GetSection("SlowdownSpeed");
        var slowdownSpeedDefault = slowdownSpeedSection.GetSection("Default").Get<float>();
        var slowndownSpeedRequestString = slowdownSpeedSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new BendDto(
            forwardPositionLimitationRequestString,
            speedCoefficientRequestString,
            slowndownSpeedRequestString)
        {
            ForwardPositionLimitation = forwardPositionLimitationDefault,
            SpeedCoefficient = speedCoefficientDefault,
            SlowdownSpeed = slowdownSpeedDefault,
        };
    }
}