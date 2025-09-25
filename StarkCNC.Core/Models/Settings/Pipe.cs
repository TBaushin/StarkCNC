using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.Settings;

public class Pipe
{
    private string _outletCoordinateRequestString = string.Empty;

    public double OutletCoordinate { get; set; }

    public Pipe(double outletCoordinate, string outletCoordinateRequestString)
    {
        OutletCoordinate = outletCoordinate;
        _outletCoordinateRequestString = outletCoordinateRequestString;
    }

    public static Pipe ReadConfiguration(IConfigurationSection section)
    {
        var pipeSection = section.GetSection("Pipe");

        var outletCoordinateSection = pipeSection.GetSection("OutletCoordinate");
        var outletCoordinateDefault = outletCoordinateSection.GetSection("Default").Get<double>();
        var outletCoordinateRequestString = outletCoordinateSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new Pipe(outletCoordinateDefault, outletCoordinateRequestString);
    }
}
