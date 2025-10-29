using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.SettingsParameters;

public class Console : ICloneable
{
    [Key]
    public Guid Id { get; set; }

    public double Coefficient { get; set; }

    public Console(double coefficient)
    {
        Coefficient = coefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not StarkCNC.Core.Models.SettingsParameters.Console other)
            return false;

        return
            Id == other.Id &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Coefficient);
}