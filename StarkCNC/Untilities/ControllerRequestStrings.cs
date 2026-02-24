using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace StarkCNC.Utilities;

[SuppressMessage("Usage", "CA1707", Justification = "—ÔÂˆË‡Î¸ÌÓ Ò‰ÂÎ‡ÌÓ, ˜ÚÓ· ‚˚‰ÂÎËÚ¸")]
public static class ControllerRequestStrings
{
    public static readonly string REQUEST = App.Configuration.GetValue<string>("MachineController:RequestString") ?? string.Empty;

    public static readonly string MANUALMODE = App.Configuration.GetValue<string>("MachineController:ManualModeRequestString") ?? string.Empty;
    
    // œŒƒ¿◊¿
    public static readonly string SUPPLY_FORWARD = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:ForwardRequestString") ?? string.Empty;
    public static readonly string SUPPLY_BACKWARD = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:BackwardRequestString") ?? string.Empty;
    public static readonly string SUPPLY_ACTUAL_COORDINATE = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:ActualCoordinateRequestString") ?? string.Empty;
    public static readonly string SUPPLY_ACTUAL_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:ActualRelativeDisplacementRequestString") ?? string.Empty;
    public static readonly string SUPPLY_RESET = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:ResetRequestString") ?? string.Empty;
    public static readonly string SUPPLY_SPEED = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:SpeedRequestString") ?? string.Empty;
    public static readonly string SUPPLY_TORQUE = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:TorqueRequestString") ?? string.Empty;
    public static readonly string SUPPLY_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:RearPositionRequestString") ?? string.Empty;
    public static readonly string SUPPLY_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:FrontPositionRequestString") ?? string.Empty;
    public static readonly string SUPPLY_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:FeedDrive:RelativeDispositionRequestString") ?? string.Empty;
    public static readonly string SUPPLY_RESETED_OFFSET = App.Configuration.GetValue<string>("Settings:Supply:ResetedOffset:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_START_ROLLING_SPEED = App.Configuration.GetValue<string>("Settings:Supply:StartRollingSpeed:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_COEFFICIENT = App.Configuration.GetValue<string>("Settings::Supply:Coefficient:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:PressZonePosition:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:PressZonePosition:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:PressZonePosition:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_PRESS_ZONE_POSITION(int level)
    {
        return level switch
        {
            1 => SUPPLY_PRESS_ZONE_POSITION_L1,
            2 => SUPPLY_PRESS_ZONE_POSITION_L2,
            3 => SUPPLY_PRESS_ZONE_POSITION_L3,
            _ => SUPPLY_PRESS_ZONE_POSITION_L1
        };
    }
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:ForwardDangerZone:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:ForwardDangerZone:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:ForwardDangerZone:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_FORWARD_DANGER_ZONE(int level)
    {
        return level switch
        {
            1 => SUPPLY_FORWARD_DANGER_ZONE_L1,
            2 => SUPPLY_FORWARD_DANGER_ZONE_L2,
            3 => SUPPLY_FORWARD_DANGER_ZONE_L3,
            _ => SUPPLY_FORWARD_DANGER_ZONE_L1
        };
    }
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:ColletJawsDepth:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:ColletJawsDepth:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:ColletJawsDepth:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_COLLET_JAWS_DEPTH(int level)
    {
        return level switch
        {
            1 => SUPPLY_COLLET_JAWS_DEPTH_L1,
            2 => SUPPLY_COLLET_JAWS_DEPTH_L2,
            3 => SUPPLY_COLLET_JAWS_DEPTH_L3,
            _ => SUPPLY_COLLET_JAWS_DEPTH_L1
        };
    }
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => SUPPLY_SPEED_COEFFICIENT_L1,
            2 => SUPPLY_SPEED_COEFFICIENT_L2,
            3 => SUPPLY_SPEED_COEFFICIENT_L3,
            _ => SUPPLY_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string SUPPLY_VALUE = App.Configuration.GetValue<string>("Program:Supply:Value:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_FACTICAL_POSITION = App.Configuration.GetValue<string>("AutomaticTags:Factical:Supply:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => SUPPLY_CURRENT_POSITION_L1,
            2 => SUPPLY_CURRENT_POSITION_L2,
            3 => SUPPLY_CURRENT_POSITION_L3,
            _ => SUPPLY_CURRENT_POSITION_L1
        };
    }
    public static readonly string SUPPLY_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:Forward:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:Forward:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:Forward:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_FORWARD(int level)
    {
        return level switch
        {
            1 => SUPPLY_FORWARD_L1,
            2 => SUPPLY_FORWARD_L2,
            3 => SUPPLY_FORWARD_L3,
            _ => SUPPLY_FORWARD_L1
        };
    }
    public static readonly string SUPPLY_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:Backward:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:Backward:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:Backward:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_BACKWARD(int level)
    {
        return level switch
        {
            1 => SUPPLY_BACKWARD_L1,
            2 => SUPPLY_BACKWARD_L2,
            3 => SUPPLY_BACKWARD_L3,
            _ => SUPPLY_BACKWARD_L1
        };
    }
    public static readonly string SUPPLY_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Supply:Reset:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_RESET_L2 = App.Configuration.GetValue<string>("Adjustment2:Supply:Reset:RequestString") ?? string.Empty;
    public static readonly string SUPPLY_RESET_L3 = App.Configuration.GetValue<string>("Adjustment3:Supply:Reset:RequestString") ?? string.Empty;
    public static string GET_SUPPLY_RESET(int level)
    {
        return level switch
        {
            1 => SUPPLY_RESET_L1,
            2 => SUPPLY_RESET_L2,
            3 => SUPPLY_RESET_L3,
            _ => SUPPLY_RESET_L1
        };
    }

    public static readonly string ROTATION_OFFSET = App.Configuration.GetValue<string>("Settings:Rotation:Offset:RequestString") ?? string.Empty;
    public static readonly string ROTATION_COEFFICIENT = App.Configuration.GetValue<string>("Settings:Rotation:Coefficient:RequestString") ?? string.Empty;
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L1 = App.Configuration.GetValue<string>("Adjustment:Rotation:OffsetAfterZeroSearch:RequestString") ?? string.Empty;
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L2 = App.Configuration.GetValue<string>("Adjustment2:Rotation:OffsetAfterZeroSearch:RequestString") ?? string.Empty;
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L3 = App.Configuration.GetValue<string>("Adjustment3:Rotation:OffsetAfterZeroSearch:RequestString") ?? string.Empty;
    public static string GET_ROTATION_OFFSET_AFTER_ZERO_SEARCH(int level)
    {
        return level switch
        {
            1 => ROTATION_OFFSET_AFTER_ZERO_SEARCH_L1,
            2 => ROTATION_OFFSET_AFTER_ZERO_SEARCH_L2,
            3 => ROTATION_OFFSET_AFTER_ZERO_SEARCH_L3,
            _ => ROTATION_OFFSET_AFTER_ZERO_SEARCH_L1
        };
    }
    public static readonly string ROTATION_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Rotation:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string ROTATION_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Rotation:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string ROTATION_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Rotation:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_ROTATION_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => ROTATION_SPEED_COEFFICIENT_L1,
            2 => ROTATION_SPEED_COEFFICIENT_L2,
            3 => ROTATION_SPEED_COEFFICIENT_L3,
            _ => ROTATION_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string ROTATION_VALUE = App.Configuration.GetValue<string>("Program:Rotation:Value:RequestString") ?? string.Empty;
    public static readonly string ROTATION_FACTICAL_POSITION = App.Configuration.GetValue<string>("AutomaticTags:Factical:Rotation:RequestString") ?? string.Empty;

    //  ŒÕ—ŒÀ‹
    public static readonly string CONSOLE_FORWARD = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:ForwardRequestString") ?? string.Empty;
    public static readonly string CONSOLE_BACKWARD = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:BackwardRequestString") ?? string.Empty;
    public static readonly string CONSOLE_ACTUAL_COORDINATE = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:ActualCoordinateRequestString") ?? string.Empty;
    public static readonly string CONSOLE_ACTUAL_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:ActualRelativeDisplacementRequestString") ?? string.Empty;
    public static readonly string CONSOLE_RESET = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:ResetRequestString") ?? string.Empty;
    public static readonly string CONSOLE_SPEED = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:SpeedRequestString") ?? string.Empty;
    public static readonly string CONSOLE_TORQUE = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:TorqueRequestString") ?? string.Empty;
    public static readonly string CONSOLE_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:RearPositionRequestString") ?? string.Empty;
    public static readonly string CONSOLE_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:FrontPositionRequestString") ?? string.Empty;
    public static readonly string CONSOLE_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:ConsoleDrive:RelativeDisplacementRequestString") ?? string.Empty;
    public static readonly string CONSOLE_COEFFICIENT = App.Configuration.GetValue<string>("Settings:Console:Coefficient:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_BEND_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Console:BendPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_BEND_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:BendPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_BEND_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:BendPosition:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_BEND_POSITION(int level)
    {
        return level switch
        {
            1 => CONSOLE_BEND_POSITION_L1,
            2 => CONSOLE_BEND_POSITION_L2,
            3 => CONSOLE_BEND_POSITION_L3,
            _ => CONSOLE_BEND_POSITION_L1
        };
    }
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Console:SecondFloorPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:SecondFloorPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:SecondFloorPosition:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_SECOND_FLOOR_POSITION(int level)
    {
        return level switch
        {
            1 => CONSOLE_SECOND_FLOOR_POSITION_L1,
            2 => CONSOLE_SECOND_FLOOR_POSITION_L2,
            3 => CONSOLE_SECOND_FLOOR_POSITION_L3,
            _ => CONSOLE_SECOND_FLOOR_POSITION_L1
        };
    }
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Console:SecondFloorIntermediatePosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:SecondFloorIntermediatePosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:SecondFloorIntermediatePosition:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION(int level)
    {
        return level switch
        {
            1 => CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L1,
            2 => CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L2,
            3 => CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L3,
            _ => CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L1
        };
    }
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Console:ThirdFloorPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L2 = App.Configuration.GetValue<string>("Adjustmen2:Console:ThirdFloorPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:ThirdFloorPosition:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_THIRD_FLOOR_POSITION(int level)
    {
        return level switch
        {
            1 => CONSOLE_THIRD_FLOOR_POSITION_L1,
            2 => CONSOLE_THIRD_FLOOR_POSITION_L2,
            3 => CONSOLE_THIRD_FLOOR_POSITION_L3,
            _ => CONSOLE_THIRD_FLOOR_POSITION_L1
        };
    }
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L1 = App.Configuration.GetValue<string>("Adjustment:Console:PipeRotationDepartureDistance:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:PipeRotationDepartureDistance:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:PipeRotationDepartureDistance:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE(int level)
    {
        return level switch
        {
            1 => CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L1,
            2 => CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L2,
            3 => CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L3,
            _ => CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L1
        };
    }
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Console:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => CONSOLE_SPEED_COEFFICIENT_L1,
            2 => CONSOLE_SPEED_COEFFICIENT_L2,
            3 => CONSOLE_SPEED_COEFFICIENT_L3,
            _ => CONSOLE_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string CONSOLE_FACTICAL_POSITION = App.Configuration.GetValue<string>("AutomaticTags:Factical:Console:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Console:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment:Console:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment:Console:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => CONSOLE_CURRENT_POSITION_L1,
            2 => CONSOLE_CURRENT_POSITION_L2,
            3 => CONSOLE_CURRENT_POSITION_L3,
            _ => CONSOLE_CURRENT_POSITION_L1
        };
    }
    public static readonly string CONSOLE_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Console:Forward:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:Forward:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:Forward:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_FORWARD(int level)
    {
        return level switch
        {
            1 => CONSOLE_FORWARD_L1,
            2 => CONSOLE_FORWARD_L2,
            3 => CONSOLE_FORWARD_L3,
            _ => CONSOLE_FORWARD_L1
        };
    }
    public static readonly string CONSOLE_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Console:Backward:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:Backward:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:Backward:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_BACKWARD(int level)
    {
        return level switch
        {
            1 => CONSOLE_BACKWARD_L1,
            2 => CONSOLE_BACKWARD_L2,
            3 => CONSOLE_BACKWARD_L3,
            _ => CONSOLE_BACKWARD_L1
        };
    }
    public static readonly string CONSOLE_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Console:Reset:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_RESET_L2 = App.Configuration.GetValue<string>("Adjustment2:Console:Reset:RequestString") ?? string.Empty;
    public static readonly string CONSOLE_RESET_L3 = App.Configuration.GetValue<string>("Adjustment3:Console:Reset:RequestString") ?? string.Empty;
    public static string GET_CONSOLE_RESET(int level)
    {
        return level switch
        {
            1 => CONSOLE_RESET_L1,
            2 => CONSOLE_RESET_L2,
            3 => CONSOLE_RESET_L3,
            _ => CONSOLE_RESET_L1
        };
    }

    // √»¡
    public static readonly string BEND_FORWARD = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:ForwardRequestString") ?? string.Empty;
    public static readonly string BEND_BACKWARD = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:BackwardRequestString") ?? string.Empty;
    public static readonly string BEND_ACTUAL_COORDINATE = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:ActualCoordinateRequestString") ?? string.Empty;
    public static readonly string BEND_ACTUAL_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:ActualRelativeDisplacementRequestString") ?? string.Empty;
    public static readonly string BEND_RESET = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:ResetRequestString") ?? string.Empty;
    public static readonly string BEND_SPEED = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:SpeedRequestString") ?? string.Empty;
    public static readonly string BEND_TORQUE = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:TorqueRequestString") ?? string.Empty;
    public static readonly string BEND_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:RearPositionRequestString") ?? string.Empty; // ƒÛ·Î¸ ‚ MachineController:OutputsFB:Bend:RearPositionRequestString
    public static readonly string BEND_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:FrontPositionRequestString") ?? string.Empty; // ƒÛ·Î¸ ‚ MachineController:OutputsFB:Bend:FrontPositionRequestString
    public static readonly string BEND_RELATIVE_DISPLACEMENT = App.Configuration.GetValue<string>("MachineController:Drive:BendDrive:RelativeDisplacementRequestString") ?? string.Empty;
    public static readonly string BEND_FORWARD_BUTTON = App.Configuration.GetValue<string>("MachineController:OutputsFB:Bend:ForwardRequestString") ?? string.Empty;
    public static readonly string BEND_BACKWARD_BUTTON = App.Configuration.GetValue<string>("MachineController:OutputsFB:Bend:BackwardButton") ?? string.Empty;
    public static readonly string BEND_COEFFICIENT = App.Configuration.GetValue<string>("Settings:Bend:Coefficient:RequestString") ?? string.Empty;
    public static readonly string BEND_SYNCHRONIZATION = App.Configuration.GetValue<string>("Settings:Bend:Synchronization:RequestString") ?? string.Empty;
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L1 = App.Configuration.GetValue<string>("Adjustment:Bend:ForwardPositionLimitation:RequestString") ?? string.Empty;
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L2 = App.Configuration.GetValue<string>("Adjustment2:Bend:ForwardPositionLimitation:RequestString") ?? string.Empty;
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L3 = App.Configuration.GetValue<string>("Adjustment3:Bend:ForwardPositionLimitation:RequestString") ?? string.Empty;
    public static string GET_BEND_FORWARD_POSITION_LIMITATION(int level)
    {
        return level switch
        {
            1 => BEND_FORWARD_POSITION_LIMITATION_L1,
            2 => BEND_FORWARD_POSITION_LIMITATION_L2,
            3 => BEND_FORWARD_POSITION_LIMITATION_L3,
            _ => BEND_FORWARD_POSITION_LIMITATION_L1
        };
    }
    public static readonly string BEND_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Bend:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string BEND_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Bend:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string BEND_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Bend:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_BEND_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => BEND_SPEED_COEFFICIENT_L1,
            2 => BEND_SPEED_COEFFICIENT_L2,
            3 => BEND_SPEED_COEFFICIENT_L3,
            _ => BEND_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string BEND_SLOWDOWN_SPEED_L1 = App.Configuration.GetValue<string>("Adjustment:Bend:SlowdownSpeed:RequestString") ?? string.Empty;
    public static readonly string BEND_SLOWDOWN_SPEED_L2 = App.Configuration.GetValue<string>("Adjustment2:Bend:SlowdownSpeed:RequestString") ?? string.Empty;
    public static readonly string BEND_SLOWDOWN_SPEED_L3 = App.Configuration.GetValue<string>("Adjustment3:Bend:SlowdownSpeed:RequestString") ?? string.Empty;
    public static string GET_BEND_SLOWDOWN_SPEED(int level)
    {
        return level switch
        {
            1 => BEND_SLOWDOWN_SPEED_L1,
            2 => BEND_SLOWDOWN_SPEED_L2,
            3 => BEND_SLOWDOWN_SPEED_L3,
            _ => BEND_SLOWDOWN_SPEED_L1
        };
    }
    public static readonly string BEND_VALUE = App.Configuration.GetValue<string>("Program:Bend:Value:RequestString") ?? string.Empty;
    public static readonly string BEND_FACTICAL_POSITION = App.Configuration.GetValue<string>("AutomaticTags:Bending:Factical:RequestString") ?? string.Empty;

    // «¿∆»Ã
    public static readonly string CLAMP_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Clamp:ForwardRequestString") ?? string.Empty;
    public static readonly string CLAMP_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Clamp:BackwardRequestString") ?? string.Empty;
    public static readonly string CLAMP_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Clamp:RearPositionRequestString") ?? string.Empty;
    public static readonly string CLAMP_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Clamp:FrontPositionRequestString") ?? string.Empty;
    public static readonly string CLAMP_DEEP_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:Deep:RequestString") ?? string.Empty;
    public static readonly string CLAMP_DEEP_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:Deep:RequestString") ?? string.Empty;
    public static readonly string CLAMP_DEEP_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:Deep:RequestString") ?? string.Empty;
    public static string GET_CLAMP_DEEP(int level)
    {
        return level switch
        {
            1 => CLAMP_DEEP_L1,
            2 => CLAMP_DEEP_L2,
            3 => CLAMP_DEEP_L3,
            _ => CLAMP_DEEP_L1
        };
    }
    public static readonly string CLAMP_LENGTH_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:Length:RequestString") ?? string.Empty;
    public static readonly string CLAMP_LENGTH_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:Length:RequestString") ?? string.Empty;
    public static readonly string CLAMP_LENGTH_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:Length:RequestString") ?? string.Empty;
    public static string GET_CLAMP_LENGTH(int level)
    {
        return level switch
        {
            1 => CLAMP_LENGTH_L1,
            2 => CLAMP_LENGTH_L2,
            3 => CLAMP_LENGTH_L3,
            _ => CLAMP_LENGTH_L1
        };
    }
    public static readonly string CLAMP_FORWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_FORWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_FORWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:ForwardPosition:RequestString") ?? string.Empty;
    public static string GET_CLAMP_FORWARD_POSITION(int level)
    {
        return level switch
        {
            1 => CLAMP_FORWARD_POSITION_L1,
            2 => CLAMP_FORWARD_POSITION_L2,
            3 => CLAMP_FORWARD_POSITION_L3,
            _ => CLAMP_FORWARD_POSITION_L1
        };
    }
    public static readonly string CLAMP_MIDDLE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_MIDDLE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_MIDDLE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:MiddlePosition:RequestString") ?? string.Empty;
    public static string GET_CLAMP_MIDDLE_POSITION(int level)
    {
        return level switch
        {
            1 => CLAMP_MIDDLE_POSITION_L1,
            2 => CLAMP_MIDDLE_POSITION_L2,
            3 => CLAMP_MIDDLE_POSITION_L3,
            _ => CLAMP_MIDDLE_POSITION_L1
        };
    }
    public static readonly string CLAMP_BACKWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_BACKWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_BACKWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:BackwardPosition:RequestString") ?? string.Empty;
    public static string GET_CLAMP_BACKWARD_POSITION(int level)
    {
        return level switch
        {
            1 => CLAMP_BACKWARD_POSITION_L1,
            2 => CLAMP_BACKWARD_POSITION_L2,
            3 => CLAMP_BACKWARD_POSITION_L3,
            _ => CLAMP_BACKWARD_POSITION_L1
        };
    }
    public static readonly string CLAMP_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string CLAMP_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string CLAMP_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_CLAMP_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => CLAMP_SPEED_COEFFICIENT_L1,
            2 => CLAMP_SPEED_COEFFICIENT_L2,
            3 => CLAMP_SPEED_COEFFICIENT_L3,
            _ => CLAMP_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string CLAMP_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string CLAMP_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_CLAMP_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => CLAMP_CURRENT_POSITION_L1,
            2 => CLAMP_CURRENT_POSITION_L2,
            3 => CLAMP_CURRENT_POSITION_L3,
            _ => CLAMP_CURRENT_POSITION_L1
        };
    }
    public static readonly string CLAMP_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:Forward:RequestString") ?? string.Empty;
    public static readonly string CLAMP_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:Forward:RequestString") ?? string.Empty;
    public static readonly string CLAMP_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:Forward:RequestString") ?? string.Empty;
    public static string GET_CLAMP_FORWARD(int level)
    {
        return level switch
        {
            1 => CLAMP_FORWARD_L1,
            2 => CLAMP_FORWARD_L2,
            3 => CLAMP_FORWARD_L3,
            _ => CLAMP_FORWARD_L1
        };
    }
    public static readonly string CLAMP_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:Backward:RequestString") ?? string.Empty;
    public static readonly string CLAMP_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:Backward:RequestString") ?? string.Empty;
    public static readonly string CLAMP_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:Backward:RequestString") ?? string.Empty;
    public static string GET_CLAMP_BACKWARD(int level)
    {
        return level switch
        {
            1 => CLAMP_BACKWARD_L1,
            2 => CLAMP_BACKWARD_L2,
            3 => CLAMP_BACKWARD_L3,
            _ => CLAMP_BACKWARD_L1
        };
    }
    public static readonly string CLAMP_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Clamp:Reset:RequestString") ?? string.Empty;
    public static readonly string CLAMP_RESET_L2 = App.Configuration.GetValue<string>("Adjustment2:Clamp:Reset:RequestString") ?? string.Empty;
    public static readonly string CLAMP_RESET_L3 = App.Configuration.GetValue<string>("Adjustment3:Clamp:Reset:RequestString") ?? string.Empty;
    public static string GET_CLAMP_RESET(int level)
    {
        return level switch
        {
            1 => CLAMP_RESET_L1,
            2 => CLAMP_RESET_L2,
            3 => CLAMP_RESET_L3,
            _ => CLAMP_RESET_L1
        };
    }

    // œ–»∆»Ã
    public static readonly string PRESS_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Press:ForwardRequestString") ?? string.Empty;
    public static readonly string PRESS_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Press:BackwardRequestString") ?? string.Empty;
    public static readonly string PRESS_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Press:RearPositionRequestString") ?? string.Empty;
    public static readonly string PRESS_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Press:FrontPosition") ?? string.Empty;
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L1 = App.Configuration.GetValue<string>("Adjustment:Press:DangerZoneCoordinate:RequestString") ?? string.Empty;
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:DangerZoneCoordinate:RequestString") ?? string.Empty;
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:DangerZoneCoordinate:RequestString") ?? string.Empty;
    public static string GET_PRESS_DANGER_ZONE_COORDINATE(int level)
    {
        return level switch
        {
            1 => PRESS_DANGER_ZONE_COORDINATE_L1,
            2 => PRESS_DANGER_ZONE_COORDINATE_L2,
            3 => PRESS_DANGER_ZONE_COORDINATE_L3,
            _ => PRESS_DANGER_ZONE_COORDINATE_L1
        };
    }
    public static readonly string PRESS_LENGTH_L1 = App.Configuration.GetValue<string>("Adjustment:Press:Length:RequestString") ?? string.Empty;
    public static readonly string PRESS_LENGTH_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:Length:RequestString") ?? string.Empty;
    public static readonly string PRESS_LENGTH_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:Length:RequestString") ?? string.Empty;
    public static string GET_PRESS_LENGTH(int level)
    {
        return level switch
        {
            1 => PRESS_LENGTH_L1,
            2 => PRESS_LENGTH_L2,
            3 => PRESS_LENGTH_L3,
            _ => PRESS_LENGTH_L1
        };
    }
    public static readonly string PRESS_FORWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Press:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_FORWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_FORWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:ForwardPosition:RequestString") ?? string.Empty;
    public static string GET_PRESS_FORWARD_POSITION(int level)
    {
        return level switch
        {
            1 => PRESS_FORWARD_POSITION_L1,
            2 => PRESS_FORWARD_POSITION_L2,
            3 => PRESS_FORWARD_POSITION_L3,
            _ => PRESS_FORWARD_POSITION_L1
        };
    }
    public static readonly string PRESS_MIDDLE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Press:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_MIDDLE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_MIDDLE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:MiddlePosition:RequestString") ?? string.Empty;
    public static string GET_PRESS_MIDDLE_POSITION(int level)
    {
        return level switch
        {
            1 => PRESS_MIDDLE_POSITION_L1,
            2 => PRESS_MIDDLE_POSITION_L2,
            3 => PRESS_MIDDLE_POSITION_L3,
            _ => PRESS_MIDDLE_POSITION_L1
        };
    }
    public static readonly string PRESS_BACKWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Press:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_BACKWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_BACKWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:BackwardPosition:RequestString") ?? string.Empty;
    public static string GET_PRESS_BACKWARD_POSITION(int level)
    {
        return level switch
        {
            1 => PRESS_BACKWARD_POSITION_L1,
            2 => PRESS_BACKWARD_POSITION_L2,
            3 => PRESS_BACKWARD_POSITION_L3,
            _ => PRESS_BACKWARD_POSITION_L1
        };
    }
    public static readonly string PRESS_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Press:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string PRESS_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string PRESS_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_PRESS_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => PRESS_SPEED_COEFFICIENT_L1,
            2 => PRESS_SPEED_COEFFICIENT_L2,
            3 => PRESS_SPEED_COEFFICIENT_L3,
            _ => PRESS_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string PRESS_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Press:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string PRESS_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_PRESS_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => PRESS_CURRENT_POSITION_L1,
            2 => PRESS_CURRENT_POSITION_L2,
            3 => PRESS_CURRENT_POSITION_L3,
            _ => PRESS_CURRENT_POSITION_L1
        };
    }
    public static readonly string PRESS_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Press:Forward:RequestString") ?? string.Empty;
    public static readonly string PRESS_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:Forward:RequestString") ?? string.Empty;
    public static readonly string PRESS_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:Forward:RequestString") ?? string.Empty;
    public static string GET_PRESS_FORWARD(int level)
    {
        return level switch
        {
            1 => PRESS_FORWARD_L1,
            2 => PRESS_FORWARD_L2,
            3 => PRESS_FORWARD_L3,
            _ => PRESS_FORWARD_L1
        };
    }
    public static readonly string PRESS_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Press:Backward:RequestString") ?? string.Empty;
    public static readonly string PRESS_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:Backward:RequestString") ?? string.Empty;
    public static readonly string PRESS_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:Backward:RequestString") ?? string.Empty;
    public static string GET_PRESS_BACKWARD(int level)
    {
        return level switch
        {
            1 => PRESS_BACKWARD_L1,
            2 => PRESS_BACKWARD_L2,
            3 => PRESS_BACKWARD_L3,
            _ => PRESS_BACKWARD_L1
        };
    }
    public static readonly string PRESS_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Press:Reset:RequestString") ?? string.Empty;
    public static readonly string PRESS_RESET_L2 = App.Configuration.GetValue<string>("Adjustment2:Press:Reset:RequestString") ?? string.Empty;
    public static readonly string PRESS_RESET_L3 = App.Configuration.GetValue<string>("Adjustment3:Press:Reset:RequestString") ?? string.Empty;
    public static string GET_PRESS_RESET(int level)
    {
        return level switch
        {
            1 => PRESS_RESET_L1,
            2 => PRESS_RESET_L2,
            3 => PRESS_RESET_L3,
            _ => PRESS_RESET_L1,
        };
    }

    // œ≈–¬€… ƒŒ∆»Ã
    public static readonly string FIRST_SQUEEZE_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:ForwardRequestString") ?? string.Empty;
    public static readonly string FIRST_SQUEEZE_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:BackwardRequestString") ?? string.Empty;
    public static readonly string FIRST_SQUEEZE_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:RearPositionRequestString") ?? string.Empty;
    public static readonly string FIRST_SQUEEZE_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:FrontPositionRequestString") ?? string.Empty;
    public static readonly string FIRST_SQUEEZE_REAR_SECOND_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:RearSecondPositionReqeustString") ?? string.Empty;
    public static readonly string FIRST_SQUEEZE_FRONT_SECOND_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstSqueeze:FrontSecondPositionRequestString") ?? string.Empty;

    // ÷¿Õ√¿
    public static readonly string COLLET_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Collet:ForwardRequestString") ?? string.Empty;
    public static readonly string COLLET_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Collet:BackwardRequestString") ?? string.Empty;
    public static readonly string COLLET_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Collet:RearPositionRequestString") ?? string.Empty;
    public static readonly string COLLET_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Collet:FrontPositionRequestString") ?? string.Empty;

    // ƒŒ–Õ
    public static readonly string DORN_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Dorn:ForwardRequestString") ?? string.Empty;
    public static readonly string DORN_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Dorn:BackwardRequestString") ?? string.Empty;
    public static readonly string DORN_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Dorn:RearPositionRequestString") ?? string.Empty;
    public static readonly string DORN_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Dorn:FrontPositionRequestString") ?? string.Empty;
    public static readonly string DORN_AUTOMATIC = App.Configuration.GetValue<string>("Settings:Dorn:Automatic:RequestString") ?? string.Empty;
    public static readonly string DORN_LEAD_WITHDRAWAL_BEFORE_BEND = App.Configuration.GetValue<string>("Settings:Dorn:LeadWithdrawalBeforeBend:RequestString") ?? string.Empty;
    public static readonly string DORN_FORWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_FORWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:ForwardPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_FORWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:ForwardPosition:RequestString") ?? string.Empty;
    public static string GET_DORN_FORWARD_POSITION(int level)
    {
        return level switch
        {
            1 => DORN_FORWARD_POSITION_L1,
            2 => DORN_FORWARD_POSITION_L2,
            3 => DORN_FORWARD_POSITION_L3,
            _ => DORN_FORWARD_POSITION_L1
        };
    }
    public static readonly string DORN_MIDDLE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string DORN_MIDDLE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string DORN_MIDDLE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:MiddlePosition:RequestString") ?? string.Empty;
    public static string GET_DORN_MIDDLE_POSITION(int level)
    {
        return level switch
        {
            1 => DORN_MIDDLE_POSITION_L1,
            2 => DORN_MIDDLE_POSITION_L2,
            3 => DORN_MIDDLE_POSITION_L3,
            _ => DORN_MIDDLE_POSITION_L1
        };
    }
    public static readonly string DORN_BACKWARD_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_BACKWARD_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:BackwardPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_BACKWARD_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:BackwardPosition:RequestString") ?? string.Empty;
    public static string GET_DORN_BACKWARD_POSITION(int level)
    {
        return level switch
        {
            1 => DORN_BACKWARD_POSITION_L1,
            2 => DORN_BACKWARD_POSITION_L2,
            3 => DORN_BACKWARD_POSITION_L3,
            _ => DORN_BACKWARD_POSITION_L1
        };
    }
    public static readonly string DORN_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string DORN_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string DORN_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_DORN_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => DORN_SPEED_COEFFICIENT_L1,
            2 => DORN_SPEED_COEFFICIENT_L2,
            3 => DORN_SPEED_COEFFICIENT_L3,
            _ => DORN_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string DORN_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string DORN_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_DORN_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => DORN_CURRENT_POSITION_L1,
            2 => DORN_CURRENT_POSITION_L2,
            3 => DORN_CURRENT_POSITION_L3,
            _ => DORN_CURRENT_POSITION_L1
        };
    }
    public static readonly string DORN_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static string GET_DORN_FORWARD(int level)
    {
        return level switch
        {
            1 => DORN_FORWARD_L1,
            2 => DORN_FORWARD_L2,
            3 => DORN_FORWARD_L3,
            _ => DORN_FORWARD_L1
        };
    }
    public static readonly string DORN_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static string GET_DORN_BACKWARD(int level)
    {
        return level switch
        {
            1 => DORN_BACKWARD_L1,
            2 => DORN_BACKWARD_L2,
            3 => DORN_BACKWARD_L3,
            _ => DORN_BACKWARD_L1
        };
    }
    public static readonly string DORN_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_RESET_L2 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static readonly string DORN_RESET_L3 = App.Configuration.GetValue<string>("Adjustment:Dorn:RequestString") ?? string.Empty;
    public static string GET_DORN_RESET(int level)
    {
        return level switch
        {
            1 => DORN_RESET_L1,
            2 => DORN_RESET_L2,
            3 => DORN_RESET_L3,
            _ => DORN_RESET_L1
        };
    }

    // Œ—Õ¿—“ ¿
    public static readonly string ADJUSTMENT_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Adjustment:ForwardRequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Adjustment:BackwardRequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Adjustment:RearPositionRequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Adjustment:CenterPositionRequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_CENTER_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Adjustment:FrontPositionRequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_TYPE_L1 = App.Configuration.GetValue<string>("Adjustment:AdjustmentType:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_TYPE_L2 = App.Configuration.GetValue<string>("Adjustment2:AdjustmentType:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_TYPE_L3 = App.Configuration.GetValue<string>("Adjustment3:AdjustmentType:RequestString") ?? string.Empty;
    public static string GET_ADJUSTMENT_TYPE(int level)
    {
        return level switch
        {
            1 => ADJUSTMENT_TYPE_L1,
            2 => ADJUSTMENT_TYPE_L2,
            3 => ADJUSTMENT_TYPE_L3,
            _ => ADJUSTMENT_TYPE_L1
        };
    }
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L1 = App.Configuration.GetValue<string>("Adjustment:PipeDiameter:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L2 = App.Configuration.GetValue<string>("Adjustment2:PipeDiameter:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L3 = App.Configuration.GetValue<string>("Adjustment3:PipeDiameter:RequestString") ?? string.Empty;
    public static string GET_ADJUSTMENT_PIPE_DIAMETER(int level)
    {
        return level switch
        {
            1 => ADJUSTMENT_PIPE_DIAMETER_L1,
            2 => ADJUSTMENT_PIPE_DIAMETER_L2,
            3 => ADJUSTMENT_PIPE_DIAMETER_L3,
            _ => ADJUSTMENT_PIPE_DIAMETER_L1
        };
    }
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L1 = App.Configuration.GetValue<string>("Adjustment:ForwardDangerZoneCoordinate:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L2 = App.Configuration.GetValue<string>("Adjustment2:ForwardDangerZoneCoordinate:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L3 = App.Configuration.GetValue<string>("Adjustment3:ForwardDangerZoneCoordinate:RequestString") ?? string.Empty;
    public static string GET_ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE(int level)
    {
        return level switch
        {
            1 => ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L1,
            2 => ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L2,
            3 => ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L3,
            _ => ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L1
        };
    }
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L1 = App.Configuration.GetValue<string>("Adjustment:DistanceFromCenter:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L2 = App.Configuration.GetValue<string>("Adjustment2:DistanceFromCenter:RequestString") ?? string.Empty;
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L3 = App.Configuration.GetValue<string>("Adjustment3:DistanceFromCenter:RequestString") ?? string.Empty;
    public static string GET_ADJUSTMENT_DISTANCE_FROM_CENTER(int level)
    {
        return level switch
        {
            1 => ADJUSTMENT_DISTANCE_FROM_CENTER_L1,
            2 => ADJUSTMENT_DISTANCE_FROM_CENTER_L2,
            3 => ADJUSTMENT_DISTANCE_FROM_CENTER_L3,
            _ => ADJUSTMENT_DISTANCE_FROM_CENTER_L1
        };
    }

    // œ–Œ¡»¬ ¿
    public static readonly string PUNCHING_FORWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Punching:ForwardRequestString") ?? string.Empty;
    public static readonly string PUNCHING_BACKWARD = App.Configuration.GetValue<string>("MachineController:OutputsFB:Punching:BackwardRequestString") ?? string.Empty;
    public static readonly string PUNCHING_REAR_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Punching:RearPositionRequestString") ?? string.Empty;
    public static readonly string PUNCHING_FRONT_POSITION = App.Configuration.GetValue<string>("MachineController:OutputsFB:Punching:FrontPositionRequestString") ?? string.Empty;

    // √»ƒ–¿¬À» ¿
    public static readonly string FIRST_HYDRAULICS_VALUE = App.Configuration.GetValue<string>("MachineController:OutputsFB:FirstHydraulics:RequestString") ?? string.Empty;
    public static readonly string SECOND_HYDRAULICS_VALUE = App.Configuration.GetValue<string>("MachineController:OutputsFB:SecondHydraulics:RequestString") ?? string.Empty;
    
    // œŒƒƒ≈–∆ ¿
    public static readonly string SUPPORT_VALUE = App.Configuration.GetValue<string>("MachineController:OutputsFB:Support:RequestString") ?? string.Empty;
    public static readonly string SUPPORT_FRONT_LIFT_BAN = App.Configuration.GetValue<string>("Settings:Support:FrontLiftBan:RequestString") ?? string.Empty;
    public static readonly string SUPPORT_MIDDLE_LIFT_BAN = App.Configuration.GetValue<string>("Settings:Support:MiddleLiftBan:RequestString") ?? string.Empty;
    public static readonly string SUPPORT_BACK_LIFT_BAN = App.Configuration.GetValue<string>("Settings:Support:BackLiftBan:RequestString") ?? string.Empty;

    // —Ã¿« ¿ ƒŒ–Õ¿
    public static readonly string DORN_LUBRICANT_TURN_ON = App.Configuration.GetValue<string>("MachineController:OutputsFB:DornLubricant:RequestString") ?? string.Empty;
    public static readonly string DORN_LUBRICANT_LUBRICANT_TURN_ON = App.Configuration.GetValue<string>("Settings:Dorn:LubricantTurnOn:RequestString") ?? string.Empty;

    // √»¡ » ƒŒ∆»Ã
    public static readonly string BEND_AND_SQUEEZE_VALUE = App.Configuration.GetValue<string>("MachineController:OutputsFB:BendAndSqueeze:RequestString") ?? string.Empty;

    // Œÿ»¡ »
    public static readonly string ERRORS_CLEAR_ACTUATOR_ERRORS = App.Configuration.GetValue<string>("MachineController:ClearActuatorErrors:RequestString") ?? string.Empty;
    public static readonly string ERRORS_HAS_ERRORS = App.Configuration.GetValue<string>("MachineController:StopErrors:RequestString") ?? string.Empty;

    // “–”¡¿
    public static readonly string PIPE_OUTLET_COORDINATE = App.Configuration.GetValue<string>("Settings:Pipe:OutletCoordinate:RequestString") ?? string.Empty;
    public static readonly string PIPE_INSTALLATION_COORDINATE = App.Configuration.GetValue<string>("Program:Pipe:InstallationCoordinate:RequestString") ?? string.Empty;
    public static readonly string PIPE_LENGTH = App.Configuration.GetValue<string>("Program:Pipe:Length:RequestString") ?? string.Empty;

    // √»¡Œ◊Õ€… –ŒÀ» 
    public static readonly string BEND_ROLLER_RADIUS_L1 = App.Configuration.GetValue<string>("Adjustment:BendRoller:Radius:RequestString") ?? string.Empty;
    public static readonly string BEND_ROLLER_RADIUS_L2 = App.Configuration.GetValue<string>("Adjustment2:BendRoller:Radius:RequestString") ?? string.Empty;
    public static readonly string BEND_ROLLER_RADIUS_L3 = App.Configuration.GetValue<string>("Adjustment3:BendRoller:Radius:RequestString") ?? string.Empty;
    public static string GET_BEND_ROLLER_RADIUS(int level)
    {
        return level switch
        {
            1 => BEND_ROLLER_RADIUS_L1,
            2 => BEND_ROLLER_RADIUS_L2,
            3 => BEND_ROLLER_RADIUS_L3,
            _ => BEND_ROLLER_RADIUS_L1
        };
    }
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L1 = App.Configuration.GetValue<string>("Adjustment:BendRoller:OuterRadius:RequestString") ?? string.Empty;
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L2 = App.Configuration.GetValue<string>("Adjustment2:BendRoller:OuterRadius:RequestString") ?? string.Empty;
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L3 = App.Configuration.GetValue<string>("Adjustment3:BendRoller:OuterRadius:RequestString") ?? string.Empty;
    public static string GET_BEND_ROLLER_OUTER_RADIUS(int level)
    {
        return level switch
        {
            1 => BEND_ROLLER_OUTER_RADIUS_L1,
            2 => BEND_ROLLER_OUTER_RADIUS_L2,
            3 => BEND_ROLLER_OUTER_RADIUS_L3,
            _ => BEND_ROLLER_OUTER_RADIUS_L1
        };
    }

    // «¿∆»ÃÕŒ… –ŒÀ» 
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L1 = App.Configuration.GetValue<string>("Adjustment:ClampRoller:OuterRadius:RequestString") ?? string.Empty;
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L2 = App.Configuration.GetValue<string>("Adjustment2:ClampRoller:OuterRadius:RequestString") ?? string.Empty;
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L3 = App.Configuration.GetValue<string>("Adjustment3:ClampRoller:OuterRadius:RequestString") ?? string.Empty;
    public static string GET_CLAMP_ROLLER_OUTER_RADIUS(int level)
    {
        return level switch
        {
            1 => CLAMP_ROLLER_OUTER_RADIUS_L1,
            2 => CLAMP_ROLLER_OUTER_RADIUS_L2,
            3 => CLAMP_ROLLER_OUTER_RADIUS_L3,
            _ => CLAMP_ROLLER_OUTER_RADIUS_L1
        };
    }
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L1 = App.Configuration.GetValue<string>("Adjustment:ClampRoller:InnerRadius:RequestString") ?? string.Empty;
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L2 = App.Configuration.GetValue<string>("Adjustment2:ClampRoller:InnerRadius:RequestString") ?? string.Empty;
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L3 = App.Configuration.GetValue<string>("Adjustment3:ClampRoller:InnerRadius:RequestString") ?? string.Empty;
    public static string GET_CLAMP_ROLLER_INNTER_RADIUS(int level)
    {
        return level switch
        {
            1 => CLAMP_ROLLER_INNER_RADIUS_L1,
            2 => CLAMP_ROLLER_INNER_RADIUS_L2,
            3 => CLAMP_ROLLER_INNER_RADIUS_L3,
            _ => CLAMP_ROLLER_INNER_RADIUS_L1
        };
    }

    // ƒŒ∆»Ã
    public static readonly string SQUEEZE_TURN_ON_L1 = App.Configuration.GetValue<string>("Adjustment:Squeeze:TurnOn:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_TURN_ON_L2 = App.Configuration.GetValue<string>("Adjustment2:Squeeze:TurnOn:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_TURN_ON_L3 = App.Configuration.GetValue<string>("Adjustment3:Squeeze:TurnOn:RequestString") ?? string.Empty;
    public static string GET_SQUEEZE_TURN_ON(int level)
    {
        return level switch
        {
            1 => SQUEEZE_TURN_ON_L1,
            2 => SQUEEZE_TURN_ON_L2,
            3 => SQUEEZE_TURN_ON_L3,
            _ => SQUEEZE_TURN_ON_L1
        };
    }
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L1 = App.Configuration.GetValue<string>("Adjustment:Squeeze:FrontPositionLimitation:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L2 = App.Configuration.GetValue<string>("Adjustment2:Squeeze:FrontPositionLimitation:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L3 = App.Configuration.GetValue<string>("Adjustment3:Squeeze:FrontPositionLimitation:RequestString") ?? string.Empty;
    public static string GET_SQUEEZE_FRONT_POSITION_LIMITATION(int level)
    {
        return level switch
        {
            1 => SQUEEZE_FRONT_POSITION_LIMITATION_L1,
            2 => SQUEEZE_FRONT_POSITION_LIMITATION_L2,
            3 => SQUEEZE_FRONT_POSITION_LIMITATION_L3,
            _ => SQUEEZE_FRONT_POSITION_LIMITATION_L1
        };
    }
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Squeeze:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Squeeze:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Squeeze:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_SQUEEZE_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => SQUEEZE_SPEED_COEFFICIENT_L1,
            2 => SQUEEZE_SPEED_COEFFICIENT_L2,
            3 => SQUEEZE_SPEED_COEFFICIENT_L3,
            _ => SQUEEZE_SPEED_COEFFICIENT_L1
        };
    }

    // œŒƒ⁄®Ã
    public static readonly string LIFT_UPPER_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:UpperPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_UPPER_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:UpperPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_UPPER_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:UpperPosition:RequestString") ?? string.Empty;
    public static string GET_LIFT_UPPER_POSITION(int level)
    {
        return level switch
        {
            1 => LIFT_UPPER_POSITION_L1,
            2 => LIFT_UPPER_POSITION_L2,
            3 => LIFT_UPPER_POSITION_L3,
            _ => LIFT_UPPER_POSITION_L1
        };
    }
    public static readonly string LIFT_MIDDLE_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_MIDDLE_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:MiddlePosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_MIDDLE_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:MiddlePosition:RequestString") ?? string.Empty;
    public static string GET_LIFT_MIDDLE_POSITION(int level)
    {
        return level switch
        {
            1 => LIFT_MIDDLE_POSITION_L1,
            2 => LIFT_MIDDLE_POSITION_L2,
            3 => LIFT_MIDDLE_POSITION_L3,
            _ => LIFT_MIDDLE_POSITION_L1
        };
    }
    public static readonly string LIFT_LOWER_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:LowerPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_LOWER_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:LowerPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_LOWER_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:LowerPosition:RequestString") ?? string.Empty;
    public static string GET_LIFT_LOWER_POSITION(int level)
    {
        return level switch
        {
            1 => LIFT_LOWER_POSITION_L1,
            2 => LIFT_LOWER_POSITION_L2,
            3 => LIFT_LOWER_POSITION_L3,
            _ => LIFT_LOWER_POSITION_L1
        };
    }
    public static readonly string LIFT_SPEED_COEFFICIENT_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string LIFT_SPEED_COEFFICIENT_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:SpeedCoefficient:RequestString") ?? string.Empty;
    public static readonly string LIFT_SPEED_COEFFICIENT_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:SpeedCoefficient:RequestString") ?? string.Empty;
    public static string GET_LIFT_SPEED_COEFFICIENT(int level)
    {
        return level switch
        {
            1 => LIFT_SPEED_COEFFICIENT_L1,
            2 => LIFT_SPEED_COEFFICIENT_L2,
            3 => LIFT_SPEED_COEFFICIENT_L3,
            _ => LIFT_SPEED_COEFFICIENT_L1
        };
    }
    public static readonly string LIFT_CURRENT_POSITION_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_CURRENT_POSITION_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:CurrentPosition:RequestString") ?? string.Empty;
    public static readonly string LIFT_CURRENT_POSITION_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:CurrentPosition:RequestString") ?? string.Empty;
    public static string GET_LIFT_CURRENT_POSITION(int level)
    {
        return level switch
        {
            1 => LIFT_CURRENT_POSITION_L1,
            2 => LIFT_CURRENT_POSITION_L2,
            3 => LIFT_CURRENT_POSITION_L3,
            _ => LIFT_CURRENT_POSITION_L1
        };
    }
    public static readonly string LIFT_FORWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:Forward:RequestString") ?? string.Empty;
    public static readonly string LIFT_FORWARD_L2 = App.Configuration.GetValue<string>("Adjustment2:Lift:Forward:RequestString") ?? string.Empty;
    public static readonly string LIFT_FORWARD_L3 = App.Configuration.GetValue<string>("Adjustment3:Lift:Forward:RequestString") ?? string.Empty;
    public static string GET_LIFT_FORWARD(int level)
    {
        return level switch
        {
            1 => LIFT_FORWARD_L1,
            2 => LIFT_FORWARD_L2,
            3 => LIFT_FORWARD_L3,
            _ => LIFT_FORWARD_L1
        };
    }
    public static readonly string LIFT_BACKWARD_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:Backward:RequestString") ?? string.Empty;
    public static readonly string LIFT_BACKWARD_L2 = App.Configuration.GetValue<string>("Adjustment:Lift:Backward:RequestString") ?? string.Empty;
    public static readonly string LIFT_BACKWARD_L3 = App.Configuration.GetValue<string>("Adjustment:Lift:Backward:RequestString") ?? string.Empty;
    public static string GET_LIFT_BACKWARD(int level)
    {
        return level switch
        {
            1 => LIFT_BACKWARD_L1,
            2 => LIFT_BACKWARD_L2,
            3 => LIFT_BACKWARD_L3,
            _ => LIFT_BACKWARD_L1
        };
    }
    public static readonly string LIFT_RESET_L1 = App.Configuration.GetValue<string>("Adjustment:Lift:Reset:RequestString") ?? string.Empty;
    public static readonly string LIFT_RESET_L2 = App.Configuration.GetValue<string>("Adjustmen2:Lift:Reset:RequestString") ?? string.Empty;
    public static readonly string LIFT_RESET_L3 = App.Configuration.GetValue<string>("Adjustmen3:Lift:Reset:RequestString") ?? string.Empty;
    public static string GET_LIFT_RESET(int level)
    {
        return level switch
        {
            1 => LIFT_RESET_L1,
            2 => LIFT_RESET_L2,
            3 => LIFT_RESET_L3,
            _ => LIFT_RESET_L1
        };
    }

    // œ–Œ√–¿ÃÃ¿
    public static readonly string PROGRAM_RADIUS = App.Configuration.GetValue<string>("Program:Radius:RequestString") ?? string.Empty;
    public static readonly string PROGRAM_FUNCTION = App.Configuration.GetValue<string>("Program:Function:RequestString") ?? string.Empty;
    public static readonly string PROGRAM_COEFFICIENT = App.Configuration.GetValue<string>("Program:Coefficient:RequestString") ?? string.Empty;
    public static readonly string PROGRAM_SPEED = App.Configuration.GetValue<string>("Program:Speed:RequestString") ?? string.Empty;
    
    // Œ“¬Œƒ
    public static readonly string OUTLET_FORWARD = App.Configuration.GetValue<string>("Program:Outlet:Forward:RequestString") ?? string.Empty;
    public static readonly string OUTLET_BACKWARD = App.Configuration.GetValue<string>("Program:Outlet:Backward:RequestString") ?? string.Empty;
    public static readonly string OUTLET_ROTATION = App.Configuration.GetValue<string>("Program:Outlet:Rotation:RequestString") ?? string.Empty;

    // Õ¿—“–Œ… »
    public static readonly string SETTINGS_SPEED = App.Configuration.GetValue<string>("Settings:Speed:RequestString") ?? string.Empty;
    public static readonly string SETTINGS_CYNCHRONIZATION_COEFFICIENT = App.Configuration.GetValue<string>("Settings:SynchronizationCoefficient:RequestString") ?? string.Empty;
    public static readonly string SETTINGS_INTERCEPTION_MODE = App.Configuration.GetValue<string>("Settings:InterceptionMode:RequestString") ?? string.Empty;

    // ¿¬“ŒÃ¿“
    public static readonly string AUTOMATIC_TAGS_TURN_ON = App.Configuration.GetValue<string>("AutomaticTags:TurnOn:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_CYCLE_TIME = App.Configuration.GetValue<string>("AutomaticTags:CycleTime:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_SEND_DATA = App.Configuration.GetValue<string>("AutomaticTags:SendData:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_END_PROGRAM = App.Configuration.GetValue<string>("AutomaticTags:EndProgram:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_STEP_NUMBER = App.Configuration.GetValue<string>("AutomaticTags:StepNumber:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_ALL_BEND = App.Configuration.GetValue<string>("AutomaticTags:AllBend:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_FULL_AUTOMATIC = App.Configuration.GetValue<string>("AutomaticTags:FullAutomatic:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_DELAY = App.Configuration.GetValue<string>("AutomaticTags:Delay:RequestString") ?? string.Empty;
    public static readonly string AUTOMATIC_TAGS_COUNT_COMPLETED_DETAILS = App.Configuration.GetValue<string>("AutomaticTags:CountCompletedDetails:RequestString") ?? string.Empty;
}