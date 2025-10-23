namespace StarkCNC.Core.Models.SettingsParameters;

public class Supply : ICloneable
{
    public Guid Id { get; set; }

    public double ResetedOffset { get; set; }

    public double StartRollingSpeed { get; set; }

    public double Coefficient { get; set; }

    public Supply(
        double resetedOffset,
        double startRollingSpeed,
        double coefficient)
    {
        ResetedOffset = resetedOffset;
        StartRollingSpeed = startRollingSpeed;
        Coefficient = coefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Supply other)
            return false;

        return
            Id == other.Id &&
            ResetedOffset == other.ResetedOffset &&
            StartRollingSpeed == other.StartRollingSpeed &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            ResetedOffset,
            StartRollingSpeed,
            Coefficient);
}