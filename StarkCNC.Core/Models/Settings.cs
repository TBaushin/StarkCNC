using StarkCNC.Core.Models.SettingsParameters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarkCNC.Core.Models;

public class Settings : ICloneable
{
    [Key]
    public Guid Id { get; set; }

    public FloorType FloorType { get; set; }

    public bool IsElectricBendingDrive { get; set; }

    public bool IsPunchingCylinder { get; set; }

    public bool IsElectricMachine { get; set; }

    public double Speed { get; set; }

    public double SynchronizationCoefficient { get; set; }
    
    public bool InterceptionMode { get; set; }

    public Guid BendId { get; set; }

    [ForeignKey(nameof(BendId))]
    public Bend Bend { get; set; }

    public Guid DornId { get; set; }

    [ForeignKey(nameof(DornId))]
    public Dorn Dorn { get; set; }

    public Guid RotationId { get; set; }

    [ForeignKey(nameof(RotationId))]
    public Rotation Rotation { get; set; }

    public Guid SupportId { get; set; }

    [ForeignKey(nameof(SupportId))]
    public Support Support { get; set; }

    public Guid SupplyId { get; set; }

    [ForeignKey(nameof(SupplyId))]
    public Supply Supply { get; set; }

    public Guid ConsoleId { get; set; }

    [ForeignKey(nameof(ConsoleId))]
    public StarkCNC.Core.Models.SettingsParameters.Console Console { get; set; }

    public Guid PipeId { get; set; }

    [ForeignKey(nameof(PipeId))]
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
        BendId = Bend.Id;
        Dorn = dorn;
        DornId = Dorn.Id;
        Rotation = rotation;
        RotationId = Rotation.Id;
        Support = support;
        SupportId = support.Id;
        Supply = supply;
        SupplyId = Supply.Id;
        Console = console;
        ConsoleId = Console.Id;
        Pipe = pipe;
        PipeId = Pipe.Id;
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