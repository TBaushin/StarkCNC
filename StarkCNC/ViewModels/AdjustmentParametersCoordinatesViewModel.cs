using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.DTO;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;

namespace StarkCNC.ViewModels;

public partial class AdjustmentParametersCoordinatesViewModel : ViewModelBase
{
    private Guid? _adjustmentId;
    private readonly IAdjustmentService _adjustmentService;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IConfiguration _configuration;
    private readonly IManualConfigurationService _manualConfigurationService;

    private AdjustmentParameters _adjustment;

    [ObservableProperty]
    private bool _isElectricMachine;

    [ObservableProperty]
    private AdjustmentParametersBendVisibleDto _bend;

    [ObservableProperty]
    private AdjustmentParametersClampVisibleDto _clamp;

    [ObservableProperty]
    private AdjustmentParametersConsoleVisibleDto _console;

    [ObservableProperty]
    private AdjustmentParametersDornVisibleDto _dorn;

    [ObservableProperty]
    private AdjustmentParametersLiftVisibleDto _lift;

    [ObservableProperty]
    private AdjustmentParametersPressVisibleDto _press;

    [ObservableProperty]
    private AdjustmentParametersRotationVisibleDto _rotation;

    [ObservableProperty]
    private AdjustmentParametersSqueezeVisibleDto _squeeze;

    [ObservableProperty]
    private AdjustmentParametersSupplyVisibleDto _supply;

    public AdjustmentParametersCoordinatesViewModel(
        IAdjustmentService serivce,
        ISettingsRepository settingsRepository,
        IConfiguration configuration,
        IManualConfigurationService manualConfigurationService,
        Guid? id)
    {
        _adjustmentService = serivce;
        _settingsRepository = settingsRepository;
        _configuration = configuration;
        _manualConfigurationService = manualConfigurationService;
        _adjustmentId = id;
    }

    private async Task SetSelectedAdjustment(Guid? id)
    {
        if (id is not Guid guid)
            throw new ArgumentNullException(nameof(id));

        var adjustment = await _adjustmentService.FindByIdAsync(guid).ConfigureAwait(true);
        if (adjustment is null)
            throw new InvalidOperationException("Не удалось найти оснастку");

        _adjustment = adjustment;
    }

    public async Task InitializeAsync()
    {
        await UpdateAdjustment().ConfigureAwait(true);
    }

    private async Task LoadSettings()
    {
        var settings = await _settingsRepository.GetAsync().ConfigureAwait(true);
        IsElectricMachine = settings?.IsElectricBendingDrive ?? false;
    }

