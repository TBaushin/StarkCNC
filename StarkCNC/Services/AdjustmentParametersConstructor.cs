using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Models.Adjustment;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.DTO.Adjustment;

namespace StarkCNC.Services;

internal class AdjustmentParametersConstructor
{
    private readonly IConfiguration _configuration;
    private readonly ISettingsRepository _settingsRepository;

    private Settings? _settings;

    public AdjustmentParametersConstructor(IConfiguration configuration, ISettingsRepository settingsRepository)
    {
        _configuration = configuration;
        _settingsRepository = settingsRepository;
    }

    //public async Task<AdjustmentParameters> Build(string name, AdjustmentType type, double pipeDiameter, double radius)
    //{
    //    _settings = await _settingsRepository.GetAsync().ConfigureAwait(false);
    //    if (_settings is null)
    //        return BuildOnlyFromConfiguration(name, type, pipeDiameter, radius);
    //    else
    //        return BuildFromSettings(name, type, pipeDiameter, radius);
    //}

    //private AdjustmentParameters BuildOnlyFromConfiguration(string name, AdjustmentType type, double pipeDiameter, double radius)
    //{
    //    var adjustmentSection = App.Configuration.GetSection("Adjustment");

    //    var forwardDangerZoneSection = adjustmentSection.GetSection("ForwardDangerZone");
    //    var forwardDangerZoneCoordinate = forwardDangerZoneSection.GetSection("Default").Get<double>();
    //    var forwardDangerZoneCoordinateRequestString = forwardDangerZoneSection.GetSection("RequestString").Get<string>() ?? string.Empty;

    //    var distanceFromCenterSection = adjustmentSection.GetSection("DistanceFromCenter");
    //    var distanceFromCenter = distanceFromCenterSection.GetSection("Default").Get<double>();
    //    var distanceFromCenterRequestString = distanceFromCenterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

    //    var bend = BendDto.CreateFromConfiguration(adjustmentSection);
    //    var bendRoller = BendRollerDto.CreateFromConfiguration(adjustmentSection);
    //    var clamp = ClampDto.CreateFromConfiguration(adjustmentSection);
    //    var clampRoller = ClampRollerDto.CreateFromConfiguration(adjustmentSection);
    //    var console = ConsoleDto.CreateFromConfiguration(adjustmentSection);
    //    var dorn = DornDto.CreateFromConfiguration(adjustmentSection);
    //    var lift = LiftDto.CreateFromConfiguration(adjustmentSection);
    //    var press = PressDto.CreateFromConfiguration(adjustmentSection);
    //    var rotation = RotationDto.CreateFromConfiguration(adjustmentSection);
    //    var squeeze = SqueezeDto.CreateFromConfiguration(adjustmentSection);
    //    var supply = SupplyDto.CreateFromConfiguration(adjustmentSection);

    //    return new AdjustmentParameters(
    //        Guid.NewGuid(),
    //        name,
    //        pipeDiameter,
    //        radius,
    //        type,
    //        0,
    //        forwardDangerZoneCoordinate,
    //        distanceFromCenter,
    //        bend,
    //        bendRoller,
    //        clamp,
    //        clampRoller,
    //        console,
    //        dorn,
    //        lift,
    //        press,
    //        rotation,
    //        squeeze,
    //        supply);
    //}

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
