using StarkCNC.Core.Models.SettingsParameters;

namespace StarkCNC.Core.Models;

public class Settings : ICloneable
{
    public Guid Id { get; set; }

    public FloorType FloorType { get; set; }

    public bool IsElectricBendingDrive { get; set; }

    public bool IsPunchingCylinder { get; set; }

    public bool IsElectricMachine { get; set; }

    public double Speed { get; set; }

    public double SynchronizationCoefficient { get; set; }
    
    public bool InterceptionMode { get; set; }

    public Guid BendId { get; set; }
    public Bend Bend { get; set; }

    public Guid DornId { get; set; }
    public Dorn Dorn { get; set; }

    public Guid RotationId { get; set; }
    public Rotation Rotation { get; set; }

    public Guid SupportId { get; set; }
    public Support Support { get; set; }

    public Guid SupplyId { get; set; }
    public Supply Supply { get; set; }

    public Guid ConsoleId { get; set; }
    public StarkCNC.Core.Models.SettingsParameters.Console Console { get; set; }

    public Guid PipeId { get; set; }
    public Pipe Pipe { get; set; }

    public Settings() { }

    public Settings(
        Guid id,
        FloorType floorType,
        bool isElectricBendingDrive,
        bool isPunchingCylinder,
        bool isElectricMachine,
        double speed,
        double synchronizationCoefficient,
        bool interceptionMode,
        Bend bend,
        Dorn dorn,
        Rotation rotation,
        Support support,
        Supply supply,
        StarkCNC.Core.Models.SettingsParameters.Console console,
        Pipe pipe)
    {
        Id = id;
        FloorType = floorType;
        IsElectricBendingDrive = isElectricBendingDrive;
        IsPunchingCylinder = isPunchingCylinder;
        IsElectricMachine = isElectricMachine;
        Speed = speed;
        SynchronizationCoefficient = synchronizationCoefficient;
        InterceptionMode = interceptionMode;
        Bend = bend;
        Dorn = dorn;
        Rotation = rotation;
        Support = support;
        Supply = supply;
        Console = console;
        Pipe = pipe;
    }

    public object Clone() =>
        new Settings(
            Id,
            FloorType,
            IsElectricBendingDrive,
            IsPunchingCylinder,
            IsElectricMachine,
            Speed,
            SynchronizationCoefficient,
            InterceptionMode,
            (Bend)Bend.Clone(),
            (Dorn)Dorn.Clone(),
            (Rotation)Rotation.Clone(),
            (Support)Support.Clone(),
            (Supply)Supply.Clone(),
            (StarkCNC.Core.Models.SettingsParameters.Console)Console.Clone(),
            (Pipe)Pipe.Clone());
}