    private async Task UpdateAdjustment()
    {
        await LoadSettings().ConfigureAwait(true);
        await SetSelectedAdjustment(_adjustmentId).ConfigureAwait(true);

        if (_adjustment is null)
            throw new InvalidOperationException("Не удалось найти оснастку");

        Bend = new AdjustmentParametersBendVisibleDto()
        {
            ForwardPositionLimitation = _adjustment.Bend.ForwardPositionLimitation,
            SpeedCoefficient = _adjustment.Bend.SpeedCoefficient,
            DeflectionDuringClampClamping = _adjustment.Bend.DeflectionDuringClampClamping,
            Deflection = _adjustment.Bend.Deflection
        };

        Clamp = new AdjustmentParametersClampVisibleDto()
        {
            ForwardPosition = _adjustment.Clamp.ForwardPosition,
            MiddlePosition = _adjustment.Clamp.MiddlePosition,
            BackwardPosition = _adjustment.Clamp.BackwardPosition,
            SpeedCoefficient = _adjustment.Clamp.SpeedCoefficient
        };

        Console = new AdjustmentParametersConsoleVisibleDto()
        {
            BendPosition = _adjustment.Console.BendPosition,
            SecondFloorPosition = _adjustment.Console.SecondFloorPosition,
            ThirdFloorPosition = _adjustment.Console.ThirdFloorPosition,
            PipeRotationDepartureDistance = _adjustment.Console.PipeRotationDepartureDistance
        };

        Dorn = new AdjustmentParametersDornVisibleDto()
        {
            ForwardPosition = _adjustment.Dorn.ForwardPosition,
            MiddlePosition = _adjustment.Dorn.MiddlePosition,
            BackwardPosition = _adjustment.Dorn.BackwardPosition,
            SpeedCoefficient = _adjustment.Dorn.SpeedCoefficient
        };

        Lift = new AdjustmentParametersLiftVisibleDto()
        {
            UpperPosition = _adjustment.Lift.UpperPosition,
            MiddlePosition = _adjustment.Lift.MiddlePosition,
            LowerPosition = _adjustment.Lift.LowerPosition,
            SpeedCoefficient = _adjustment.Lift.SpeedCoefficient
        };

        Press = new AdjustmentParametersPressVisibleDto()
        {
            ForwardPosition = _adjustment.Press.ForwardPosition,
            MiddlePosition = _adjustment.Press.MiddlePosition,
            BackwardPosition = _adjustment.Press.BackwardPosition,
            SpeedCoefficient = _adjustment.Press.SpeedCoefficient
        };

        Rotation = new AdjustmentParametersRotationVisibleDto()
        {
            OffsetAfterZeroSearch = _adjustment.Rotation.OffsetAfterZeroSearch
        };

        Squeeze = new AdjustmentParametersSqueezeVisibleDto()
        {
            FrontPositionLimitation = _adjustment.Squeeze.FrontPositionLimitation,
            SpeedCoefficient = _adjustment.Squeeze.SpeedCoefficient
        };

        Supply = new AdjustmentParametersSupplyVisibleDto()
        {
            PressZonePosition = _adjustment.Supply.PressZonePosition,
            ForwardDangerZonePosition = _adjustment.Supply.ForwardDangerZonePosition,
            ColletJawsDepth = _adjustment.Supply.ColletJawsDepth,
        };
    }

