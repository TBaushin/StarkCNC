namespace StarkCNC.Core.Models;

public class AdjustmentParameters
{
    public string Name { get; set; } = string.Empty;

    public double PipeDiameter { get; set; }

    public double Radius { get; set; }

    public AdjustmentType Type { get; set; }

    public int InstalledLevel { get; set; }

    public double ClampLength { get; set; }

    public double PressLength { get; set; }

    public AdjustmentParameters(string name, AdjustmentType type)
    {
        Name = name;
        Type = type;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AdjustmentParameters parameter)
            return false;

        return parameter.Name == Name && parameter.PipeDiameter == PipeDiameter && parameter.Radius == Radius &&
            parameter.Type == Type && parameter.InstalledLevel == InstalledLevel;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Name, PipeDiameter, Type, InstalledLevel);
    

    public AdjustmentParameters Copy() =>
        new AdjustmentParameters(Name, Type) {
            PipeDiameter = PipeDiameter,
            Radius = Radius,
            InstalledLevel = InstalledLevel,
            ClampLength = ClampLength,
            PressLength = PressLength
        };
    
}
