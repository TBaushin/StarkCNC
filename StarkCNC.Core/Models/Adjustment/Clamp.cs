using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models.Adjustment;

public class Clamp : ICloneable
{
    [Key]
    public Guid Id { get; set; }
    public float Deep { get; set; }
    public float Length { get; set; }
    public float ForwardPosition { get; set; }
    public float MiddlePosition { get; set; }
    public float BackwardPosition { get; set; }
    public float SpeedCoefficient { get; set; }

    public Clamp(
        float deep,
        float length,
        float forwardPosition,
        float middlePosition,
        float backwardPosition,
        float speedCoefficient)
    {
        Deep = deep;
        Length = length;
        ForwardPosition = forwardPosition;
        MiddlePosition = middlePosition;
        BackwardPosition = backwardPosition;
        SpeedCoefficient = speedCoefficient;
    }

    public object Clone() => MemberwiseClone();

    public override bool Equals(object? obj)
    {
        if (obj is not Clamp other)
            return false;

        return
            Deep == other.Deep &&
            Length == other.Length &&
            ForwardPosition == other.ForwardPosition &&
            MiddlePosition == other.MiddlePosition &&
            BackwardPosition == other.BackwardPosition &&
            SpeedCoefficient == other.SpeedCoefficient;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Deep,
            Length,
            ForwardPosition,
            MiddlePosition,
            BackwardPosition,
            SpeedCoefficient);
}