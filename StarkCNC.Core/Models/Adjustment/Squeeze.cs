using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Squeeze : ICloneable
{
    private string _turnOnRequestString;

    public bool TurnOn { get; set; }

    public Squeeze(bool turnOn, string turnOnRequestString)
    {
        TurnOn = turnOn;
        _turnOnRequestString = turnOnRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Squeeze other)
            return false;

        return TurnOn == other.TurnOn;
    }

    public override int GetHashCode() =>
        HashCode.Combine(TurnOn, _turnOnRequestString);

    public static Squeeze ReadConfiguration(IConfigurationSection section)
    {
        var squeezeSection = section.GetSection("Squeeze");

        var turnOnSection = squeezeSection.GetSection("TurnOn");
        var turnOnDefault = turnOnSection.GetSection("Default").Get<bool>();
        var turnOnRequestString = turnOnSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Squeeze(turnOnDefault, turnOnRequestString);
    }
}
