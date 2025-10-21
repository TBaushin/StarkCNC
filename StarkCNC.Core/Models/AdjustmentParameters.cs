using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.Core.Models;

public class AdjustmentParameters : ICloneable
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double PipeDiameter { get; set; }

    public double Radius { get; set; }

    public AdjustmentType Type { get; set; }

    public int InstalledLevel { get; set; }

    public double ForwardDangerZoneCoordinate { get; set; }

    public double DistanceFromCenter { get; set; }

    public Guid BendId { get; set; }
    public Bend Bend { get; set; }

    public Guid BendRollerId { get; set; }
    public BendRoller BendRoller { get; set; }
    
    public Guid ClampId { get; set; }
    public Clamp Clamp { get; set; }

    public Guid ClampRollerId { get; set; }
    public ClampRoller ClampRoller { get; set; }

    public Guid ConsoleId { get; set; }
    public StarkCNC.Core.Models.Adjustment.Console Console { get; set; }

    public Guid DornId { get; set; }
    public Dorn Dorn { get; set; }

    public Guid LiftId { get; set; }
    public Lift Lift { get; set; }

    public Guid PressId { get; set; }
    public Press Press { get; set; }

    public Guid RotationId { get; set; }
    public Rotation Rotation { get; set; }

    public Guid SqueezeId { get; set; }
    public Squeeze Squeeze { get; set; }

    public Guid SupplyId { get; set; }
    public Supply Supply { get; set; }

    public AdjustmentParameters(
        Guid id,
        string name,
        double pipeDiameter,
        double radius,
        AdjustmentType type,
        int installedLevel,
        double forwardDangerZoneCoordinate,
        double distanceFromCenter,
        Bend bend,
        BendRoller bendRoller,
        Clamp clamp,
        ClampRoller clampRoller,
        StarkCNC.Core.Models.Adjustment.Console console,
        Dorn dorn,
        Lift lift,
        Press press,
        Rotation rotation,
        Squeeze squeeze,
        Supply supply)
    {
        Id = id;
        Name = name;
        PipeDiameter = pipeDiameter;
        Radius = radius;
        Type = type;
        InstalledLevel = installedLevel;
        ForwardDangerZoneCoordinate = forwardDangerZoneCoordinate;
        DistanceFromCenter = distanceFromCenter;

        Bend = bend;
        BendRoller = bendRoller;
        Clamp = clamp;
        ClampRoller = clampRoller;
        Console = console;
        Dorn = dorn;
        Lift = lift;
        Press = press;
        Rotation = rotation;
        Squeeze = squeeze;
        Supply = supply;
    }

    public AdjustmentParameters() { }

    public override bool Equals(object? obj)
    {
        if (obj is not AdjustmentParameters other)
            return false;

        return
            other.Name == Name &&
            other.PipeDiameter == PipeDiameter &&
            other.Radius == Radius &&
            other.Type == Type &&
            other.InstalledLevel == InstalledLevel &&
            other.ForwardDangerZoneCoordinate == ForwardDangerZoneCoordinate &&
            other.DistanceFromCenter == DistanceFromCenter &&
            other.Bend.Equals(Bend) &&
            other.BendRoller.Equals(BendRoller) &&
            other.Clamp.Equals(Clamp) &&
            other.ClampRoller.Equals(ClampRoller) &&
            other.Console.Equals(Console) &&
            other.Dorn.Equals(Dorn) &&
            other.Lift.Equals(Lift) &&
            other.Press.Equals(Press) &&
            other.Rotation.Equals(Rotation) &&
            other.Squeeze.Equals(Squeeze) &&
            other.Supply.Equals(Supply);
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Name,
            PipeDiameter,
            Type,
            InstalledLevel,
            ForwardDangerZoneCoordinate,
            DistanceFromCenter,
            GetHashCodeFromBToD(),
            GetHashCodeFromLToS());

    private int GetHashCodeFromBToD() =>
        HashCode.Combine(
            Bend.GetHashCode(),
            BendRoller.GetHashCode(),
            Clamp.GetHashCode(),
            ClampRoller.GetHashCode(),
            Console.GetHashCode(),
            Dorn.GetHashCode());

    private int GetHashCodeFromLToS() =>
        HashCode.Combine(
            Lift.GetHashCode(),
            Press.GetHashCode(),
            Rotation.GetHashCode(),
            Squeeze.GetHashCode(),
            Supply.GetHashCode());
    
    public object Clone() =>
        new AdjustmentParameters(
            Id,
            Name,
            PipeDiameter,
            Radius,
            Type,
            InstalledLevel,
            ForwardDangerZoneCoordinate,
            DistanceFromCenter,
            (Bend)Bend.Clone(),
            (BendRoller)BendRoller.Clone(),
            (Clamp)Clamp.Clone(),
            (ClampRoller)ClampRoller.Clone(),
            (StarkCNC.Core.Models.Adjustment.Console)Console.Clone(),
            (Dorn)Dorn.Clone(),
            (Lift)Lift.Clone(),
            (Press)Press.Clone(),
            (Rotation)Rotation.Clone(),
            (Squeeze)Squeeze.Clone(),
            (Supply)Supply.Clone());
}