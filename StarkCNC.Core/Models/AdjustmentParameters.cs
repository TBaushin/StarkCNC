namespace StarkCNC.Core.Models
{
    public class AdjustmentParameters
    {
        public string Name { get; set; } = string.Empty;

        public double PipeDiameter { get; set; } = 50;

        public AdjustmentType Type { get; set; }

        public int? InstalledLevel { get; set; }

        public AdjustmentParameters(string name, AdjustmentType type)
        {
            Name = name;
            Type = type;
        }

        public AdjustmentParameters Copy()
        {
            return new AdjustmentParameters(Name, Type) { PipeDiameter = PipeDiameter };
        }
    }
}
