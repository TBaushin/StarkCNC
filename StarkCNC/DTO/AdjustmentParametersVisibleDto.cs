using StarkCNC.Core.Models;

namespace StarkCNC.DTO;

public class AdjustmentParametersVisibleDto
{
    public string Name { get; set; } = string.Empty;

    public double PipeDiameter { get; set; }

    public double Radius { get; set; }

    public AdjustmentType AdjustmentType { get; set; } = AdjustmentType.Winding;
}
