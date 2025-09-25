using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Rotation
{
    private string _offsetRequestString;
    private string _coefficientRequestString;

    public double Offset { get; set; }
    public double Coefficient { get; set; }

    public Rotation(double offset, string offsetRequestString, double coefficient, string coefficientRequestString)
    {
        Offset = offset;
        _offsetRequestString = offsetRequestString;
        Coefficient = coefficient;
        _coefficientRequestString = coefficientRequestString;
    }

    public static Rotation ReadConfiguration(IConfigurationSection section)
    {
        var rotationSection = section.GetSection("Rotation");

        var offsetSection = rotationSection.GetSection("Offset");
        var offsetDefault = offsetSection.GetSection("Default").Get<double>();
        var offsetRequestString = offsetSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var coefficientSection = rotationSection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Rotation(offsetDefault, offsetRequestString, coefficientDefault, coefficientRequestString);
    }
}
