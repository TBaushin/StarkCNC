using Microsoft.Extensions.Configuration;

namespace StarkCNC.Core.Models.SettingsParameters;

public class Pipe : ICloneable
{
    public Guid Id { get; set; }

    public double OutletCoordinate { get; set; }

    public Pipe(double outletCoordinate)
    {
        OutletCoordinate = outletCoordinate;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Pipe other)
            return false;

        return
            Id == other.Id &&
            OutletCoordinate == other.OutletCoordinate;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, OutletCoordinate);
}