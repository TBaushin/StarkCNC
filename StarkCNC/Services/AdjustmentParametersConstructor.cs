using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Adjustment;
using StarkCNC.Core.Repository;

namespace StarkCNC.Services;

public class AdjustmentParametersConstructor
{
    private readonly IConfiguration _configuration;
    private readonly ISettingsRepository _settingsRepository;

    private Settings? _settings;

    public AdjustmentParametersConstructor(IConfiguration configuration, ISettingsRepository settingsRepository)
    {
        _configuration = configuration;
        _settingsRepository = settingsRepository;
    }

    public async Task<AdjustmentParameters> Build(string name, AdjustmentType type, float pipeDiameter, float radius)
    {
        _settings = await _settingsRepository.GetAsync().ConfigureAwait(false);
        //if (_settings is null)
        //    return BuildOnlyFromConfiguration(name, type, pipeDiameter, radius);
        //else
        //    return BuildFromSettings(name, type, pipeDiameter, radius);
        return BuildAdjustmentFromConfiguration(name, type, pipeDiameter, radius);
    }

    private AdjustmentParameters BuildAdjustmentFromConfiguration(string name, AdjustmentType type, float pipeDiameter, float radius)
    {
        var adjustmentSection = _configuration.GetSection("Adjustment");

        var forwardDangerZoneSection = adjustmentSection.GetSection("ForwardDangerZone");
        var forwardDangerZoneCoordinate = forwardDangerZoneSection.GetSection("Default").Get<float>();
        var forwardDangerZoneCoordinateRequestString = forwardDangerZoneSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var distanceFromCenterSection = adjustmentSection.GetSection("DistanceFromCenter");
        var distanceFromCenter = distanceFromCenterSection.GetSection("Default").Get<float>();
        var distanceFromCenterRequestString = distanceFromCenterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new AdjustmentParameters(
            Guid.NewGuid(),
            name,
            pipeDiameter,
            radius,
            type,
            0,
            false,
            forwardDangerZoneCoordinate,
            distanceFromCenter,
            BuildBendFromConfiguration(adjustmentSection),
            BuildBendRollerFromConfiguration(adjustmentSection),
            BuildClampFromConfiguration(adjustmentSection),
            BuildClampRollerFromConfiguration(adjustmentSection),
            BuildConsoleFromConfiguration(adjustmentSection),
            BuildDornFromConfiguration(adjustmentSection),
            BuildLiftFromConfiguration(adjustmentSection),
            BuildPressFromConfiguration(adjustmentSection),
            BuildRotationFromConfiguration(adjustmentSection),
            BuildSqueezeFromConfiguration(adjustmentSection),
            BuildSupplyFromConfiguration(adjustmentSection));
    }

    private static Bend BuildBendFromConfiguration(IConfigurationSection section)
    {
        var bendSection = section.GetSection("Bend");

        var forwardPositionLimitationSection = bendSection.GetSection("ForwardPositionLimitation");
        var forwardPositionLimitationDefault = forwardPositionLimitationSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = bendSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        var deflectionDuringClampClampingSection = bendSection.GetSection("DeflectionDuringClampClamping");
        var deflectionDuringClampClampingDefault = deflectionDuringClampClampingSection.GetSection("Default").Get<float>();

        var deflectionSection = bendSection.GetSection("Deflection");
        var deflectionDefault = deflectionSection.GetSection("Default").Get<float>();

        return new Bend(forwardPositionLimitationDefault, speedCoefficientDefault, deflectionDuringClampClampingDefault, deflectionDefault);
    }

    private static BendRoller BuildBendRollerFromConfiguration(IConfigurationSection section)
    {
        var bendRollerSection = section.GetSection("BendRoller");

        var radiusSection = bendRollerSection.GetSection("Radius");
        var radiusDefault = radiusSection.GetSection("Default").Get<float>();

        var outerRadiusSection = bendRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<float>();

        return new BendRoller(radiusDefault, outerRadiusDefault);
    }

