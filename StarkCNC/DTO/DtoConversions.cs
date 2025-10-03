using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Adjustment;
using StarkCNC.DTO.Adjustment;

namespace StarkCNC.DTO;

internal static class DtoConversions
{
    public static AdjustmentParametersDto? ToDto(this AdjustmentParameters adjustment, IConfiguration configuration)
    {
        if (adjustment is null)
            throw new ArgumentNullException(nameof(adjustment));

        var adjustmentDto = AdjustmentParametersDto.CreateFromConfiguration(configuration);
        if (adjustmentDto is null)
            throw new ArgumentException(nameof(adjustmentDto));

        adjustmentDto.Id = adjustment.Id;
        adjustmentDto.Name = adjustment.Name;
        adjustmentDto.PipeDiameter = adjustment.PipeDiameter;
        adjustmentDto.Radius = adjustment.Radius;
        adjustmentDto.Type = adjustment.Type;
        adjustmentDto.InstalledLevel = adjustment.InstalledLevel;
        adjustmentDto.ForwardDangerZoneCoordinate = adjustment.ForwardDangerZoneCoordinate;
        adjustmentDto.DistanceFromCenter = adjustment.DistanceFromCenter;
        adjustmentDto.Bend = adjustment.Bend.ToDto(configuration);
        adjustmentDto.BendRoller = adjustment.BendRoller.ToDto(configuration);
        adjustmentDto.Clamp = adjustment.Clamp.ToDto(configuration);
        adjustmentDto.ClampRoller = adjustment.ClampRoller.ToDto(configuration);
        adjustmentDto.Console = adjustment.Console.ToDto(configuration);
        adjustmentDto.Dorn = adjustment.Dorn.ToDto(configuration);
        adjustmentDto.Lift = adjustment.Lift.ToDto(configuration);
        adjustmentDto.Press = adjustment.Press.ToDto(configuration);
        adjustmentDto.Rotation = adjustment.Rotation.ToDto(configuration);
        adjustmentDto.Squeeze = adjustment.Squeeze.ToDto(configuration);
        adjustmentDto.Supply = adjustment.Supply.ToDto(configuration);
        return adjustmentDto;
    }

    public static BendDto? ToDto(this Bend bend, IConfiguration configuration)
    {
        if (bend is null)
            throw new ArgumentNullException(nameof(bend));

        var bendDto = BendDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (bendDto is null)
            return bendDto;

        bendDto.ForwardPositionLimitation = bend.ForwardPositionLimitation;
        bendDto.SpeedCoefficient = bend.SpeedCoefficient;
        bendDto.SlowdownSpeed = bend.SlowdownSpeed;
        return bendDto;
    }

    public static BendRollerDto? ToDto(this BendRoller bendRoller, IConfiguration configuration)
    {
        if (bendRoller is null)
            throw new ArgumentNullException(nameof(bendRoller));

        var bendRollerDto = BendRollerDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (bendRollerDto is null)
            return bendRollerDto;

        bendRollerDto.Radius = bendRoller.Radius;
        bendRollerDto.OuterRadius = bendRoller.OuterRadius;
        return bendRollerDto;
    }

    public static ClampDto? ToDto(this Clamp clamp, IConfiguration configuration)
    {
        if (clamp is null)
            throw new ArgumentNullException(nameof(clamp));

        var clampDto = ClampDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (clampDto is null)
            return clampDto;

        clampDto.Deep = clamp.Deep;
        clampDto.Length = clamp.Length;
        clampDto.ForwardPosition = clamp.ForwardPosition;
        clampDto.MiddlePosition = clamp.MiddlePosition;
        clampDto.BackwardPosition = clamp.BackwardPosition;
        clampDto.SpeedCoefficient = clamp.SpeedCoefficient;
        return clampDto;
    }

    public static ClampRollerDto? ToDto(this ClampRoller clamp, IConfiguration configuration)
    {
        if (clamp is null)
            throw new ArgumentNullException(nameof(clamp));

        var clampRollerDto = ClampRollerDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (clampRollerDto is null)
            return clampRollerDto;

        clampRollerDto.OuterRadius = clamp.OuterRadius;
        clampRollerDto.InnerRadius = clamp.InnerRadius;
        return clampRollerDto;
    }

