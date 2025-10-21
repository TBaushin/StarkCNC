namespace StarkCNC.Core.Models.Adjustment;

public class Rotation : ICloneable
{
    public Guid Id { get; set; }
    public double OffsetAfterZeroSearch { get; set; }
    public double SpeedCoefficient { get; set; }

    public Rotation(
        double offsetAfterZeroSearch,
        double speedCoefficient)
    {
        OffsetAfterZeroSearch = offsetAfterZeroSearch;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Rotation other)
            return false;

        return
            other.SpeedCoefficient == SpeedCoefficient &&
            other.OffsetAfterZeroSearch == OffsetAfterZeroSearch;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            OffsetAfterZeroSearch,
            SpeedCoefficient);
}