    private static Clamp BuildClampFromConfiguration(IConfigurationSection section)
    {
        var clampSection = section.GetSection("Clamp");

        var deepSection = clampSection.GetSection("Deep");
        var deepDefault = deepSection.GetSection("Default").Get<float>();

        var lengthSection = clampSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<float>();

        var forwardPositionSection = clampSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<float>();

        var middlePositionSection = clampSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();

        var backwardPositionSection = clampSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = clampSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        return new Clamp(
            deepDefault,
            lengthDefault,
            forwardPositionDefault,
            middlePositionDefault,
            backwardPositionDefault,
            speedCoefficientDefault);
    }

    private static ClampRoller BuildClampRollerFromConfiguration(IConfigurationSection section)
    {
        var clampRollerSection = section.GetSection("ClampRoller");

        var outerRadiusSection = clampRollerSection.GetSection("OuterRadius");
        var outerRadiusDefault = outerRadiusSection.GetSection("Default").Get<float>();

        var innerRadiusSection = clampRollerSection.GetSection("InnerRadius");
        var innerRadiusDefault = innerRadiusSection.GetSection("Default").Get<float>();

        return new ClampRoller(outerRadiusDefault, innerRadiusDefault);
    }

    private static StarkCNC.Core.Models.Adjustment.Console BuildConsoleFromConfiguration(IConfigurationSection section)
    {
        var consoleSection = section.GetSection("Console");

        var bendPositionSection = consoleSection.GetSection("BendPosition");
        var bendPositionDefault = bendPositionSection.GetSection("Default").Get<float>();

        var secondFloorPositionSection = consoleSection.GetSection("SecondFloorPosition");
        var secondFloorPositionDefault = secondFloorPositionSection.GetSection("Default").Get<float>();

        var secondFloorIntermediatePositionSection = consoleSection.GetSection("SecondFloorIntermediatePosition");
        var secondFloorIntermediatePositionDefault = secondFloorIntermediatePositionSection.GetSection("Default").Get<float>();

        var thirdFloorPositionSection = consoleSection.GetSection("ThirdFloorPosition");
        var thirdFloorPositionDefault = thirdFloorPositionSection.GetSection("Default").Get<float>();

        var pipeRotationDepartureDistanceSection = consoleSection.GetSection("PipeRotationDepartureDistance");
        var pipeRotationDepartureDistanceDefault = pipeRotationDepartureDistanceSection.GetSection("Default").Get<float>();

        return new StarkCNC.Core.Models.Adjustment.Console(
            bendPositionDefault,
            secondFloorPositionDefault,
            secondFloorIntermediatePositionDefault,
            thirdFloorPositionDefault,
            pipeRotationDepartureDistanceDefault);
    }

    private static Dorn BuildDornFromConfiguration(IConfigurationSection section)
    {
        var dornSection = section.GetSection("Dorn");

        var forwardPositionSection = dornSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<float>();

        var middlePositionSection = dornSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();

        var backwardPositionSection = dornSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = dornSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        return new Dorn(forwardPositionDefault, middlePositionDefault, backwardPositionDefault, speedCoefficientDefault);
    }

    private static Lift BuildLiftFromConfiguration(IConfigurationSection section)
    {
        var liftSection = section.GetSection("Lift");

        var upperPositionSection = liftSection.GetSection("UpperPosition");
        var upperPositionDefault = upperPositionSection.GetSection("Default").Get<float>();

        var middlePositionSection = liftSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();

        var lowerPositionSection = liftSection.GetSection("LowerPosition");
        var lowerPositionDefault = lowerPositionSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = liftSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        return new Lift(upperPositionDefault, middlePositionDefault, lowerPositionDefault, speedCoefficientDefault);
    }

