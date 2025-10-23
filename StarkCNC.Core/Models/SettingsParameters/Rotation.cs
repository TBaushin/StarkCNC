namespace StarkCNC.Core.Models.SettingsParameters;

public class Rotation : ICloneable
{
    public Guid Id { get; set; }
    public double Offset { get; set; }
    public double Coefficient { get; set; }

    public Rotation(double offset, double coefficient)
    {
        Offset = offset;
        Coefficient = coefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Rotation other)
            return false;

        return
            Id == other.Id &&
            Offset == other.Offset &&
            Coefficient == other.Coefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Offset, Coefficient);
}