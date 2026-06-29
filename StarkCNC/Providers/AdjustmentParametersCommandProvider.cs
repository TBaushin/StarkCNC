using StarkCNC.Core.Models;
using StarkCNC.Core.Providers;
using StarkCNC.Utilities;

namespace StarkCNC.Providers;

public class AdjustmentParametersCommandProvider : IMachineCommandProvider<AdjustmentParameters>
{
    public IEnumerable<MachineCommand> GetCommands(AdjustmentParameters model)
    {
        if (model is null || model.InstalledLevel <= 0 || !model.IsEnabled)
            yield break;

        var level = model.InstalledLevel;

        yield return new(model.PipeDiameter, ControllerRequestStrings.GET_ADJUSTMENT_PIPE_DIAMETER(level));
        yield return new(model.Radius, ControllerRequestStrings.GET_ADJUSTMENT_RADIUS(level));
        yield return new((int)model.Type, ControllerRequestStrings.GET_ADJUSTMENT_TYPE(level));
        yield return new(model.ForwardDangerZoneCoordinate, ControllerRequestStrings.GET_ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE(level));
        yield return new(model.DistanceFromCenter, ControllerRequestStrings.GET_ADJUSTMENT_DISTANCE_FROM_CENTER(level));

        // Bend
        yield return new(model.Bend.ForwardPositionLimitation, ControllerRequestStrings.GET_BEND_FORWARD_POSITION_LIMITATION(level));
        yield return new(model.Bend.SpeedCoefficient, ControllerRequestStrings.GET_BEND_SPEED_COEFFICIENT(level));
        yield return new(model.Bend.DeflectionDuringClampClamping, ControllerRequestStrings.GET_BEND_DEFLECTION_DURING_CLAMP_CLAMPING(level));
        yield return new(model.Bend.Deflection, ControllerRequestStrings.GET_BEND_DEFLECTION(level));

        // BendRoller
        yield return new(model.BendRoller.Radius, ControllerRequestStrings.GET_BEND_ROLLER_RADIUS(level));
        yield return new(model.BendRoller.OuterRadius, ControllerRequestStrings.GET_BEND_ROLLER_OUTER_RADIUS(level));

        // Clamp
        yield return new(model.Clamp.Deep, ControllerRequestStrings.GET_CLAMP_DEEP(level));
        yield return new(model.Clamp.Length, ControllerRequestStrings.GET_CLAMP_LENGTH(level));
        yield return new(model.Clamp.ForwardPosition, ControllerRequestStrings.GET_CLAMP_FORWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Clamp.MiddlePosition, ControllerRequestStrings.GET_CLAMP_MIDDLE_POSITION(level)); // TODO: Проверить
        yield return new(model.Clamp.BackwardPosition, ControllerRequestStrings.GET_CLAMP_BACKWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Clamp.SpeedCoefficient, ControllerRequestStrings.GET_CLAMP_SPEED_COEFFICIENT(level));

        // ClampRoller
        yield return new(model.ClampRoller.OuterRadius, ControllerRequestStrings.GET_CLAMP_ROLLER_OUTER_RADIUS(level));
        yield return new(model.ClampRoller.InnerRadius, ControllerRequestStrings.GET_CLAMP_ROLLER_INNTER_RADIUS(level));

        // Console
        yield return new(model.Console.BendPosition, ControllerRequestStrings.GET_CONSOLE_BEND_POSITION(level));
        yield return new(model.Console.SecondFloorPosition, ControllerRequestStrings.GET_CONSOLE_SECOND_FLOOR_POSITION(level));
        yield return new(model.Console.SecondFloorIntermediatePosition, ControllerRequestStrings.GET_CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION(level));
        yield return new(model.Console.ThirdFloorPosition, ControllerRequestStrings.GET_CONSOLE_THIRD_FLOOR_POSITION(level));
        yield return new(model.Console.PipeRotationDepartureDistance, ControllerRequestStrings.GET_CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE(level));

        // Dorn
        yield return new(model.Dorn.ForwardPosition, ControllerRequestStrings.GET_DORN_FORWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Dorn.MiddlePosition, ControllerRequestStrings.GET_DORN_MIDDLE_POSITION(level)); // TODO: Проверить
        yield return new(model.Dorn.BackwardPosition, ControllerRequestStrings.GET_DORN_BACKWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Dorn.SpeedCoefficient, ControllerRequestStrings.GET_DORN_SPEED_COEFFICIENT(level));

        // Lift
        yield return new(model.Lift.UpperPosition, ControllerRequestStrings.GET_LIFT_UPPER_POSITION(level)); // TODO: Проверить
        yield return new(model.Lift.MiddlePosition, ControllerRequestStrings.GET_LIFT_MIDDLE_POSITION(level)); // TODO: Проверить
        yield return new(model.Lift.LowerPosition, ControllerRequestStrings.GET_LIFT_LOWER_POSITION(level)); // TODO: Проверить
        yield return new(model.Lift.SpeedCoefficient, ControllerRequestStrings.GET_LIFT_SPEED_COEFFICIENT(level));

        // Press
        yield return new(model.Press.DangerZoneCoordinate, ControllerRequestStrings.GET_PRESS_DANGER_ZONE_COORDINATE(level));
        yield return new(model.Press.Length, ControllerRequestStrings.GET_PRESS_LENGTH(level));
        yield return new(model.Press.ForwardPosition, ControllerRequestStrings.GET_PRESS_FORWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Press.MiddlePosition, ControllerRequestStrings.GET_PRESS_MIDDLE_POSITION(level)); // TODO: Проверить
        yield return new(model.Press.BackwardPosition, ControllerRequestStrings.GET_PRESS_BACKWARD_POSITION(level)); // TODO: Проверить
        yield return new(model.Press.SpeedCoefficient, ControllerRequestStrings.GET_PRESS_SPEED_COEFFICIENT(level));

        // Rotation
        yield return new(model.Rotation.OffsetAfterZeroSearch, ControllerRequestStrings.GET_ROTATION_OFFSET_AFTER_ZERO_SEARCH(level));

        // Squeeze
        yield return new(model.Squeeze.TurnOn, ControllerRequestStrings.GET_SQUEEZE_TURN_ON(level));
        yield return new(model.Squeeze.FrontPositionLimitation, ControllerRequestStrings.GET_SQUEEZE_FRONT_POSITION_LIMITATION(level));
        yield return new(model.Squeeze.SpeedCoefficient, ControllerRequestStrings.GET_SQUEEZE_SPEED_COEFFICIENT(level));

        // Supply
        yield return new(model.Supply.PressZonePosition, ControllerRequestStrings.GET_SUPPLY_PRESS_ZONE_POSITION(level));
        yield return new(model.Supply.ForwardDangerZonePosition, ControllerRequestStrings.GET_SUPPLY_FORWARD_DANGER_ZONE(level));
        yield return new(model.Supply.ColletJawsDepth, ControllerRequestStrings.GET_SUPPLY_COLLET_JAWS_DEPTH(level));
    }
}
