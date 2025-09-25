using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Bend
{
    private string _coefficientRequestString = string.Empty;
    private string _synchronizationRequestString = string.Empty;

    public double Coefficient { get; set; }
    public bool Synchronization { get; set; }

    public Bend(
        double coefficient,
        string coefficientRequestString,
        bool synchronization,
        string synchronizationRequestString)
    {
        Coefficient = coefficient;
        _coefficientRequestString = coefficientRequestString;
        Synchronization = synchronization;
        _synchronizationRequestString = synchronizationRequestString;
    }

    public static Bend ReadConfiguration(IConfigurationSection section)
    {
        var bendSection = section.GetSection("Bend");

        var coefficientSection = bendSection.GetSection("Coefficient");
        var coefficientDefault = coefficientSection.GetSection("Default").Get<double>();
        var coefficientRequestString = coefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var synchronizationSection = bendSection.GetSection("Synchronization");
        var synchronizationDefault = synchronizationSection.GetSection("Default").Get<bool>();
        var synchronizationRequestString = synchronizationSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Bend(coefficientDefault, coefficientRequestString, synchronizationDefault, synchronizationRequestString);
    }
}
