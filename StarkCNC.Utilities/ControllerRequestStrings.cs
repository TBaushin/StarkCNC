using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace StarkCNC.Utilities;

[SuppressMessage("Usage", "CA1707", Justification = "Ñïåöèàëüíî ñäåëàíî, ÷òîá âûäåëèòü")]
public static class ControllerRequestStrings
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
        .Build();

    public static readonly string REQUEST = GetValue("Connection:RequestString");

    public static readonly string MANUAL_MODE = GetValue("RequestStrings:Manual:ManualMode");
    
    // ÏÎÄÀ×À
    public static readonly string SUPPLY_FORWARD = GetValue("RequestStrings:Supply:Forward");
    public static readonly string SUPPLY_BACKWARD = GetValue("RequestStrings:Supply:Backward");
    public static readonly string SUPPLY_ACTUAL_COORDINATE = GetValue("RequestStrings:Supply:ActualCoordinate");
    public static readonly string SUPPLY_ACTUAL_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Supply:ActualRelativeDisplacement");
    public static readonly string SUPPLY_RESET = GetValue("RequestStrings:Supply:Reset");
    public static readonly string SUPPLY_SPEED = GetValue("RequestStrings:Supply:Speed");
    public static readonly string SUPPLY_TORQUE = GetValue("RequestStrings:Supply:Torque");
    public static readonly string SUPPLY_REAR_POSITION = GetValue("RequestStrings:Supply:RearPosition");
    public static readonly string SUPPLY_FRONT_POSITION = GetValue("RequestStrings:Supply:FrontPosition");
    public static readonly string SUPPLY_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Supply:RelativeDisposition");
    public static readonly string SUPPLY_RESETED_OFFSET = GetValue("RequestStrings:Supply:ResetedOffset");
    public static readonly string SUPPLY_START_ROLLING_SPEED = GetValue("RequestStrings:Supply:StartRollingSpeed");
    public static readonly string SUPPLY_COEFFICIENT = GetValue("RequestStrings:Supply:Coefficient");
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L1 = GetValue("RequestStrings:Supply:PressZonePositionL1");
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L2 = GetValue("RequestStrings:Supply:PressZonePositionL2");
    public static readonly string SUPPLY_PRESS_ZONE_POSITION_L3 = GetValue("RequestStrings:Supply:PressZonePositionL3");
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
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L1 = GetValue("RequestStrings:Supply:ForwardDangerZoneL1");
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L2 = GetValue("RequestStrings:Supply:ForwardDangerZoneL2");
    public static readonly string SUPPLY_FORWARD_DANGER_ZONE_L3 = GetValue("RequestStrings:Supply:ForwardDangerZoneL3");
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
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L1 = GetValue("RequestStrings:Supply:ColletJawsDepthL1");
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L2 = GetValue("RequestStrings:Supply:ColletJawsDepthL2");
    public static readonly string SUPPLY_COLLET_JAWS_DEPTH_L3 = GetValue("RequestStrings:Supply:ColletJawsDepthL3");
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
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Supply:SpeedCoefficientL1");
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Supply:SpeedCoefficientL2");
    public static readonly string SUPPLY_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Supply:SpeedCoefficientL3");
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
    public static readonly string SUPPLY_VALUE = GetValue("RequestStrings:Supply:Value");
    public static readonly string SUPPLY_FACTICAL_POSITION = GetValue("RequestStrings:Supply:Factical");
    public static readonly string SUPPLY_CURRENT_POSITION_L1 = GetValue("RequestStrings:Supply:CurrentPositionL1");
    public static readonly string SUPPLY_CURRENT_POSITION_L2 = GetValue("RequestStrings:Supply:CurrentPositionL2");
    public static readonly string SUPPLY_CURRENT_POSITION_L3 = GetValue("RequestStrings:Supply:CurrentPositionL3");
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
    public static readonly string SUPPLY_FORWARD_L1 = GetValue("RequestStrings:Supply:ForwardL1");
    public static readonly string SUPPLY_FORWARD_L2 = GetValue("RequestStrings:Supply:ForwardL2");
    public static readonly string SUPPLY_FORWARD_L3 = GetValue("RequestStrings:Supply:ForwardL3");
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
    public static readonly string SUPPLY_BACKWARD_L1 = GetValue("RequestStrings:Supply:BackwardL1");
    public static readonly string SUPPLY_BACKWARD_L2 = GetValue("RequestStrings:Supply:BackwardL2");
    public static readonly string SUPPLY_BACKWARD_L3 = GetValue("RequestStrings:Supply:BackwardL3");
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
    public static readonly string SUPPLY_RESET_L1 = GetValue("RequestStrings:Supply:ResetL1");
    public static readonly string SUPPLY_RESET_L2 = GetValue("RequestStrings:Supply:ResetL2");
    public static readonly string SUPPLY_RESET_L3 = GetValue("RequestStrings:Supply:ResetL3");
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
    public static readonly string SUPPLY_BACKWARD_SENSOR = GetValue("RequestStrings:Supply:BackwardSensor");
    public static readonly string SUPPLY_RESET_SENSOR = GetValue("RequestStrings:Supply:ResetSensor");
    public static readonly string SUPPLY_ACCELERATION = GetValue("RequestString:Supply:Acceleration");
    public static readonly string SUPPLY_BRAKING = GetValue("RequestString:Supply:Braking");
    public static readonly string SUPPLY_JERK = GetValue("RequestString:Supply:Jerk");

    // ÏÎÂÎÐÎÒ
    public static readonly string ROTATION_OFFSET = GetValue("RequestStrings:Rotation:Offset");
    public static readonly string ROTATION_COEFFICIENT = GetValue("RequestStrings:Rotation:Coefficient");
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L1 = GetValue("RequestStrings:Rotation:OffsetAfterZeroSearchL1");
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L2 = GetValue("RequestStrings:Rotation:OffsetAfterZeroSearchL2");
    public static readonly string ROTATION_OFFSET_AFTER_ZERO_SEARCH_L3 = GetValue("RequestStrings:Rotation:OffsetAfterZeroSearchL3");
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
    public static readonly string ROTATION_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Rotation:SpeedCoefficientL1");
    public static readonly string ROTATION_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Rotation:SpeedCoefficientL2");
    public static readonly string ROTATION_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Rotation:SpeedCoefficientL3");
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
    public static readonly string ROTATION_VALUE = GetValue("RequestStrings:Rotation:Value");
    public static readonly string ROTATION_FACTICAL_POSITION = GetValue("RequestStrings:Rotation:Factical");
    public static readonly string ROTATION_RESET_SENSOR = GetValue("RequestStrings:Rotation:ResetSensor");
    public static readonly string ROTATION_BRAKING_OUTPUT_SIGNAL = GetValue("RequestStrings:Rotation:BrakingOutputSignal");
    public static readonly string ROTATION_ACCELERATION = GetValue("RequestString:Rotation:Acceleration");
    public static readonly string ROTATION_BRAKING = GetValue("RequestString:Rotation:Braking");
    public static readonly string ROTATION_JERK = GetValue("RequestString:Rotation:Jerk");

    // ÊÎÍÑÎËÜ
    public static readonly string CONSOLE_FORWARD = GetValue("RequestStrings:Console:Forward");
    public static readonly string CONSOLE_BACKWARD = GetValue("RequestStrings:Console:Backward");
    public static readonly string CONSOLE_ACTUAL_COORDINATE = GetValue("RequestStrings:Console:ActualCoordinate");
    public static readonly string CONSOLE_ACTUAL_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Console:ActualRelativeDisplacement");
    public static readonly string CONSOLE_RESET = GetValue("RequestStrings:Console:Reset");
    public static readonly string CONSOLE_SPEED = GetValue("RequestStrings:Console:Speed");
    public static readonly string CONSOLE_TORQUE = GetValue("RequestStrings:Console:Torque");
    public static readonly string CONSOLE_REAR_POSITION = GetValue("RequestStrings:Console:RearPosition");
    public static readonly string CONSOLE_FRONT_POSITION = GetValue("RequestStrings:Console:FrontPosition");
    public static readonly string CONSOLE_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Console:RelativeDisplacement");
    public static readonly string CONSOLE_COEFFICIENT = GetValue("RequestStrings:Console:Coefficient");
    public static readonly string CONSOLE_BEND_POSITION_L1 = GetValue("RequestStrings:Console:BendPositionL1");
    public static readonly string CONSOLE_BEND_POSITION_L2 = GetValue("RequestStrings:Console:BendPositionL2");
    public static readonly string CONSOLE_BEND_POSITION_L3 = GetValue("RequestStrings:Console:BendPositionL3");
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
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L1 = GetValue("RequestStrings:Console:SecondFloorPositionL1");
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L2 = GetValue("RequestStrings:Console:SecondFloorPositionL2");
    public static readonly string CONSOLE_SECOND_FLOOR_POSITION_L3 = GetValue("RequestStrings:Console:SecondFloorPositionL3");
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
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L1 = GetValue("RequestStrings:Console:SecondFloorIntermediatePositionL1");
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L2 = GetValue("RequestStrings:Console:SecondFloorIntermediatePositionL2");
    public static readonly string CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION_L3 = GetValue("RequestStrings:Console:SecondFloorIntermediatePositionL3");
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
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L1 = GetValue("RequestStrings:Console:ThirdFloorPositionL1");
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L2 = GetValue("RequestStrings:Console:ThirdFloorPositionL2");
    public static readonly string CONSOLE_THIRD_FLOOR_POSITION_L3 = GetValue("RequestStrings:Console:ThirdFloorPositionL3");
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
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L1 = GetValue("RequestStrings:Console:PipeRotationDepartureDistanceL1");
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L2 = GetValue("RequestStrings:Console:PipeRotationDepartureDistanceL2");
    public static readonly string CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE_L3 = GetValue("RequestStrings:Console:PipeRotationDepartureDistanceL3");
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
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Console:SpeedCoefficientL1");
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Console:SpeedCoefficientL2");
    public static readonly string CONSOLE_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Console:SpeedCoefficientL3");
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
    public static readonly string CONSOLE_FACTICAL_POSITION = GetValue("AutomaticTags:Factical:Console");
    public static readonly string CONSOLE_CURRENT_POSITION_L1 = GetValue("RequestStrings:Console:CurrentPositionL1");
    public static readonly string CONSOLE_CURRENT_POSITION_L2 = GetValue("RequestStrings:Console:CurrentPositionL2");
    public static readonly string CONSOLE_CURRENT_POSITION_L3 = GetValue("RequestStrings:Console:CurrentPositionL3");
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
    public static readonly string CONSOLE_FORWARD_L1 = GetValue("RequestStrings:Console:ForwardL1");
    public static readonly string CONSOLE_FORWARD_L2 = GetValue("RequestStrings:Console:ForwardL2");
    public static readonly string CONSOLE_FORWARD_L3 = GetValue("RequestStrings:Console:ForwardL3");
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
    public static readonly string CONSOLE_BACKWARD_L1 = GetValue("RequestStrings:Console:BackwardL1");
    public static readonly string CONSOLE_BACKWARD_L2 = GetValue("RequestStrings:Console:BackwardL2");
    public static readonly string CONSOLE_BACKWARD_L3 = GetValue("RequestStrings:Console:BackwardL3");
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
    public static readonly string CONSOLE_RESET_L1 = GetValue("RequestStrings:Console:ResetL1");
    public static readonly string CONSOLE_RESET_L2 = GetValue("RequestStrings:Console:ResetL2");
    public static readonly string CONSOLE_RESET_L3 = GetValue("RequestStrings:Console:ResetL3");
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
    public static readonly string CONSOLE_FORWARD_SENSOR = GetValue("RequestStrings:Console:ForwardSensor");
    public static readonly string CONSOLE_BACKWARD_SENSOR = GetValue("RequestStrings:Console:BackwardSensor");
    public static readonly string CONSOLE_OUTLET_FOR_PIPE_INSTALLING = GetValue("RequestStrings:Console:OutletForPipeInstalling");
    public static readonly string CONSOLE_ACCELERATION = GetValue("RequestString:Console:Acceleration");
    public static readonly string CONSOLE_BRAKING = GetValue("RequestString:Console:Braking");
    public static readonly string CONSOLE_JERK = GetValue("RequestString:Console:Jerk");

    // ÃÈÁ
    public static readonly string BEND_FORWARD = GetValue("RequestStrings:Bend:Forward");
    public static readonly string BEND_BACKWARD = GetValue("RequestStrings:Bend:Backward");
    public static readonly string BEND_ACTUAL_COORDINATE = GetValue("RequestStrings:Bend:ActualCoordinate");
    public static readonly string BEND_ACTUAL_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Bend:ActualRelativeDisplacement");
    public static readonly string BEND_RESET = GetValue("RequestStrings:Bend:Reset");
    public static readonly string BEND_SPEED = GetValue("RequestStrings:Bend:Speed");
    public static readonly string BEND_TORQUE = GetValue("RequestStrings:Bend:Torque");
    public static readonly string BEND_REAR_POSITION = GetValue("RequestStrings:Bend:RearPosition");
    public static readonly string BEND_FRONT_POSITION = GetValue("RequestStrings:Bend:FrontPosition");
    public static readonly string BEND_RELATIVE_DISPLACEMENT = GetValue("RequestStrings:Bend:RelativeDisplacement");
    public static readonly string BEND_FORWARD_BUTTON = GetValue("RequestStrings:Bend:Forward");
    public static readonly string BEND_BACKWARD_BUTTON = GetValue("RequestStrings:Bend:BackwardButton");
    public static readonly string BEND_COEFFICIENT = GetValue("RequestStrings:Bend:Coefficient");
    public static readonly string BEND_SYNCHRONIZATION = GetValue("RequestStrings:Bend:Synchronization");
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L1 = GetValue("RequestStrings:Bend:ForwardPositionLimitationL1");
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L2 = GetValue("RequestStrings:Bend:ForwardPositionLimitationL2");
    public static readonly string BEND_FORWARD_POSITION_LIMITATION_L3 = GetValue("RequestStrings:Bend:ForwardPositionLimitationL3");
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
    public static readonly string BEND_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Bend:SpeedCoefficientL1");
    public static readonly string BEND_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Bend:SpeedCoefficientL2");
    public static readonly string BEND_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Bend:SpeedCoefficientL3");
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
    public static readonly string BEND_SLOWDOWN_SPEED_L1 = GetValue("RequestStrings:Bend:SlowdownSpeedL1");
    public static readonly string BEND_SLOWDOWN_SPEED_L2 = GetValue("RequestStrings:Bend:SlowdownSpeedL2");
    public static readonly string BEND_SLOWDOWN_SPEED_L3 = GetValue("RequestStrings:Bend:SlowdownSpeedL3");
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
    public static readonly string BEND_VALUE = GetValue("RequestStrings:Bend:Value");
    public static readonly string BEND_FACTICAL_POSITION = GetValue("RequestStrings:Bend:FacticalPosition");
    public static readonly string BEND_FORWARD_SENSOR = GetValue("RequestStrings:Bend:ForwardSensor");
    public static readonly string BEND_BACKWARD_SENSOR = GetValue("RequestStrings:Bend:BackwardSensor");
    public static readonly string BEND_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Bend:ForwardOutputSignal");
    public static readonly string BEND_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Bend:BackwardOutputSignal");
    public static readonly string BEND_ACCELERATION = GetValue("RequestString:Bend:Acceleration");
    public static readonly string BEND_BRAKING = GetValue("RequestString:Bend:Braking");
    public static readonly string BEND_JERK = GetValue("RequestString:Bend:Jerk");

    // ÇÀÆÈÌ
    public static readonly string CLAMP_FORWARD = GetValue("RequestStrings:Clamp:Forward");
    public static readonly string CLAMP_BACKWARD = GetValue("RequestStrings:Clamp:Backward");
    public static readonly string CLAMP_REAR_POSITION = GetValue("RequestStrings:Clamp:RearPosition");
    public static readonly string CLAMP_FRONT_POSITION = GetValue("RequestStrings:Clamp:FrontPosition");
    public static readonly string CLAMP_DEEP_L1 = GetValue("RequestStrings:Clamp:DeepL1");
    public static readonly string CLAMP_DEEP_L2 = GetValue("RequestStrings:Clamp:DeepL2");
    public static readonly string CLAMP_DEEP_L3 = GetValue("RequestStrings:Clamp:DeepL3");
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
    public static readonly string CLAMP_LENGTH_L1 = GetValue("RequestStrings:Clamp:LengthL1");
    public static readonly string CLAMP_LENGTH_L2 = GetValue("RequestStrings:Clamp:LengthL2");
    public static readonly string CLAMP_LENGTH_L3 = GetValue("RequestStrings:Clamp:LengthL3");
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
    public static readonly string CLAMP_FORWARD_POSITION_L1 = GetValue("RequestStrings:Clamp:ForwardPositionL1");
    public static readonly string CLAMP_FORWARD_POSITION_L2 = GetValue("RequestStrings:Clamp:ForwardPositionL2");
    public static readonly string CLAMP_FORWARD_POSITION_L3 = GetValue("RequestStrings:Clamp:ForwardPositionL3");
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
    public static readonly string CLAMP_MIDDLE_POSITION_L1 = GetValue("RequestStrings:Clamp:MiddlePositionL1");
    public static readonly string CLAMP_MIDDLE_POSITION_L2 = GetValue("RequestStrings:Clamp:MiddlePositionL2");
    public static readonly string CLAMP_MIDDLE_POSITION_L3 = GetValue("RequestStrings:Clamp:MiddlePositionL3");
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
    public static readonly string CLAMP_BACKWARD_POSITION_L1 = GetValue("RequestStrings:Clamp:BackwardPositionL1");
    public static readonly string CLAMP_BACKWARD_POSITION_L2 = GetValue("RequestStrings:Clamp:BackwardPositionL2");
    public static readonly string CLAMP_BACKWARD_POSITION_L3 = GetValue("RequestStrings:Clamp:BackwardPositionL3");
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
    public static readonly string CLAMP_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Clamp:SpeedCoefficientL1");
    public static readonly string CLAMP_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Clamp:SpeedCoefficientL2");
    public static readonly string CLAMP_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Clamp:SpeedCoefficientL3");
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
    public static readonly string CLAMP_CURRENT_POSITION_L1 = GetValue("RequestStrings:Clamp:CurrentPositionL1");
    public static readonly string CLAMP_CURRENT_POSITION_L2 = GetValue("RequestStrings:Clamp:CurrentPositionL2");
    public static readonly string CLAMP_CURRENT_POSITION_L3 = GetValue("RequestStrings:Clamp:CurrentPositionL3");
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
    public static readonly string CLAMP_FORWARD_L1 = GetValue("RequestStrings:Clamp:ForwardL1");
    public static readonly string CLAMP_FORWARD_L2 = GetValue("RequestStrings:Clamp:ForwardL2");
    public static readonly string CLAMP_FORWARD_L3 = GetValue("RequestStrings:Clamp:ForwardL3");
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
    public static readonly string CLAMP_BACKWARD_L1 = GetValue("RequestStrings:Clamp:BackwardL1");
    public static readonly string CLAMP_BACKWARD_L2 = GetValue("RequestStrings:Clamp:BackwardL2");
    public static readonly string CLAMP_BACKWARD_L3 = GetValue("RequestStrings:Clamp:BackwardL3");
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
    public static readonly string CLAMP_RESET_L1 = GetValue("RequestStrings:Clamp:ResetL1");
    public static readonly string CLAMP_RESET_L2 = GetValue("RequestStrings:Clamp:ResetL2");
    public static readonly string CLAMP_RESET_L3 = GetValue("RequestStrings:Clamp:ResetL3");
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
    public static readonly string CLAMP_FORWARD_SENSOR = GetValue("RequestStrings:Clamp:ForwardSensor");
    public static readonly string CLAMP_BACKWARD_SENSOR = GetValue("RequestStrings:Clamp:BackwardSensor");
    public static readonly string CLAMP_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Clamp:ForwardOutputSignal");
    public static readonly string CLAMP_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Clamp:BackwardOutputSignal");
    public static readonly string CLAMP_INVERT_SENSORS = GetValue("RequestStrings:Clamp:InvertSensors");

    // ÏÐÈÆÈÌ
    public static readonly string PRESS_FORWARD = GetValue("RequestStrings:Press:Forward");
    public static readonly string PRESS_BACKWARD = GetValue("RequestStrings:Press:Backward");
    public static readonly string PRESS_REAR_POSITION = GetValue("RequestStrings:Press:RearPosition");
    public static readonly string PRESS_FRONT_POSITION = GetValue("RequestStrings:Press:FrontPosition");
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L1 = GetValue("RequestStrings:Press:DangerZoneCoordinateL1");
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L2 = GetValue("RequestStrings:Press:DangerZoneCoordinateL2");
    public static readonly string PRESS_DANGER_ZONE_COORDINATE_L3 = GetValue("RequestStrings:Press:DangerZoneCoordinateL3");
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
    public static readonly string PRESS_LENGTH_L1 = GetValue("RequestStrings:Press:LengthL1");
    public static readonly string PRESS_LENGTH_L2 = GetValue("RequestStrings:Press:LengthL2");
    public static readonly string PRESS_LENGTH_L3 = GetValue("RequestStrings:Press:LengthL3");
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
    public static readonly string PRESS_FORWARD_POSITION_L1 = GetValue("RequestStrings:Press:ForwardPositionL1");
    public static readonly string PRESS_FORWARD_POSITION_L2 = GetValue("RequestStrings:Press:ForwardPositionL2");
    public static readonly string PRESS_FORWARD_POSITION_L3 = GetValue("RequestStrings:Press:ForwardPositionL3");
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
    public static readonly string PRESS_MIDDLE_POSITION_L1 = GetValue("RequestStrings:Press:MiddlePositionL1");
    public static readonly string PRESS_MIDDLE_POSITION_L2 = GetValue("RequestStrings:Press:MiddlePositionL2");
    public static readonly string PRESS_MIDDLE_POSITION_L3 = GetValue("RequestStrings:Press:MiddlePositionL3");
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
    public static readonly string PRESS_BACKWARD_POSITION_L1 = GetValue("RequestStrings:Press:BackwardPositionL1");
    public static readonly string PRESS_BACKWARD_POSITION_L2 = GetValue("RequestStrings:Press:BackwardPositionL2");
    public static readonly string PRESS_BACKWARD_POSITION_L3 = GetValue("RequestStrings:Press:BackwardPositionL3");
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
    public static readonly string PRESS_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Press:SpeedCoefficientL1");
    public static readonly string PRESS_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Press:SpeedCoefficientL2");
    public static readonly string PRESS_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Press:SpeedCoefficientL3");
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
    public static readonly string PRESS_CURRENT_POSITION_L1 = GetValue("RequestStrings:Press:CurrentPositionL1");
    public static readonly string PRESS_CURRENT_POSITION_L2 = GetValue("RequestStrings:Press:CurrentPositionL2");
    public static readonly string PRESS_CURRENT_POSITION_L3 = GetValue("RequestStrings:Press:CurrentPositionL3");
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
    public static readonly string PRESS_FORWARD_L1 = GetValue("RequestStrings:Press:ForwardL1");
    public static readonly string PRESS_FORWARD_L2 = GetValue("RequestStrings:Press:ForwardL2");
    public static readonly string PRESS_FORWARD_L3 = GetValue("RequestStrings:Press:ForwardL3");
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
    public static readonly string PRESS_BACKWARD_L1 = GetValue("RequestStrings:Press:BackwardL1");
    public static readonly string PRESS_BACKWARD_L2 = GetValue("RequestStrings:Press:BackwardL2");
    public static readonly string PRESS_BACKWARD_L3 = GetValue("RequestStrings:Press:BackwardL3");
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
    public static readonly string PRESS_RESET_L1 = GetValue("RequestStrings:Press:ResetL1");
    public static readonly string PRESS_RESET_L2 = GetValue("RequestStrings:Press:ResetL2");
    public static readonly string PRESS_RESET_L3 = GetValue("RequestStrings:Press:ResetL3");
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
    public static readonly string PRESS_FORWARD_SENSOR = GetValue("RequestStrings:Press:ForwardSensor");
    public static readonly string PRESS_BACKWARD_SENSOR = GetValue("RequestStrings:Press:BackwardSensor");
    public static readonly string PRESS_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Press:ForwardOutputSignal");
    public static readonly string PRESS_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Press:BackwardOutputSignal");
    public static readonly string PRESS_INCOMPLETE_MOVEMENT = GetValue("RequestStrings:Press:IncompleteMovement");

    // ÏÅÐÂÛÉ ÄÎÆÈÌ
    public static readonly string FIRST_SQUEEZE_FORWARD = GetValue("RequestStrings:FirstSqueeze:Forward");
    public static readonly string FIRST_SQUEEZE_BACKWARD = GetValue("RequestStrings:FirstSqueeze:Backward");
    public static readonly string FIRST_SQUEEZE_REAR_POSITION = GetValue("RequestStrings:FirstSqueeze:RearPosition");
    public static readonly string FIRST_SQUEEZE_FRONT_POSITION = GetValue("RequestStrings:FirstSqueeze:FrontPosition");
    public static readonly string FIRST_SQUEEZE_REAR_SECOND_POSITION = GetValue("RequestStrings:FirstSqueeze:RearSecondPosition");
    public static readonly string FIRST_SQUEEZE_FRONT_SECOND_POSITION = GetValue("RequestStrings:FirstSqueeze:FrontSecondPosition");

    // ÖÀÍÃÀ
    public static readonly string COLLET_FORWARD = GetValue("RequestStrings:Collet:Forward");
    public static readonly string COLLET_BACKWARD = GetValue("RequestStrings:Collet:Backward");
    public static readonly string COLLET_REAR_POSITION = GetValue("RequestStrings:Collet:RearPosition");
    public static readonly string COLLET_FRONT_POSITION = GetValue("RequestStrings:Collet:FrontPosition");
    public static readonly string COLLET_FORWARD_SENSOR = GetValue("RequestStrings:Collet:ForwardSensor");
    public static readonly string COLLET_BACKWARD_SENSOR = GetValue("RequestStrings:Collet:BackwardSensor");
    public static readonly string COLLET_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:ForwardOutputSignal");
    public static readonly string COLLET_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:BackwardOutputSignal");

    // ÄÎÐÍ
    public static readonly string DORN_FORWARD = GetValue("RequestStrings:Dorn:Forward");
    public static readonly string DORN_BACKWARD = GetValue("RequestStrings:Dorn:Backward");
    public static readonly string DORN_REAR_POSITION = GetValue("RequestStrings:Dorn:RearPosition");
    public static readonly string DORN_FRONT_POSITION = GetValue("RequestStrings:Dorn:FrontPosition");
    public static readonly string DORN_AUTOMATIC = GetValue("RequestStrings:Dorn:Automatic");
    public static readonly string DORN_LEAD_WITHDRAWAL_BEFORE_BEND = GetValue("RequestStrings:Dorn:LeadWithdrawalBeforeBend");
    public static readonly string DORN_FORWARD_POSITION_L1 = GetValue("RequestStrings:Dorn:ForwardPositionL1");
    public static readonly string DORN_FORWARD_POSITION_L2 = GetValue("RequestStrings:Dorn:ForwardPositionL2");
    public static readonly string DORN_FORWARD_POSITION_L3 = GetValue("RequestStrings:Dorn:ForwardPositionL3");
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
    public static readonly string DORN_MIDDLE_POSITION_L1 = GetValue("RequestStrings:Dorn:MiddlePositionL1");
    public static readonly string DORN_MIDDLE_POSITION_L2 = GetValue("RequestStrings:Dorn:MiddlePositionL2");
    public static readonly string DORN_MIDDLE_POSITION_L3 = GetValue("RequestStrings:Dorn:MiddlePositionL3");
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
    public static readonly string DORN_BACKWARD_POSITION_L1 = GetValue("RequestStrings:Dorn:BackwardPositionL1");
    public static readonly string DORN_BACKWARD_POSITION_L2 = GetValue("RequestStrings:Dorn:BackwardPositionL2");
    public static readonly string DORN_BACKWARD_POSITION_L3 = GetValue("RequestStrings:Dorn:BackwardPositionL3");
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
    public static readonly string DORN_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Dorn:SpeedCoefficientL1");
    public static readonly string DORN_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Dorn:SpeedCoefficientL2");
    public static readonly string DORN_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Dorn:SpeedCoefficientL3");
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
    public static readonly string DORN_CURRENT_POSITION_L1 = GetValue("RequestStrings:Dorn:CurrentPositionL1");
    public static readonly string DORN_CURRENT_POSITION_L2 = GetValue("RequestStrings:Dorn:CurrentPositionL2");
    public static readonly string DORN_CURRENT_POSITION_L3 = GetValue("RequestStrings:Dorn:CurrentPositionL3");
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
    public static readonly string DORN_FORWARD_L1 = GetValue("RequestStrings:Dorn:ForwardL1");
    public static readonly string DORN_FORWARD_L2 = GetValue("RequestStrings:Dorn:ForwardL2");
    public static readonly string DORN_FORWARD_L3 = GetValue("RequestStrings:Dorn:ForwardL3");
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
    public static readonly string DORN_BACKWARD_L1 = GetValue("RequestStrings:Dorn:BackwardL1");
    public static readonly string DORN_BACKWARD_L2 = GetValue("RequestStrings:Dorn:BackwardL2");
    public static readonly string DORN_BACKWARD_L3 = GetValue("RequestStrings:Dorn:BackwardL3");
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
    public static readonly string DORN_RESET_L1 = GetValue("RequestStrings:Dorn:ResetL1");
    public static readonly string DORN_RESET_L2 = GetValue("RequestStrings:Dorn:ResetL2");
    public static readonly string DORN_RESET_L3 = GetValue("RequestStrings:Dorn:ResetL3");
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
    public static readonly string DORN_FORWARD_SENSOR = GetValue("RequestStrings:Dorn:ForwardSensor");
    public static readonly string DORN_BACKWARD_SENSOR = GetValue("RequestStrings:Dorn:BackwardSensor");
    public static readonly string DORN_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Dorn:ForwardOutputSignal");
    public static readonly string DORN_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Dorn:BackwardOutputSignal");

    // ÎÑÍÀÑÒÊÀ
    public static readonly string ADJUSTMENT_FORWARD = GetValue("RequestStrings:Adjustment:Forward");
    public static readonly string ADJUSTMENT_BACKWARD = GetValue("RequestStrings:Adjustment:Backward");
    public static readonly string ADJUSTMENT_REAR_POSITION = GetValue("RequestStrings:Adjustment:RearPosition");
    public static readonly string ADJUSTMENT_FRONT_POSITION = GetValue("RequestStrings:Adjustment:CenterPosition");
    public static readonly string ADJUSTMENT_CENTER_POSITION = GetValue("RequestStrings:Adjustment:FrontPosition");
    public static readonly string ADJUSTMENT_TYPE_L1 = GetValue("RequestStrings:Adjustment:AdjustmentTypeL1");
    public static readonly string ADJUSTMENT_TYPE_L2 = GetValue("RequestStrings:Adjustment:AdjustmentTypeL2");
    public static readonly string ADJUSTMENT_TYPE_L3 = GetValue("RequestStrings:Adjustment:AdjustmentTypeL3");
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
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L1 = GetValue("RequestStrings:Adjustment:PipeDiameterL1");
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L2 = GetValue("RequestStrings:Adjustment:PipeDiameterL2");
    public static readonly string ADJUSTMENT_PIPE_DIAMETER_L3 = GetValue("RequestStrings:Adjustment:PipeDiameterL3");
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
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L1 = GetValue("RequestStrings:Adjustment:ForwardDangerZoneCoordinateL1");
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L2 = GetValue("RequestStrings:Adjustment:ForwardDangerZoneCoordinateL2");
    public static readonly string ADJUSTMENT_FORWARD_DANGER_ZONE_COORDINATE_L3 = GetValue("RequestStrings:Adjustment:ForwardDangerZoneCoordinateL3");
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
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L1 = GetValue("RequestStrings:Adjustment:DistanceFromCenterL1");
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L2 = GetValue("RequestStrings:Adjustment:DistanceFromCenterL2");
    public static readonly string ADJUSTMENT_DISTANCE_FROM_CENTER_L3 = GetValue("RequestStrings:Adjustment:DistanceFromCenterL3");
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
    public static readonly string ADJUSTMENT_UPPER_SENSOR = GetValue("RequestStrings:Adjustment:UpperSensor");
    public static readonly string ADJUSTMENT_MIDDLE_SENSOR = GetValue("RequestStrings:Adjustment:MiddleSensor");
    public static readonly string ADJUSTMENT_LOWER_SENSOR = GetValue("RequestStrings:Adjustment:LowerSensor");
    public static readonly string ADJUSTMENT_UP_OUTPUT_SIGNAL = GetValue("RequestStrings:Adjustment:UpOutputSignal");
    public static readonly string ADJUSTMENT_DOWN_OUTPUT_SIGNAL = GetValue("RequestStrings:Adjustment:DownOutputSignal");

    // ÏÐÎÁÈÂÊÀ
    public static readonly string PUNCHING_FORWARD = GetValue("RequestStrings:Punching:Forward");
    public static readonly string PUNCHING_BACKWARD = GetValue("RequestStrings:Punching:Backward");
    public static readonly string PUNCHING_REAR_POSITION = GetValue("RequestStrings:Punching:RearPosition");
    public static readonly string PUNCHING_FRONT_POSITION = GetValue("RequestStrings:Punching:FrontPosition");
    public static readonly string PUNCHING_FORWARD_SENSOR = GetValue("RequestStrings:Punching:ForwardSensor");
    public static readonly string PUNCHING_BACKWARD_SENSOR = GetValue("RequestStrings:Punching:BackwardSensor");
    public static readonly string PUNCHING_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Punching:ForwardOutputSignal");
    public static readonly string PUNCHING_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Punching:BackwardOutputSignal");

    // ÃÈÄÐÀÂËÈÊÀ
    public static readonly string FIRST_HYDRAULICS_VALUE = GetValue("RequestStrings:FirstHydraulics:Value");
    public static readonly string SECOND_HYDRAULICS_VALUE = GetValue("RequestStrings:SecondHydraulics:Value");
    public static readonly string FIRST_HYDRAULICS_OUTPUT_SIGNAL = GetValue("RequestStrings:FirstHydraulics:OutputSignal");
    public static readonly string SECOND_HYDRAULICS_OUTPUT_SIGNAL = GetValue("RequestStrings:SecondHydraulics:OutputSignal");
    public static readonly string HYDRAULICS_MOVEMENT_WITHOUT_SENSORS = GetValue("RequestStrings:Hydraulics:MovementWithoutSensors");

    // ÏÎÄÄÅÐÆÊÀ
    public static readonly string SUPPORT_VALUE = GetValue("RequestStrings:Support:Value");
    public static readonly string SUPPORT_FRONT_LIFT_BAN = GetValue("RequestStrings:Support:FrontLiftBan");
    public static readonly string SUPPORT_MIDDLE_LIFT_BAN = GetValue("RequestStrings:Support:MiddleLiftBan");
    public static readonly string SUPPORT_BACK_LIFT_BAN = GetValue("RequestStrings:Support:BackLiftBan");
    public static readonly string SUPPORT_LOWER_SENSOR = GetValue("RequestStrings:Support:LowerSensor");
    public static readonly string FIRST_SUPPORT_UP_OUTPUT_SIGNAL = GetValue("RequestStrings:Support:FirstUpOutputSignal");
    public static readonly string SECOND_SUPPORT_UP_OUTPUT_SIGNAL = GetValue("RequestStrings:Support:SecondUpOutputSignal");
    public static readonly string THIRD_SUPPORT_UP_OUTPUT_SIGNAL = GetValue("RequestStrings:Support:ThirdUpOutputSignal");
    public static readonly string FOURTH_SUPPORT_UP_OUTPUT_SIGNAL = GetValue("RequestStrings:Support:ForthUpSignal");
    public static readonly string SUPPORT_FIRST_COORDINATE_BAN = GetValue("RequestStrings:Support:FirstCoordinateBan");
    public static readonly string SUPPORT_SECOND_COORDINATE_BAN = GetValue("RequestStrings:Support:SecondCoordinateBan");
    public static readonly string SUPPORT_THIRD_FORWARD_COORDINATE_BAN = GetValue("RequestStrings:Support:ThirdForwardCoordinateBan");
    public static readonly string SUPPORT_THIRD_BACKWARD_COORDINATE_BAN = GetValue("RequestStrings:Support:ThirdBackwardCoordinateBan");
    public static readonly string SUPPORT_FOURTH_COORDINATE_BAN = GetValue("RequestStrings:Support:FourthCoordinateBan");
    public static readonly string SUPPORT_BAN_PRESS = GetValue("RequestStrings:Support:BanPress");

    // ÑÌÀÇÊÀ ÄÎÐÍÀ
    public static readonly string DORN_LUBRICANT_TURN_ON = GetValue("RequestStrings:DornLubricant:TurnOn");
    public static readonly string DORN_LUBRICANT_LUBRICANT_TURN_ON = GetValue("RequestStrings:DornLubricant:LubricantTurnOn");
    public static readonly string DORN_LUBRICANT_OUTPUT_SIGNAL = GetValue("RequestStrings:DornLubricant:OutputSignal");

    // ÃÈÁ È ÄÎÆÈÌ
    public static readonly string BEND_AND_SQUEEZE_VALUE = GetValue("RequestStrings:BendAndSqueeze:Value");

    // ÃÈÁ È ÏÎÄÀ×À
    public static readonly string BEND_AND_SUPPLY_PUSHING_ENABLE = GetValue("RequestStrings:BendAndSupply:PushingEnable");
    public static readonly string BEND_AND_SUPPLY_COEFFICIENT = GetValue("RequestStrings:BendAndSupply:Coefficient");
    public static readonly string BEND_AND_SUPPLY_SYNCHRONIZATION = GetValue("RequestStrings:BendAndSupply:Synchronization");

    // ÎØÈÁÊÈ
    public static readonly string ERRORS_CLEAR_ACTUATOR_ERRORS = GetValue("RequestStrings:Errors:ClearActuatorErrors");
    public static readonly string ERRORS_HAS_ERRORS = GetValue("RequestStrings:Errors:StopErrors");

    // ÒÐÓÁÀ
    public static readonly string PIPE_OUTLET_COORDINATE = GetValue("RequestStrings:Pipe:OutletCoordinate");
    public static readonly string PIPE_INSTALLATION_COORDINATE = GetValue("RequestStrings:Pipe:InstallationCoordinate");
    public static readonly string PIPE_LENGTH = GetValue("RequestStrings:Pipe:Length");

    // ÃÈÁÎ×ÍÛÉ ÐÎËÈÊ
    public static readonly string BEND_ROLLER_RADIUS_L1 = GetValue("RequestStrings:BendRoller:RadiusL1");
    public static readonly string BEND_ROLLER_RADIUS_L2 = GetValue("RequestStrings:BendRoller:RadiusL2");
    public static readonly string BEND_ROLLER_RADIUS_L3 = GetValue("RequestStrings:BendRoller:RadiusL3");
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
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L1 = GetValue("RequestStrings:BendRoller:OuterRadiusL1");
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L2 = GetValue("RequestStrings:BendRoller:OuterRadiusL2");
    public static readonly string BEND_ROLLER_OUTER_RADIUS_L3 = GetValue("RequestStrings:BendRoller:OuterRadiusL3");
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

    // ÇÀÆÈÌÍÎÉ ÐÎËÈÊ
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L1 = GetValue("RequestStrings:ClampRoller:OuterRadiusL1");
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L2 = GetValue("RequestStrings:ClampRoller:OuterRadiusL2");
    public static readonly string CLAMP_ROLLER_OUTER_RADIUS_L3 = GetValue("RequestStrings:ClampRoller:OuterRadiusL3");
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
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L1 = GetValue("RequestStrings:ClampRoller:InnerRadiusL1");
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L2 = GetValue("RequestStrings:ClampRoller:InnerRadiusL2");
    public static readonly string CLAMP_ROLLER_INNER_RADIUS_L3 = GetValue("RequestStrings:ClampRoller:InnerRadiusL3");
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

    // ÄÎÆÈÌ
    public static readonly string SQUEEZE_TURN_ON_L1 = GetValue("RequestStrings:Squeeze:TurnOnL1");
    public static readonly string SQUEEZE_TURN_ON_L2 = GetValue("RequestStrings:Squeeze:TurnOnL2");
    public static readonly string SQUEEZE_TURN_ON_L3 = GetValue("RequestStrings:Squeeze:TurnOnL3");
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
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L1 = GetValue("RequestStrings:Squeeze:FrontPositionLimitationL1");
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L2 = GetValue("RequestStrings:Squeeze:FrontPositionLimitationL2");
    public static readonly string SQUEEZE_FRONT_POSITION_LIMITATION_L3 = GetValue("RequestStrings:Squeeze:FrontPositionLimitationL3");
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
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Squeeze:SpeedCoefficientL1");
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Squeeze:SpeedCoefficientL2");
    public static readonly string SQUEEZE_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Squeeze:SpeedCoefficientL3");
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
    public static readonly string FIRST_SQUEEZE_FORWARD_SENSOR = GetValue("RequestStrings:Squeeze:FirstForwardSensor");
    public static readonly string FIRST_SQUEEZE_BACKWARD_SENSOR = GetValue("RequestStrings:Squeeze:FirstBackwardSensor");
    public static readonly string SECOND_SQUEEZE_FORWARD_SENSOR = GetValue("RequestStrings:Squeeze:SecondForwardSensor");
    public static readonly string SECOND_SQUEEZE_BACKWARD_SENSOR = GetValue("RequestStrings:Squeeze:SecondBackwardSensor");
    public static readonly string FIRST_SQUEEZE_FORWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Squeeze:FirstForwardOutputSignal");
    public static readonly string FIRST_SQUEEZE_BACKWARD_OUTPUT_SIGNAL = GetValue("RequestStrings:Squeeze:FirstBackwardOutputSignal");
    public static readonly string SQUEEZE_WORK_TIME = GetValue("RequestStrings:Squeeze:WorkTime");

    // ÏÎÄÚ¨Ì
    public static readonly string LIFT_UPPER_POSITION_L1 = GetValue("RequestStrings:Lift:UpperPositionL1");
    public static readonly string LIFT_UPPER_POSITION_L2 = GetValue("RequestStrings:Lift:UpperPositionL2");
    public static readonly string LIFT_UPPER_POSITION_L3 = GetValue("RequestStrings:Lift:UpperPositionL3");
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
    public static readonly string LIFT_MIDDLE_POSITION_L1 = GetValue("RequestStrings:Lift:MiddlePositionL1");
    public static readonly string LIFT_MIDDLE_POSITION_L2 = GetValue("RequestStrings:Lift:MiddlePositionL2");
    public static readonly string LIFT_MIDDLE_POSITION_L3 = GetValue("RequestStrings:Lift:MiddlePositionL3");
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
    public static readonly string LIFT_LOWER_POSITION_L1 = GetValue("RequestStrings:Lift:LowerPositionL1");
    public static readonly string LIFT_LOWER_POSITION_L2 = GetValue("RequestStrings:Lift:LowerPositionL2");
    public static readonly string LIFT_LOWER_POSITION_L3 = GetValue("RequestStrings:Lift:LowerPositionL3");
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
    public static readonly string LIFT_SPEED_COEFFICIENT_L1 = GetValue("RequestStrings:Lift:SpeedCoefficientL1");
    public static readonly string LIFT_SPEED_COEFFICIENT_L2 = GetValue("RequestStrings:Lift:SpeedCoefficientL2");
    public static readonly string LIFT_SPEED_COEFFICIENT_L3 = GetValue("RequestStrings:Lift:SpeedCoefficientL3");
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
    public static readonly string LIFT_CURRENT_POSITION_L1 = GetValue("RequestStrings:Lift:CurrentPositionL1");
    public static readonly string LIFT_CURRENT_POSITION_L2 = GetValue("RequestStrings:Lift:CurrentPositionL2");
    public static readonly string LIFT_CURRENT_POSITION_L3 = GetValue("RequestStrings:Lift:CurrentPositionL3");
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
    public static readonly string LIFT_FORWARD_L1 = GetValue("RequestStrings:Lift:ForwardL1");
    public static readonly string LIFT_FORWARD_L2 = GetValue("RequestStrings:Lift:ForwardL2");
    public static readonly string LIFT_FORWARD_L3 = GetValue("RequestStrings:Lift:ForwardL3");
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
    public static readonly string LIFT_BACKWARD_L1 = GetValue("RequestStrings:Lift:BackwardL1");
    public static readonly string LIFT_BACKWARD_L2 = GetValue("RequestStrings:Lift:BackwardL2");
    public static readonly string LIFT_BACKWARD_L3 = GetValue("RequestStrings:Lift:BackwardL3");
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
    public static readonly string LIFT_RESET_L1 = GetValue("RequestStrings:Lift:ResetL1");
    public static readonly string LIFT_RESET_L2 = GetValue("RequestStrings:Lift:ResetL2");
    public static readonly string LIFT_RESET_L3 = GetValue("RequestStrings:Lift:ResetL3");
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

    // ÎÒÂÎÄ
    public static readonly string OUTLET_FORWARD = GetValue("RequestStrings:Outlet:Forward");
    public static readonly string OUTLET_BACKWARD = GetValue("RequestStrings:Outlet:Backward");
    public static readonly string OUTLET_ROTATION = GetValue("RequestStrings:Outlet:Rotation");

    // ÏÅÄÀËÜ
    public static readonly string PEDAL_SENSOR = GetValue("RequestStrings:Pedal:Sensor");

    // ÊÍÎÏÊÀ ÑÒÎÏ
    public static readonly string STOP_BUTTON_SENSOR = GetValue("RequestStrings:StopButton:Sensor");

    // ÊÍÎÏÊÀ ÑÒÀÐÒ
    public static readonly string START_BUTTON_SENSOR = GetValue("RequestStrings:StartButton:Sensor");

    // ÀÂÒÎÌÀÒ ÇÀÙÈÒÛ ÝËÅÊÒÐÎÌÎÒÎÐÎÂ
    public static readonly string ELECTRIC_MOTORS_BREAKER_SENSOR = GetValue("RequestStrings:ElectricMotorsBreaker:Sensor");

    // ÇÀÙÈÒÍÀß ÏÀÍÅËÜ
    public static readonly string FORWARD_PROTECTION_PANEL_SENSOR = GetValue("RequestStrings:ProtectionPanel:ForwardSensor");
    public static readonly string BACKWARD_PROTECTION_PANEL_SENSOR = GetValue("RequestStrings:ProtectionPanel:BackwardSensor");

    // ÒÓÌÁËÅÐ ÑÊÎÐÎÑÒÈ
    public static readonly string SPEED_TUMBLER_LEFTWARD_SENSOR = GetValue("RequestStrings:SpeedTumbler:FirstOutputSignal");
    public static readonly string SPEED_TUMBLER_RIGHTWARD_SENSOR = GetValue("RequestStrings:SpeedTumbler:SecondOutputSignal");

    // ÊËÀÏÀÍ
    public static readonly string FIRST_VALVE_OUPUT_SIGNAL = GetValue("RequestStrings:Valve:FirstOutputSignal");
    public static readonly string SECOND_VALVE_OUTPUT_SIGNAL = GetValue("RequestStrings:Valve:SecondOutputSignal");

    // ÍÀÏÐÀÂËßÞÙÈÅ
    public static readonly string GUIDE_LUBRICANT_OUTPUT_SIGNAL = GetValue("RequestStrings:Guide:OutputSignal");

    // ÏÐÎÃÐÀÌÌÀ
    public static readonly string PROGRAM_RADIUS = GetValue("RequestStrings:Program:Radius");
    public static readonly string PROGRAM_FUNCTION = GetValue("RequestStrings:Program:Function");
    public static readonly string PROGRAM_COEFFICIENT = GetValue("RequestStrings:Program:Coefficient");
    public static readonly string PROGRAM_SPEED = GetValue("RequestStrings:Program:Speed");

    // ÍÀÑÒÐÎÉÊÈ
    public static readonly string SETTINGS_SPEED = GetValue("RequestStrings:Settings:Speed");
    public static readonly string SETTINGS_CYNCHRONIZATION_COEFFICIENT = GetValue("RequestStrings:Settings:SynchronizationCoefficient");
    public static readonly string SETTINGS_INTERCEPTION_MODE = GetValue("RequestStrings:Settings:InterceptionMode");
    public static readonly string SETTINGS_SINGLE_LEVELED = GetValue("RequestStrings:Settings:SingleLeveled");
    public static readonly string SETTINGS_WITH_PUNCHING_CYLINDER = GetValue("RequestStrings:Settings:WithPunchingCylinder");
    public static readonly string SETTINGS_DISTANCE_FROM_BENDING_TO_PUNCHING = GetValue("RequestStrings:Settings:DistanceFromBendingToPunching");

    // ÀÂÒÎÌÀÒ
    public static readonly string AUTOMATIC_TAGS_TURN_ON = GetValue("RequestStrings:AutomaticTags:TurnOn");
    public static readonly string AUTOMATIC_TAGS_CYCLE_TIME = GetValue("RequestStrings:AutomaticTags:CycleTime");
    public static readonly string AUTOMATIC_TAGS_SEND_DATA = GetValue("RequestStrings:AutomaticTags:SendData");
    public static readonly string AUTOMATIC_TAGS_END_PROGRAM = GetValue("RequestStrings:AutomaticTags:EndProgram");
    public static readonly string AUTOMATIC_TAGS_STEP_NUMBER = GetValue("RequestStrings:AutomaticTags:StepNumber");
    public static readonly string AUTOMATIC_TAGS_ALL_BEND = GetValue("RequestStrings:AutomaticTags:AllBend");
    public static readonly string AUTOMATIC_TAGS_FULL_AUTOMATIC = GetValue("RequestStrings:AutomaticTags:FullAutomatic");
    public static readonly string AUTOMATIC_TAGS_DELAY = GetValue("RequestStrings:AutomaticTags:Delay");
    public static readonly string AUTOMATIC_TAGS_COUNT_COMPLETED_DETAILS = GetValue("RequestStrings:AutomaticTags:CountCompletedDetails");

    private static string GetValue(string key) =>
        Configuration.GetValue<string>(key) ?? string.Empty;
}