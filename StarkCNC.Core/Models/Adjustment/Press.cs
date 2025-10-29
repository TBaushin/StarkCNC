using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Press : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public double DangerZoneCoordinate { get; set; }
    public double Length { get; set; }
    public double ForwardPosition { get; set; }
    public double MiddlePosition { get; set; }
    public double BackwardPosition { get; set; }
    public double SpeedCoefficient { get; set; }

    public Press(
        double dangerZoneCoordinate,
        double length,
        double forwardPosition,
        double middlePosition,
        double backwardPosition,
        double speedCoefficient)
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