    [RelayCommand]
    private async Task EditParameters(string parameter)
    {
        var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(_adjustment, parameter, _adjustmentService, _manualConfigurationService);
        parametersSettingsWindow.ShowDialog();

        var adjustment = parametersSettingsWindow.Adjustment;
        if (adjustment.Id == _adjustment.Id)
        {
            await _adjustmentService.UpdateElementAsync(adjustment).ConfigureAwait(true);
            await UpdateAdjustment().ConfigureAwait(true);

            // Отсылка на контроллер
            var level = _adjustment.InstalledLevel;
            switch (parameter)
            {
                case nameof(adjustment.Bend):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Bend.ForwardPositionLimitation,
                            ControllerRequestStrings.GET_BEND_FORWARD_POSITION_LIMITATION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Bend.DeflectionDuringClampClamping,
                            ControllerRequestStrings.GET_BEND_DEFLECTION_DURING_CLAMP_CLAMPING(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Bend.Deflection, ControllerRequestStrings.GET_BEND_DEFLECTION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Bend.SpeedCoefficient,
                            ControllerRequestStrings.GET_BEND_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.BendRoller):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.BendRoller.Radius,
                            ControllerRequestStrings.GET_BEND_ROLLER_RADIUS(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.BendRoller.OuterRadius,
                            ControllerRequestStrings.GET_BEND_ROLLER_OUTER_RADIUS(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Clamp):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.Length,
                            ControllerRequestStrings.GET_CLAMP_LENGTH(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.Deep,
                            ControllerRequestStrings.GET_CLAMP_DEEP(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.ForwardPosition,
                            ControllerRequestStrings.GET_CLAMP_FORWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.MiddlePosition,
                            ControllerRequestStrings.GET_CLAMP_MIDDLE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.BackwardPosition,
                            ControllerRequestStrings.GET_CLAMP_BACKWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Clamp.SpeedCoefficient,
                            ControllerRequestStrings.GET_CLAMP_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.ClampRoller):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.ClampRoller.InnerRadius,
                            ControllerRequestStrings.GET_CLAMP_ROLLER_INNTER_RADIUS(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.ClampRoller.OuterRadius,
                            ControllerRequestStrings.GET_CLAMP_ROLLER_OUTER_RADIUS(level))
                        .ConfigureAwait(true);
                    break;

                case nameof(adjustment.Console):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Console.BendPosition,
                            ControllerRequestStrings.GET_CONSOLE_BEND_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Console.PipeRotationDepartureDistance,
                            ControllerRequestStrings.GET_CONSOLE_PIPE_ROTATION_DEPARTURE_DISTANCE(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Console.SecondFloorPosition,
                            ControllerRequestStrings.GET_CONSOLE_SECOND_FLOOR_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Console.SecondFloorIntermediatePosition,
                            ControllerRequestStrings.GET_CONSOLE_SECOND_FLOOR_INTERMEDIATE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Console.ThirdFloorPosition,
                            ControllerRequestStrings.GET_CONSOLE_THIRD_FLOOR_POSITION(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Dorn):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Dorn.ForwardPosition,
                            ControllerRequestStrings.GET_DORN_FORWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Dorn.MiddlePosition,
                            ControllerRequestStrings.GET_DORN_MIDDLE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Dorn.BackwardPosition,
                            ControllerRequestStrings.GET_DORN_BACKWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Dorn.SpeedCoefficient,
                            ControllerRequestStrings.GET_DORN_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Lift):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Lift.UpperPosition,
                            ControllerRequestStrings.GET_LIFT_UPPER_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Lift.MiddlePosition,
                            ControllerRequestStrings.GET_LIFT_MIDDLE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Lift.LowerPosition,
                            ControllerRequestStrings.GET_LIFT_LOWER_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Lift.SpeedCoefficient,
                            ControllerRequestStrings.GET_LIFT_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Press):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.Length,
                            ControllerRequestStrings.GET_PRESS_LENGTH(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.DangerZoneCoordinate,
                            ControllerRequestStrings.GET_PRESS_DANGER_ZONE_COORDINATE(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.ForwardPosition,
                            ControllerRequestStrings.GET_PRESS_FORWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.MiddlePosition,
                            ControllerRequestStrings.GET_PRESS_MIDDLE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.BackwardPosition,
                            ControllerRequestStrings.GET_PRESS_BACKWARD_POSITION(level))
                        .ConfigureAwait(true); // TODO: Проверить
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Press.SpeedCoefficient,
                            ControllerRequestStrings.GET_PRESS_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Rotation):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Rotation.OffsetAfterZeroSearch,
                            ControllerRequestStrings.GET_ROTATION_OFFSET_AFTER_ZERO_SEARCH(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Squeeze):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Squeeze.TurnOn,
                            ControllerRequestStrings.GET_SQUEEZE_TURN_ON(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Squeeze.FrontPositionLimitation,
                            ControllerRequestStrings.GET_SQUEEZE_FRONT_POSITION_LIMITATION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Squeeze.SpeedCoefficient,
                            ControllerRequestStrings.GET_SQUEEZE_SPEED_COEFFICIENT(level))
                        .ConfigureAwait(true);
                    break;
                case nameof(adjustment.Supply):
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Supply.PressZonePosition,
                            ControllerRequestStrings.GET_SUPPLY_PRESS_ZONE_POSITION(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Supply.ColletJawsDepth,
                            ControllerRequestStrings.GET_SUPPLY_COLLET_JAWS_DEPTH(level))
                        .ConfigureAwait(true);
                    await _manualConfigurationService
                        .WriteAsync(
                            _adjustment.Supply.ForwardDangerZonePosition,
                            ControllerRequestStrings.GET_SUPPLY_FORWARD_DANGER_ZONE(level))
                        .ConfigureAwait(true);
                    break;
            }
        }
    }
}
