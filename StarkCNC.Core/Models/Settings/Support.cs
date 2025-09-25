using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Support
{
    private string _frontLiftBanRequestString = string.Empty;
    private string _middleLiftBanRequestString = string.Empty;
    private string _backLiftBanRequestString = string.Empty;

    public double FrontLiftBan { get; set; }
    public double MiddleLiftBan { get; set; }
    public double BackLiftBan { get; set; }

    public Support(
        double frontLiftBan,
        string frontLiftBanRequestString,
        double middleLiftBan,
        string middleLiftBanRequestString,
        double backLiftBan,
        string backLiftBanRequestString)
    {
        FrontLiftBan = frontLiftBan;
        _frontLiftBanRequestString = frontLiftBanRequestString;
        MiddleLiftBan = middleLiftBan;
        _middleLiftBanRequestString = middleLiftBanRequestString;
        BackLiftBan = backLiftBan;
        _backLiftBanRequestString = backLiftBanRequestString;
    }

    public static Support ReadConfiguration(IConfigurationSection section)
    {
        var supportSection = section.GetSection("Support");

        var frontLiftBanSection = supportSection.GetSection("FrontLiftBan");
        var frontLiftBanDefault = frontLiftBanSection.GetSection("Default").Get<double>();
        var frontLiftBanRequestString = frontLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middlefLiftBanSection = supportSection.GetSection("MiddleLiftBan");
        var middlefLiftBanDefault = middlefLiftBanSection.GetSection("Default").Get<double>();
        var middlefLiftBanRequestString = middlefLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backLiftBanSection = supportSection.GetSection("BackLiftBan");
        var backLiftBanDefault = backLiftBanSection.GetSection("Default").Get<double>();
        var backLiftBanRequestString = backLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Support(
            frontLiftBanDefault,
            frontLiftBanRequestString,
            middlefLiftBanDefault,
            middlefLiftBanRequestString,
            backLiftBanDefault,
            backLiftBanRequestString);
    }
}
