using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Dorn
{
    private string _automaticRequestString = string.Empty;
    private string _lubricantTurnOnRequestString = string.Empty;
    private string _leadWithdrawalBeforeBendRequestString = string.Empty;

    public bool Automatic { get; set; }
    public bool LubricantTurnOn { get; set; }
    public double LeadWithdrawalBeforeBend { get; set; }

    public Dorn(
        bool automatic,
        string automaticRequestString,
        bool lubricantTurnOn,
        string lubricantTurnOnRequestString,
        double leadWithdrawalBeforeBend,
        string leadWithdrawalBeforeBendRequestString)
    {
        Automatic = automatic;
        _automaticRequestString = automaticRequestString;
        LubricantTurnOn = lubricantTurnOn;
        _lubricantTurnOnRequestString = lubricantTurnOnRequestString;
        LeadWithdrawalBeforeBend = leadWithdrawalBeforeBend;
        _leadWithdrawalBeforeBendRequestString = leadWithdrawalBeforeBendRequestString;
    }

    public static Dorn ReadConfiguration(IConfigurationSection section)
    {
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

        return new Dorn(
            automaticDefault,
            automaticRequestString,
            lubricantTurnOnDefault,
            lubricantTurnOnRequestString,
            leadWithdrawalBeforeBendDefault,
            leadWithdrawalBeforeBendRequestString);
    }
}