    private static Press BuildPressFromConfiguration(IConfigurationSection section)
    {
        var pressSection = section.GetSection("Press");

        var dangerZoneCoordinateSection = pressSection.GetSection("DangerZoneCoordinate");
        var dangerZoneCoordinateDefault = dangerZoneCoordinateSection.GetSection("Default").Get<float>();

        var lengthSection = pressSection.GetSection("Length");
        var lengthDefault = lengthSection.GetSection("Default").Get<float>();

        var forwardPositionSection = pressSection.GetSection("ForwardPosition");
        var forwardPositionDefault = forwardPositionSection.GetSection("Default").Get<float>();

        var middlePositionSection = pressSection.GetSection("MiddlePosition");
        var middlePositionDefault = middlePositionSection.GetSection("Default").Get<float>();

        var backwardPositionSection = pressSection.GetSection("BackwardPosition");
        var backwardPositionDefault = backwardPositionSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = pressSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        return new Press(
            dangerZoneCoordinateDefault,
            lengthDefault,
            forwardPositionDefault,
            middlePositionDefault,
            backwardPositionDefault,
            speedCoefficientDefault);
    }

    private static Rotation BuildRotationFromConfiguration(IConfigurationSection section)
    {
        var rotationSection = section.GetSection("Rotation");

        var offsetAfterZeroSearchSection = rotationSection.GetSection("OffsetAfterZeroSearch");
        var offsetAfterZeroSearchDefault = offsetAfterZeroSearchSection.GetSection("Default").Get<float>();

        return new Rotation(offsetAfterZeroSearchDefault);
    }

    private static Squeeze BuildSqueezeFromConfiguration(IConfigurationSection section)
    {
        var squeezeSection = section.GetSection("Squeeze");

        var turnOnSection = squeezeSection.GetSection("TurnOn");
        var turnOnDefault = turnOnSection.GetSection("Default").Get<bool>();

        var frontPositionLimitationSection = squeezeSection.GetSection("FrontPositionLimitation");
        var frontPositionLimitationDefault = frontPositionLimitationSection.GetSection("Default").Get<float>();

        var speedCoefficientSection = squeezeSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<float>();

        return new Squeeze(turnOnDefault, frontPositionLimitationDefault, speedCoefficientDefault);
    }

    private static Supply BuildSupplyFromConfiguration(IConfigurationSection section)
    {
        var supplySection = section.GetSection("Supply");

        var pressZonePositionSection = supplySection.GetSection("PressZonePosition");
        var pressZonePositionDefault = pressZonePositionSection.GetSection("Default").Get<float>();

        var forwardDangerZonePositionSection = supplySection.GetSection("ForwardDangerZonePosition");
        var forwardDangerZonePositionDefault = forwardDangerZonePositionSection.GetSection("Default").Get<float>();

        var colletJawsDepthSection = supplySection.GetSection("ColletJawsDepth");
        var colletJawsDepthDefault = colletJawsDepthSection.GetSection("Default").Get<float>();

        return new Supply(
            pressZonePositionDefault,
            forwardDangerZonePositionDefault,
            colletJawsDepthDefault);
    }

    //private AdjustmentParameters BuildFromSettings(string name, AdjustmentType type, double pipeDiameter, double radius)
    //{
    //    return new AdjustmentParameters(
    //        Guid.NewGuid(),
    //        name,
    //        pipeDiameter,
    //        radius,
    //        type,
    //        0,
    //        10000000, // TODO: !
    //        1000000, // TODO: !
    //        new Bend(_settings.Bend.Coefficient),
    //        new BendRoller(),
    //        new Clamp(),
    //        new ClampRoller(),
    //        new Core.Models.Adjustment.Console(_settings.Console.Coefficient),
    //        new Dorn(),
    //        new Lift(),
    //        new Press(),
    //        new Rotation(_settings.Rotation.Offset, _settings.Rotation.Coefficient),
    //        new Squeeze(),
    //        new Supply(_settings.Supply.Coefficient));
    //}
}
