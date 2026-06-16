using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Console : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float BendPosition { get; set; }
    public float SecondFloorPosition { get; set; }
    public float SecondFloorIntermediatePosition { get; set; }
    public float ThirdFloorPosition { get; set; }
    public float PipeRotationDepartureDistance { get; set; }

    public Console(
        float bendPosition,
        float secondFloorPosition,
        float secondFloorIntermediatePosition,
        float thirdFloorPosition,
        float pipeRotationDepartureDistance)
    {
        BendPosition = bendPosition;
        SecondFloorPosition = secondFloorPosition;
        SecondFloorIntermediatePosition = secondFloorIntermediatePosition;
        ThirdFloorPosition = thirdFloorPosition;
        PipeRotationDepartureDistance = pipeRotationDepartureDistance;
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
            PipeRotationDepartureDistance == other.PipeRotationDepartureDistance;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            BendPosition,
            SecondFloorPosition,
            SecondFloorIntermediatePosition,
            ThirdFloorPosition,
            PipeRotationDepartureDistance);
}