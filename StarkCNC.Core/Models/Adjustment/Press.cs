using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Press : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float DangerZoneCoordinate { get; set; }
    public float Length { get; set; }
    public float ForwardPosition { get; set; }
    public float MiddlePosition { get; set; }
    public float BackwardPosition { get; set; }
    public float SpeedCoefficient { get; set; }

    public Press(
        float dangerZoneCoordinate,
        float length,
        float forwardPosition,
        float middlePosition,
        float backwardPosition,
        float speedCoefficient)
    {
        DangerZoneCoordinate = dangerZoneCoordinate;
        Length = length;
        ForwardPosition = forwardPosition;
        MiddlePosition = middlePosition;
        BackwardPosition = backwardPosition;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Press other)
            return false;

        return
            DangerZoneCoordinate == other.DangerZoneCoordinate &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            DangerZoneCoordinate,
            Length,
            ForwardPosition,
            MiddlePosition,
            BackwardPosition,
            SpeedCoefficient);
}