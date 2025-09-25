using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Console
{
    private string _coefficientRequestString = string.Empty;

    public double Coefficient { get; set; }

    public Console(double coefficient, string coefficientRequestString)
    {
        Coefficient = coefficient;
        _coefficientRequestString = coefficientRequestString;
    }

    public static Console ReadConfiguration(IConfigurationSection section)
    {
        var consoleSection = section.GetSection("Console");

        var coefficientSection = consoleSection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Console(coefficientDefault, coefficientRequestString);
    }
}
