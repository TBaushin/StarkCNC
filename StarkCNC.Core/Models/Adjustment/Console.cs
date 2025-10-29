using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Console : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public double BendPosition { get; set; }
    public double SecondFloorPosition { get; set; }
    public double SecondFloorIntermediatePosition { get; set; }
    public double ThirdFloorPosition { get; set; }
    public double PipeRotationDepartureDistance { get; set; }
    public double SpeedCoefficient { get; set; }

    public Console(
        double bendPosition,
        double secondFloorPosition,
        double secondFloorIntermediatePosition,
        double thirdFloorPosition,
        double pipeRotationDepartureDistance,
        double speedCoefficient)
    {
        BendPosition = bendPosition;
        SecondFloorPosition = secondFloorPosition;
        SecondFloorIntermediatePosition = secondFloorIntermediatePosition;
        ThirdFloorPosition = thirdFloorPosition;
        PipeRotationDepartureDistance = pipeRotationDepartureDistance;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Console other)
            return false;

        return
            BendPosition == other.BendPosition &&
            SecondFloorPosition == other.SecondFloorPosition &&
            SecondFloorIntermediatePosition == other.SecondFloorIntermediatePosition &&
            ThirdFloorPosition == other.ThirdFloorPosition &&
            PipeRotationDepartureDistance == other.PipeRotationDepartureDistance &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            BendPosition,
            SecondFloorPosition,
            SecondFloorIntermediatePosition,
            ThirdFloorPosition,
            PipeRotationDepartureDistance,
            SpeedCoefficient);
}