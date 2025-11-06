using StarkCNC.Core.Models;

namespace StarkCNC.Core.Calculations;

public interface IBendCalculation
{
    public BendingData BendParameters { get; set; }

    public ICollection<BendPositions> CalculateBend(double pipeDiameter, double carriagePos);
}