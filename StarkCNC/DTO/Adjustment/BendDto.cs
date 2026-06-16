using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class BendDto : ObservableObject, ICloneable
{
    private string _forwardPositionLimitationRequestString = string.Empty;
    private string _speedCoefficientRequestString = string.Empty;
    private string _deflectionDuringClampClampingRequestString = string.Empty;
    private string _deflectionRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _forwardPositionLimitation;

    [ObservableProperty]
    private float _speedCoefficient;

    [ObservableProperty]
    private float _deflectionDuringClampClamping;

    [ObservableProperty]
    private float _deflection;

    public BendDto(
        string forwardPositionLimitationRequestString,
        string speedCoefficientRequestString,
        string deflectionDuringClampClampingRequestString,
        string deflectionRequestString)
    {
        _forwardPositionLimitationRequestString = forwardPositionLimitationRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
        _deflectionDuringClampClampingRequestString = deflectionDuringClampClampingRequestString;
        _deflectionRequestString = deflectionRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Bend Parse(Guid? id) =>
        new Bend(ForwardPositionLimitation, SpeedCoefficient, DeflectionDuringClampClamping, Deflection) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not BendDto other)
            return false;

        return
            other.Id == Id &&
            other.ForwardPositionLimitation == ForwardPositionLimitation &&
            other.SpeedCoefficient == SpeedCoefficient &&
            other.DeflectionDuringClampClamping == DeflectionDuringClampClamping &&
            other.Deflection == Deflection;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            ForwardPositionLimitation,
            _forwardPositionLimitationRequestString,
            SpeedCoefficient,
            _speedCoefficientRequestString,
            DeflectionDuringClampClamping,
            _deflectionDuringClampClampingRequestString,
            HashCode.Combine(
                Deflection,
                _deflectionRequestString));

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

        var deflectionDuringClampClampingSection = bendSection.GetSection("DeflectionDuringClampClamping");
        var deflectionDuringClampClampingDefault = deflectionDuringClampClampingSection.GetSection("Default").Get<float>();
        var deflectionDuringClampClampingRequestString = deflectionDuringClampClampingSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var deflectionSection = bendSection.GetSection("Deflection");
        var deflectionDefault = deflectionDuringClampClampingSection.GetSection("Default").Get<float>();
        var deflectionRequestString = deflectionDuringClampClampingSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new BendDto(
            forwardPositionLimitationRequestString,
            speedCoefficientRequestString,
            deflectionDuringClampClampingRequestString,
            deflectionRequestString)
        {
            ForwardPositionLimitation = forwardPositionLimitationDefault,
            SpeedCoefficient = speedCoefficientDefault,
            DeflectionDuringClampClamping = deflectionDuringClampClampingDefault,
            Deflection = deflectionDefault
        };
    }
}