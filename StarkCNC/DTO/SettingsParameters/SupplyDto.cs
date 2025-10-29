using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class SupplyDto : ObservableObject, ICloneable
{
    private string _resetedOffsetRequestString = string.Empty;
    private string _startRollingSpeedRequestString = string.Empty;
    private string _coefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _resetedOffset;

    [ObservableProperty]
    private double _startRollingSpeed;

    [ObservableProperty]
    private double _coefficient;

    public SupplyDto(
        string resetedOffsetRequestString,
        string startRollingSpeedRequestString,
        string coefficientRequestString)
    {
        _resetedOffsetRequestString = resetedOffsetRequestString;
        _startRollingSpeedRequestString = startRollingSpeedRequestString;
        _coefficientRequestString = coefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Supply Parse(Guid? id) =>
        new Supply(ResetedOffset, StartRollingSpeed, Coefficient) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not SupplyDto other)
            return false;

        return
            Id == other.Id &&
            ResetedOffset == other.ResetedOffset &&
            StartRollingSpeed == other.StartRollingSpeed &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            ResetedOffset,
            _resetedOffsetRequestString,
            StartRollingSpeed,
            _startRollingSpeedRequestString,
            Coefficient,
            _coefficientRequestString);

    public static SupplyDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var supplySection = section.GetSection("Rotation");

        var resetedOffsetSection = supplySection.GetSection("ResetedOffset");
        var resetedOffsetDefault = resetedOffsetSection.GetSection("Default").Get<double>();
        var resetedOffsetRequestString = resetedOffsetSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var startRollingSpeedSection = supplySection.GetSection("StartRollingSpeed");
        var startRollingSpeedDefault = startRollingSpeedSection.GetSection("Default").Get<double>();
        var startRollingSpeedRequestString = startRollingSpeedSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var coefficientSection = supplySection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new SupplyDto(
            resetedOffsetRequestString,
            startRollingSpeedRequestString,
            coefficientRequestString)
            {
                ResetedOffset = resetedOffsetDefault,
                StartRollingSpeed = startRollingSpeedDefault,
                Coefficient = coefficientDefault
            };
    }
}