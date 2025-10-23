using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class DornDto : ObservableObject, ICloneable
{
    private string _automaticRequestString = string.Empty;
    private string _lubricantTurnOnRequestString = string.Empty;
    private string _leadWithdrawalBeforeBendRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private bool _automatic;

    [ObservableProperty]
    private bool _lubricantTurnOn;

    [ObservableProperty]
    private double _leadWithdrawalBeforeBend;

    public DornDto(
        string automaticRequestString,
        string lubricantTurnOnRequestString,
        string leadWithdrawalBeforeBendRequestString)
    {
        _automaticRequestString = automaticRequestString;
        _lubricantTurnOnRequestString = lubricantTurnOnRequestString;
        _leadWithdrawalBeforeBendRequestString = leadWithdrawalBeforeBendRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Dorn Parse(Guid? id) =>
        new Dorn(Automatic, LubricantTurnOn, LeadWithdrawalBeforeBend) { Id = id ?? Guid.NewGuid() };

    public override bool Equals(object? obj)
    {
        if (obj is not DornDto other)
            return false;

        return
            Automatic == other.Automatic &&
            LubricantTurnOn == other.LubricantTurnOn &&
            LeadWithdrawalBeforeBend == other.LeadWithdrawalBeforeBend;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Automatic,
            _automaticRequestString,
            LubricantTurnOn,
            _lubricantTurnOnRequestString,
            LeadWithdrawalBeforeBend,
            _leadWithdrawalBeforeBendRequestString);

    public static DornDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var dornSection = section.GetSection("Dorn");

        var automaticSection = dornSection.GetSection("Automatic");
        var automaticDefault = automaticSection.GetSection("Default").Get<bool>();
        var automaticRequestString = automaticSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lubricantTurnOnSection = dornSection.GetSection("LubricantTurnOn");
        var lubricantTurnOnDefault = lubricantTurnOnSection.GetSection("Default").Get<bool>();
        var lubricantTurnOnRequestString = lubricantTurnOnSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var leadWithdrawalBeforeBendSection = dornSection.GetSection("LeadWithdrawalBeforeBend");
        var leadWithdrawalBeforeBendDefault = leadWithdrawalBeforeBendSection.GetSection("Default").Get<double>();
        var leadWithdrawalBeforeBendRequestString = leadWithdrawalBeforeBendSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new DornDto(
            automaticRequestString,
            lubricantTurnOnRequestString,
            leadWithdrawalBeforeBendRequestString)
            {
                Automatic = automaticDefault,
                LubricantTurnOn = lubricantTurnOnDefault,
                LeadWithdrawalBeforeBend = leadWithdrawalBeforeBendDefault
            };
    }
}