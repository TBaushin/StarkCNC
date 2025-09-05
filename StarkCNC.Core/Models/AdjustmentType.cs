namespace StarkCNC.Core.Models;

public class AdjustmentType
{
    public string Name { get; set; }

    public AdjustmentType(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AdjustmentType other)
            return false;

        return other.Name == Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}