    public static ConsoleDto? ToDto(this StarkCNC.Core.Models.Adjustment.Console console, IConfiguration configuration)
    {
        if (console is null)
            throw new ArgumentNullException(nameof(console));

        var consoleDto = ConsoleDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (consoleDto is null)
            return consoleDto;

        consoleDto.BendPosition = console.BendPosition;
        consoleDto.SecondFloorPosition = console.SecondFloorPosition;
        consoleDto.SecondFloorIntermediatePosition = console.SecondFloorIntermediatePosition;
        consoleDto.ThirdFloorPosition = console.ThirdFloorPosition;
        consoleDto.PipeRotationDepartureDistance = console.PipeRotationDepartureDistance;
        consoleDto.SpeedCoefficient = console.SpeedCoefficient;
        return consoleDto;
    }

    public static DornDto? ToDto(this Dorn dorn, IConfiguration configuration)
    {
        if (dorn is null)
            throw new ArgumentNullException(nameof(dorn));

        var dornDto = DornDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (dornDto is null)
            return dornDto;

        dornDto.ForwardPosition = dorn.ForwardPosition;
        dornDto.MiddlePosition = dorn.MiddlePosition;
        dornDto.BackwardPosition = dorn.BackwardPosition;
        dornDto.SpeedCoefficient = dorn.SpeedCoefficient;
        return dornDto;
    }

    public static LiftDto? ToDto(this Lift lift, IConfiguration configuration)
    {
        if (lift is null)
            throw new ArgumentNullException(nameof(lift));

        var liftDto = LiftDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (liftDto is null)
            return liftDto;

        liftDto.UpperPosition = lift.UpperPosition;
        liftDto.MiddlePosition = lift.MiddlePosition;
        lift.LowerPosition = lift.LowerPosition;
        liftDto.SpeedCoefficient = lift.SpeedCoefficient;
        return liftDto;
    }

    public static PressDto? ToDto(this Press press, IConfiguration configuration)
    {
        if (press is null)
            throw new ArgumentNullException(nameof(press));

        var pressDto = PressDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (pressDto is null)
            return pressDto;

        pressDto.DangerZoneCoordinate = press.DangerZoneCoordinate;
        pressDto.Length = press.Length;
        pressDto.ForwardPosition = press.ForwardPosition;
        pressDto.MiddlePosition = press.MiddlePosition;
        pressDto.BackwardPosition = press.BackwardPosition;
        pressDto.SpeedCoefficient = press.SpeedCoefficient;
        return pressDto;
    }

    public static RotationDto? ToDto(this Rotation rotation, IConfiguration configuration)
    {
        if (rotation is null)
            throw new ArgumentNullException(nameof(rotation));

        var rotationDto = RotationDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (rotationDto is null)
            return rotationDto;

        rotationDto.OffsetAfterZeroSearch = rotation.OffsetAfterZeroSearch;
        rotationDto.SpeedCoefficient = rotation.SpeedCoefficient;
        return rotationDto;
    }

    public static SqueezeDto? ToDto(this Squeeze squeeze, IConfiguration configuration)
    {
        if (squeeze is null)
            throw new ArgumentNullException(nameof(squeeze));

        var squeezeDto = SqueezeDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (squeezeDto is null)
            return squeezeDto;

        squeezeDto.TurnOn = squeeze.TurnOn;
        squeezeDto.FrontPositionLimitation = squeeze.FrontPositionLimitation;
        squeezeDto.SpeedCoefficient = squeeze.SpeedCoefficient;
        return squeezeDto;
    }

    public static SupplyDto? ToDto(this Supply supply, IConfiguration configuration)
    {
        if (supply is null)
            throw new ArgumentNullException(nameof(supply));

        var supplyDto = SupplyDto.CreateFromConfiguration(configuration.GetSection("Adjustment"));
        if (supplyDto is null)
            return supplyDto;

        supplyDto.PressZonePosition = supply.PressZonePosition;
        supplyDto.ForwardDangerZonePosition = supply.ForwardDangerZonePosition;
        supplyDto.ColletJawsDepth = supply.ColletJawsDepth;
        supplyDto.SpeedCoefficient = supply.SpeedCoefficient;
        return supplyDto;
    }
}
