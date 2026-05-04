using StarkCNC.Core.Models.Adjustment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarkCNC.Core.Models;

public class AdjustmentParameters : ICloneable
{
    private int _installedLevel;
    private bool _isEnabled;

    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public float PipeDiameter { get; set; }

    public float Radius { get; set; }

    public AdjustmentType Type { get; set; }

    public int InstalledLevel
    {
        get => _installedLevel;
        set
        {
            _installedLevel = value;
            if (value <= 0 || value > 3)
                IsEnabled = false;
            else
                IsEnabled = true;
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (InstalledLevel <= 0 || InstalledLevel > 3)
                _isEnabled = false;
            else
                _isEnabled = value;
        }
    }

    public float ForwardDangerZoneCoordinate { get; set; }

    public float DistanceFromCenter { get; set; }

    public Guid BendId { get; set; }

    [ForeignKey(nameof(BendId))]
    public Bend Bend { get; set; }

    public Guid BendRollerId { get; set; }

    [ForeignKey(nameof(BendRollerId))]
    public BendRoller BendRoller { get; set; }

    public Guid ClampId { get; set; }

    [ForeignKey(nameof(ClampId))]
    public Clamp Clamp { get; set; }

    public Guid ClampRollerId { get; set; }

    [ForeignKey(nameof(ClampRollerId))]
    public ClampRoller ClampRoller { get; set; }

    public Guid ConsoleId { get; set; }

    [ForeignKey(nameof(ConsoleId))]
    public StarkCNC.Core.Models.Adjustment.Console Console { get; set; }

    public Guid DornId { get; set; }

    [ForeignKey(nameof(DornId))]
    public Dorn Dorn { get; set; }

    public Guid LiftId { get; set; }

    [ForeignKey(nameof(LiftId))]
    public Lift Lift { get; set; }

    public Guid PressId { get; set; }

    [ForeignKey(nameof(PressId))]
    public Press Press { get; set; }

    public Guid RotationId { get; set; }

    [ForeignKey(nameof(RotationId))]
    public Rotation Rotation { get; set; }

    public Guid SqueezeId { get; set; }

    [ForeignKey(nameof(SqueezeId))]
    public Squeeze Squeeze { get; set; }

    public Guid SupplyId { get; set; }

    [ForeignKey(nameof(SupplyId))]
    public Supply Supply { get; set; }

    public AdjustmentParameters(
        Guid id,
        string name,
        float pipeDiameter,
        float radius,
        AdjustmentType type,
        int installedLevel,
        bool isEnabled,
        float forwardDangerZoneCoordinate,
        float distanceFromCenter,
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
        IsEnabled = isEnabled;
        ForwardDangerZoneCoordinate = forwardDangerZoneCoordinate;
        DistanceFromCenter = distanceFromCenter;

        Bend = bend;
        BendId = Bend.Id;
        BendRoller = bendRoller;
        BendRollerId = bendRoller.Id;
        Clamp = clamp;
        ClampId = Clamp.Id;
        ClampRoller = clampRoller;
        ClampRollerId = ClampRoller.Id;
        Console = console;
        ConsoleId = Console.Id;
        Dorn = dorn;
        DornId = Dorn.Id;
        Lift = lift;
        LiftId = Lift.Id;
        Press = press;
        PressId = Press.Id;
        Rotation = rotation;
        RotationId = Rotation.Id;
        Squeeze = squeeze;
        SqueezeId = Squeeze.Id;
        Supply = supply;
        SupplyId = Supply.Id;
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
            other.IsEnabled == IsEnabled &&
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
            IsEnabled,
            ForwardDangerZoneCoordinate,
            GetHashCodeFromBToD(),
            GetHashCodeFromLToS());

    private int GetHashCodeFromBToD() =>
        HashCode.Combine(
            DistanceFromCenter.GetHashCode(),
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
            IsEnabled,
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