using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models;

public class Settings
{
    [Key]
    public Guid Id { get; set; }

    public string Server { get; set; } = "192.168.1.3";

    public bool DornAutomatic { get; set; }
    public float DornLeadWithdrawalBeforeBend { get; set; }
    public bool DornLubricantTurnOn { get; set; }
    public bool BendSynchronization { get; set; }
    public float BendSynchronizationCoefficient { get; set; } = 1;
    public bool BendAndSupplySynchronization { get; set; }
    public bool InterceptionMode { get; set; }
    public bool ConsoleOutletForPipeInstalling { get; set; }
    public float PipeOutletCoordinate { get; set; } = 3000;
    public bool MultiLeveled { get; set; }
    public bool WithPunchingCylinder { get; set; }
    public float DistanceFromBendingToPunching { get; set; } = 40;
    public bool IsElectricBendingDrive { get; set; }
    public bool AbsoluteUnitCoordinate { get; set; }
    public float SupportFirstLiftBan { get; set; } = 2000.1f;
    public float SupportSecondLiftBan { get; set; }
    public float SupportThirdLiftBanRear { get; set; }
    public float SupportThirdLiftBanFront { get; set; }
    public float SupportFourthLiftBan { get; set; }
    public bool BanPressWhenSupportIsLifted { get; set; }
    public float SqueezeWorkTime { get; set; } = 0.5f;
    public float SupplyStartRollingSpeed { get; set; } = 99.0f;
    public bool IncompletePressMovement { get; set; }
    public bool HydraulicMovementWithoutSensors { get; set; }
    public bool ShowButtonFullAutomatic { get; set; }
    public bool InvertClampSensors { get; set; }
    public float SupplyCoefficient { get; set; } = 4.65f;
    public float RotationCoefficient { get; set; } = 1.6f;
    public float ConsoleCoefficient { get; set; } = 2;
    public float BendCoefficient { get; set; } = 2;
    public float SupplyAcceleration { get; set; } = 2000;
    public float RotationAcceleration { get; set; } = 2000;
    public float ConsoleAcceleration { get; set; } = 2000;
    public float BendAcceleration { get; set; } = 2000;
    public float SupplyBraking { get; set; } = 2000;
    public float RotationBraking { get; set; } = 2000;
    public float ConsoleBraking { get; set; } = 2000;
    public float BendBraking { get; set; } = 2000;
    public float SupplyJerk { get; set; } = 10000;
    public float RotationJerk { get; set; } = 10000;
    public float ConsoleJerk { get; set; } = 10000;
    public float BendJerk { get; set; } = 10000;

    public Settings() { }
}