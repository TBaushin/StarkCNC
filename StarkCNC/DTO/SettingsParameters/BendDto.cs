using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class BendDto : ObservableObject, ICloneable
{
    private string _coefficientRequestString = string.Empty;
    private string _synchronizationRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _coefficient;

    [ObservableProperty]
    private bool _synchronization;

    public BendDto(string speedRequestString, string synchronizationRequestString)
    {
        _coefficientRequestString = speedRequestString;
        _synchronizationRequestString = synchronizationRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Bend Parse(Guid? id) =>
        new Bend(Coefficient, Synchronization) { Id = id ?? Guid.NewGuid() };

    public override bool Equals(object? obj)
    {
        if (obj is not BendDto other)
            return false;

        return
            Id == other.Id &&
            Coefficient == other.Coefficient &&
            Synchronization == other.Synchronization;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Coefficient,
            _coefficientRequestString,
            Synchronization,
            _synchronizationRequestString);

    public static BendDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var coefficientSection = section.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var synchronizationSection = section.GetSection("Synchronization");
        var synchronizationDefault = synchronizationSection.GetSection("Default").Get<bool>();
        var synchronizationRequestString = synchronizationSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new BendDto(coefficientRequestString, synchronizationRequestString)
        {
            Coefficient = coefficientDefault,
            Synchronization = synchronizationDefault
        };
    }
}