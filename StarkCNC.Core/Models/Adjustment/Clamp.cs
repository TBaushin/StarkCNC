using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Adjustment;

public class Clamp : ICloneable
{
    private string _deepRequestString = string.Empty;
    private string _lengthRequestString = string.Empty;

    public double Deep { get; set; }
    public double Length { get; set; }

    public Clamp(double deep, string deepRequestString, double length, string lengthRequestString)
    {
        Deep = deep;
        _deepRequestString = deepRequestString;
        Length = length;
        _lengthRequestString = lengthRequestString;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Clamp other)
            return false;

        return Deep == other.Deep && Length == other.Length;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Deep, _deepRequestString, Length, _lengthRequestString);

    public static Clamp ReadConfiguration(IConfigurationSection section)
    {
        var clampSection = section.GetSection("Clamp");

        var deepSection = clampSection.GetSection("Deep");
        var deepDefault = deepSection.GetSection("Default").Get<double>();
        var deepRequestString = deepSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var lengthSection = clampSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<double>();
        var lengthRequestString = lengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Clamp(deepDefault, deepRequestString, lengthDefault, lengthRequestString);
    }
}
