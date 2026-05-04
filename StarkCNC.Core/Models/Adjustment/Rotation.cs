using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Rotation : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float OffsetAfterZeroSearch { get; set; }
    public float SpeedCoefficient { get; set; }

    public Rotation(
        float offsetAfterZeroSearch,
        float speedCoefficient)
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