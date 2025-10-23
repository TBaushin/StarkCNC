namespace StarkCNC.Core.Models.SettingsParameters;

public class Bend : ICloneable
{
    public Guid Id { get; set; }
    public double Coefficient { get; set; }
    public bool Synchronization { get; set; }

    public Bend(
        double coefficient,
        bool synchronization)
    {
        Coefficient = coefficient;
        Synchronization = synchronization;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Bend other)
            return false;

        return
            Id == other.Id &&
            Coefficient == other.Coefficient &&
            Synchronization == other.Synchronization;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            Coefficient,
            Synchronization);
}