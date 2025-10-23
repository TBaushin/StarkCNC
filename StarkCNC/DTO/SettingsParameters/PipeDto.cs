using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.DTO.SettingsParameters;

public partial class PipeDto : ObservableObject, ICloneable
{
    private string _outletCoordinateRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _outletCoordinate;

    public PipeDto(string outletCoordinateRequestString)
    {
        _outletCoordinateRequestString = _outletCoordinateRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Pipe Parse(Guid? id) =>
        new Pipe(OutletCoordinate) { Id = id ?? Guid.NewGuid() };

    public override bool Equals(object? obj)
    {
        if (obj is not PipeDto other)
            return false;

        return
            Id == other.Id &&
            OutletCoordinate == other.OutletCoordinate;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, OutletCoordinate, _outletCoordinateRequestString);

    public static PipeDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var pipeSection = section.GetSection("Pipe");

        var outletCoordinateSection = pipeSection.GetSection("OutletCoordinate");
        var outletCoordinateDefault = outletCoordinateSection.GetSection("Default").Get<double>();
        var outletCoordinateRequestString = outletCoordinateSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new PipeDto(outletCoordinateRequestString) { OutletCoordinate = outletCoordinateDefault };
    }
}