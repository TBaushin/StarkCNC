using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Rotation : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float OffsetAfterZeroSearch { get; set; }

    public Rotation(
        float offsetAfterZeroSearch)
    {
        OffsetAfterZeroSearch = offsetAfterZeroSearch;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Rotation other)
            return false;

        return other.OffsetAfterZeroSearch == OffsetAfterZeroSearch;
    }

    public override int GetHashCode() =>
        HashCode.Combine(OffsetAfterZeroSearch);
}