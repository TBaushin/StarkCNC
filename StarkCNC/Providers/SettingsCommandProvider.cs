using StarkCNC.Core.Models;
using StarkCNC.Core.Providers;
using StarkCNC.Utilities;

namespace StarkCNC.Providers;

public class SettingsCommandProvider : IMachineCommandProvider<Settings>
{
    public IEnumerable<MachineCommand> GetCommands(Settings model)
    {
        if (model is null)
            yield break;

        yield return new(model.DornAutomatic, ControllerRequestStrings.DORN_AUTOMATIC);
        yield return new(model.DornLeadWithdrawalBeforeBend, ControllerRequestStrings.DORN_LEAD_WITHDRAWAL_BEFORE_BEND);
        yield return new(model.DornLubricantTurnOn, ControllerRequestStrings.DORN_LUBRICANT_LUBRICANT_TURN_ON);
        yield return new(model.BendSynchronization, ControllerRequestStrings.BEND_AND_SUPPLY_PUSHING_ENABLE);
        yield return new(model.BendSynchronizationCoefficient, ControllerRequestStrings.BEND_AND_SUPPLY_COEFFICIENT);
        yield return new(model.BendAndSupplySynchronization, ControllerRequestStrings.BEND_AND_SUPPLY_SYNCHRONIZATION);
        yield return new(model.InterceptionMode, ControllerRequestStrings.SETTINGS_INTERCEPTION_MODE);
        yield return new(model.ConsoleOutletForPipeInstalling, ControllerRequestStrings.CONSOLE_OUTLET_FOR_PIPE_INSTALLING);
        yield return new(model.PipeOutletCoordinate, ControllerRequestStrings.PIPE_OUTLET_COORDINATE);
        yield return new(model.WithPunchingCylinder, ControllerRequestStrings.SETTINGS_WITH_PUNCHING_CYLINDER);
        yield return new(model.DistanceFromBendingToPunching, ControllerRequestStrings.SETTINGS_DISTANCE_FROM_BENDING_TO_PUNCHING);
        yield return new(model.SupportFirstLiftBan, ControllerRequestStrings.SUPPORT_FIRST_COORDINATE_BAN);
        yield return new(model.SupportSecondLiftBan, ControllerRequestStrings.SUPPORT_SECOND_COORDINATE_BAN);
        yield return new(model.SupportThirdLiftBanRear, ControllerRequestStrings.SUPPORT_THIRD_BACKWARD_COORDINATE_BAN);
        yield return new(model.SupportThirdLiftBanFront, ControllerRequestStrings.SUPPORT_THIRD_FORWARD_COORDINATE_BAN);
        yield return new(model.SupportFourthLiftBan, ControllerRequestStrings.SUPPORT_THIRD_FORWARD_COORDINATE_BAN);
        yield return new(model.BanPressWhenSupportIsLifted, ControllerRequestStrings.SUPPORT_BAN_PRESS);
        yield return new(model.SqueezeWorkTime, ControllerRequestStrings.SQUEEZE_WORK_TIME);
        yield return new(model.SupplyStartRollingSpeed, ControllerRequestStrings.SUPPLY_START_ROLLING_SPEED);
        yield return new(model.IncompletePressMovement, ControllerRequestStrings.PRESS_INCOMPLETE_MOVEMENT);
        yield return new(model.HydraulicMovementWithoutSensors, ControllerRequestStrings.HYDRAULICS_MOVEMENT_WITHOUT_SENSORS);
        yield return new(model.InvertClampSensors, ControllerRequestStrings.CLAMP_INVERT_SENSORS);
        yield return new(model.SupplyCoefficient, ControllerRequestStrings.SUPPLY_COEFFICIENT);
        yield return new(model.RotationCoefficient, ControllerRequestStrings.ROTATION_COEFFICIENT);
        yield return new(model.ConsoleCoefficient, ControllerRequestStrings.CONSOLE_COEFFICIENT);
        yield return new(model.BendCoefficient, ControllerRequestStrings.BEND_COEFFICIENT);
        yield return new(model.SupplyAcceleration, ControllerRequestStrings.SUPPLY_ACCELERATION);
        yield return new(model.RotationAcceleration, ControllerRequestStrings.ROTATION_ACCELERATION);
        yield return new(model.ConsoleAcceleration, ControllerRequestStrings.CONSOLE_ACCELERATION);
        yield return new(model.BendAcceleration, ControllerRequestStrings.BEND_ACCELERATION);
        yield return new(model.SupplyBraking, ControllerRequestStrings.SUPPLY_BRAKING);
        yield return new(model.RotationBraking, ControllerRequestStrings.ROTATION_BRAKING);
        yield return new(model.ConsoleBraking, ControllerRequestStrings.CONSOLE_BRAKING);
        yield return new(model.BendBraking, ControllerRequestStrings.BEND_BRAKING);
        yield return new(model.SupplyJerk, ControllerRequestStrings.SUPPLY_JERK);
        yield return new(model.RotationJerk, ControllerRequestStrings.ROTATION_JERK);
        yield return new(model.ConsoleJerk, ControllerRequestStrings.CONSOLE_JERK);
        yield return new(model.BendJerk, ControllerRequestStrings.BEND_JERK);
    }
}
