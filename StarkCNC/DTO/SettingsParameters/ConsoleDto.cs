using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace StarkCNC.DTO.SettingsParameters;

public partial class ConsoleDto : ObservableObject, ICloneable
{
    private string _coefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _coefficient;

    public ConsoleDto(string coefficientRequestString)
    {
        _coefficientRequestString = coefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public StarkCNC.Core.Models.SettingsParameters.Console Parse(Guid? id) =>
        new StarkCNC.Core.Models.SettingsParameters.Console(Coefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not ConsoleDto other)
            return false;

        return
            Id == other.Id &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Coefficient, _coefficientRequestString);

    public static ConsoleDto? CreateFromConfiguration(IConfigurationSection section)
    {
        var consoleSection = section.GetSection("Console");

        var coefficientSection = consoleSection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ConsoleDto(coefficientRequestString)
        {
            Coefficient = coefficientDefault
        };
    }
}