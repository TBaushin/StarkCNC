using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class RotationDto : ObservableObject, ICloneable
{
    private string _offsetRequestString = string.Empty;
    private string _coefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _offset;

    [ObservableProperty]
    private double _coefficient;

    public RotationDto(string offsetRequestString, string coefficientRequestString)
    {
        _offsetRequestString = offsetRequestString;
        _coefficientRequestString = coefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Rotation Parse(Guid? id) =>
        new Rotation(Offset, Coefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not RotationDto other)
            return false;

        return
            Id == other.Id &&
            Offset == other.Offset &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Offset,
            _offsetRequestString,
            Coefficient,
            _coefficientRequestString);

    public static RotationDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var rotationSection = section.GetSection("Rotation");

        var offsetSection = rotationSection.GetSection("Offset");
        var offsetDefault = offsetSection.GetSection("Default").Get<double>();
        var offsetRequestString = offsetSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var coefficientSection = rotationSection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new RotationDto(offsetRequestString, coefficientRequestString)
        {
            Offset = offsetDefault,
            Coefficient = coefficientDefault
        };
    }
}