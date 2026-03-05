using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models;

public class Settings
{
    [Key]
    public Guid Id { get; set; }

    public bool DornAutomatic { get; set; }
    public double DornLeadWithdrawalBeforeBend { get; set; }
    public bool DornLubricantTurnOn { get; set; }
    public bool BendSynchronization { get; set; }
    public double BendSynchronizationCoefficient { get; set; } = 1;
    public bool BendAndSupplySynchronization { get; set; }
    public bool InterceptionMode { get; set; }
    public bool ConsoleOutletForPipeInstalling { get; set; }
    public double PipeOutletCoordinate { get; set; } = 3000;
    public bool MultiLeveled { get; set; }
    public bool WithPunchingCylinder { get; set; }
    public double DistanceFromBendingToPunching { get; set; } = 40;
    public bool IsElectricBendingDrive { get; set; }
    public bool AbsoluteUnitCoordinate { get; set; }
    public double SupportFirstLiftBan { get; set; } = 2000.1;
    public double SupportSecondLiftBan { get; set; }
    public double SupportThirdLiftBanRear { get; set; }
    public double SupportThirdLiftBanFront { get; set; }
    public double SupportFourthLiftBan { get; set; }
    public bool BanPressWhenSupportIsLifted { get; set; }
    public double SqueezeWorkTime { get; set; } = 0.5;
    public double SupplyStartRollingSpeed { get; set; } = 99.0;
    public bool IncompleteClampMovement { get; set; }
    public bool HydraulicMovementWithoutSensors { get; set; }
    public bool ShowButtonFullAutomatic { get; set; }
    public bool InvertClampSensors { get; set; }
    public double SupplyCoefficient { get; set; } = 4.65;
    public double RotationCoefficient { get; set; } = 1.6;
    public double ConsoleCoefficient { get; set; } = 2;
    public double BendCoefficient { get; set; } = 2;
    public double SupplyAcceleration { get; set; } = 2000;
    public double RotationAcceleration { get; set; } = 2000;
    public double ConsoleAcceleration { get; set; } = 2000;
    public double BendAcceleration { get; set; } = 2000;
    public double SupplyBraking { get; set; } = 2000;
    public double RotationBraking { get; set; } = 2000;
    public double ConsoleBraking { get; set; } = 2000;
    public double BendBraking { get; set; } = 2000;
    public double SupplyJerk { get; set; } = 10000;
    public double RotationJerk { get; set; } = 10000;
    public double ConsoleJerk { get; set; } = 10000;
    public double BendJerk { get; set; } = 10000;

    public Settings() { }
}