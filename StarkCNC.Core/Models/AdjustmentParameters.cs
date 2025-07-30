namespace StarkCNC.Core.Models
{
    public class AdjustmentParameters
    {
        public string Name { get; set; } = string.Empty;

        public double PipeDiameter { get; set; } = 50;

        public string Type = string.Empty;

        public AdjustmentParameters(string name)
        {
            Name = name;
        }
    }
}
