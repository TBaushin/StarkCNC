using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class SupportDto : ObservableObject, ICloneable
{
    private string _frontLiftBanRequestString = string.Empty;
    private string _middleLiftBanRequestString = string.Empty;
    private string _backLiftBanRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _frontLiftBan;

    [ObservableProperty]
    private double _middleLiftBan;

    [ObservableProperty]
    private double _backLiftBan;

    public SupportDto(
        string frontLiftBanRequestString,
        string middleLiftBanRequestString,
        string backLiftBanRequestString)
    {
        _frontLiftBanRequestString = frontLiftBanRequestString;
        _middleLiftBanRequestString = middleLiftBanRequestString;
        _backLiftBanRequestString = backLiftBanRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Support Parse(Guid? id) =>
        new Support(FrontLiftBan, MiddleLiftBan, BackLiftBan) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not SupportDto other)
            return false;

        return
            Id == other.Id &&
            FrontLiftBan == other.FrontLiftBan &&
            MiddleLiftBan == other.MiddleLiftBan &&
            BackLiftBan == other.BackLiftBan;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            FrontLiftBan,
            _frontLiftBanRequestString,
            MiddleLiftBan,
            _middleLiftBanRequestString,
            BackLiftBan,
            _backLiftBanRequestString);

    public static SupportDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var supportSection = section.GetSection("Support");

        var frontLiftBanSection = supportSection.GetSection("FrontLiftBan");
        var frontLiftBanDefault = frontLiftBanSection.GetSection("Default").Get<double>();
        var frontLiftBanRequestString = frontLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var middleLiftBanSection = supportSection.GetSection("MiddleLiftBan");
        var middleLiftBanDefault = middleLiftBanSection.GetSection("Default").Get<double>();
        var middleLiftBanRequestString = middleLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var backLiftBanSection = supportSection.GetSection("BackLiftBan");
        var backLiftBanDefault = backLiftBanSection.GetSection("Default").Get<double>();
        var backLiftBanRequestString = backLiftBanSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new SupportDto(
            frontLiftBanRequestString,
            middleLiftBanRequestString,
            backLiftBanRequestString)
            {
                FrontLiftBan = frontLiftBanDefault,
                MiddleLiftBan = middleLiftBanDefault,
                BackLiftBan = backLiftBanDefault
            };
    }
}