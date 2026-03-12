using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;

namespace StarkCNC.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    ISettingsRepository _settingsRepository;
    IManualConfigurationService _manualConfigurationService;
    Settings _settings;

    [ObservableProperty]
    bool _dornAutomatic;

    [ObservableProperty]
    double _dornLeadWithdrawalBeforeBend;

    [ObservableProperty]
    bool _dornLubricantTurnOn;

    [ObservableProperty]
    bool _bendSynchronization;

    [ObservableProperty]
    double _bendSynchronizationCoefficient;

    [ObservableProperty]
    bool _bendAndSupplySynchronization;

    [ObservableProperty]
    bool _interceptionMode;

    [ObservableProperty]
    bool _consoleOutletForPipeInstalling;

    [ObservableProperty]
    double _pipeOutletCoordinate;

    [ObservableProperty]
    bool _multiLeveled;

    [ObservableProperty]
    bool _withPunchingCylinder;

    [ObservableProperty]
    double _distanceFromBendingToPunching;

    [ObservableProperty]
    bool _isElectricBendingDrive;

    [ObservableProperty]
    bool _absoluteUnitCoordinate;

    [ObservableProperty]
    double _supportFirstLiftBan;

    [ObservableProperty]
    double _supportSecondLiftBan;

    [ObservableProperty]
    double _supportThirdLiftBanRear;

    [ObservableProperty]
    double _supportThirdLiftBanFront;

    [ObservableProperty]
    double _supportFourthLiftBan;

    [ObservableProperty]
    bool _banPressWhenSupportIsLifted;

    [ObservableProperty]
    double _squeezeWorkTime;

    [ObservableProperty]
    double _supplyStartRollingSpeed;

    [ObservableProperty]
    bool _incompletePressMovement;

    [ObservableProperty]
    bool _hydraulicMovementWithoutSensors;

    [ObservableProperty]
    bool _showButtonFullAutomatic;

    [ObservableProperty]
    bool _invertClampSensors;

    [ObservableProperty]
    double _supplyCoefficient;

    [ObservableProperty]
    double _rotationCoefficient;

    [ObservableProperty]
    double _consoleCoefficient;

    [ObservableProperty]
    double _bendCoefficient;

    [ObservableProperty]
    double _supplyAcceleration;

    [ObservableProperty]
    double _rotationAcceleration;

    [ObservableProperty]
    double _consoleAcceleration;

    [ObservableProperty]
    double _bendAcceleration;

    [ObservableProperty]
    double _supplyBraking;

    [ObservableProperty]
    double _rotationBraking;

    [ObservableProperty]
    double _consoleBraking;

    [ObservableProperty]
    double _bendBraking;

    [ObservableProperty]
    double _supplyJerk;

    [ObservableProperty]
    double _rotationJerk;

    [ObservableProperty]
    double _consoleJerk;

    [ObservableProperty]
    double _bendJerk;

    public SettingsViewModel(ISettingsRepository settingsRepository, IManualConfigurationService manualConfigurationService)
    {
        _settingsRepository = settingsRepository;
        _manualConfigurationService = manualConfigurationService;

        var settings = _settingsRepository.GetAsync().Result;
        if (settings is null)
        {
            settings = new Settings();
            settings.Id = Guid.NewGuid();
            _settings = _settingsRepository
                .AddElementAsync(settings)
                .GetAwaiter()
                .GetResult() ?? settings;
        }
        else
            _settings = settings;

        ReadData();

        PropertyChanged += SettingsViewModel_PropertyChanged;
    }

    void ReadData()
    {
        DornAutomatic = _settings.DornAutomatic;
        DornLeadWithdrawalBeforeBend = _settings.DornLeadWithdrawalBeforeBend;
        DornLubricantTurnOn = _settings.DornLubricantTurnOn;
        BendSynchronization = _settings.BendSynchronization;
        BendSynchronizationCoefficient = _settings.BendSynchronizationCoefficient;
        BendAndSupplySynchronization = _settings.BendAndSupplySynchronization;
        InterceptionMode = _settings.InterceptionMode;
        ConsoleOutletForPipeInstalling = _settings.ConsoleOutletForPipeInstalling;
        PipeOutletCoordinate = _settings.PipeOutletCoordinate;
        MultiLeveled = _settings.MultiLeveled;
        WithPunchingCylinder = _settings.WithPunchingCylinder;
        DistanceFromBendingToPunching = _settings.DistanceFromBendingToPunching;
        IsElectricBendingDrive = _settings.IsElectricBendingDrive;
        AbsoluteUnitCoordinate = _settings.AbsoluteUnitCoordinate;
        SupportFirstLiftBan = _settings.SupportFirstLiftBan;
        SupportSecondLiftBan = _settings.SupportSecondLiftBan;
        SupportThirdLiftBanRear = _settings.SupportThirdLiftBanRear;
        SupportThirdLiftBanFront = _settings.SupportThirdLiftBanFront;
        SupportFourthLiftBan = _settings.SupportFourthLiftBan;
        BanPressWhenSupportIsLifted = _settings.BanPressWhenSupportIsLifted;
        SqueezeWorkTime = _settings.SqueezeWorkTime;
        SupplyStartRollingSpeed = _settings.SupplyStartRollingSpeed;
        IncompletePressMovement = _settings.IncompletePressMovement;
        HydraulicMovementWithoutSensors = _settings.HydraulicMovementWithoutSensors;
        ShowButtonFullAutomatic = _settings.ShowButtonFullAutomatic;
        InvertClampSensors = _settings.InvertClampSensors;
        SupplyCoefficient = _settings.SupplyCoefficient;
        RotationCoefficient = _settings.RotationCoefficient;
        ConsoleCoefficient = _settings.ConsoleCoefficient;
        BendCoefficient = _settings.BendCoefficient;
        SupplyAcceleration = _settings.SupplyAcceleration;
        RotationAcceleration = _settings.RotationAcceleration;
        ConsoleAcceleration = _settings.ConsoleAcceleration;
        BendAcceleration = _settings.BendAcceleration;
        SupplyBraking = _settings.SupplyBraking;
        RotationBraking = _settings.RotationBraking;
        ConsoleBraking = _settings.ConsoleBraking;
        BendBraking = _settings.BendBraking;
        SupplyJerk = _settings.SupplyJerk;
        RotationJerk = _settings.RotationJerk;
        ConsoleJerk = _settings.ConsoleJerk;
        BendJerk = _settings.BendJerk;
    }

    async void SettingsViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        _settings.DornAutomatic = DornAutomatic;
        _settings.DornLeadWithdrawalBeforeBend = DornLeadWithdrawalBeforeBend;
        _settings.DornLubricantTurnOn = DornLubricantTurnOn;
        _settings.BendSynchronization = BendSynchronization;
        _settings.BendSynchronizationCoefficient = BendSynchronizationCoefficient;
        _settings.BendAndSupplySynchronization = BendAndSupplySynchronization;
        _settings.InterceptionMode = InterceptionMode;
        _settings.ConsoleOutletForPipeInstalling = ConsoleOutletForPipeInstalling;
        _settings.PipeOutletCoordinate = PipeOutletCoordinate;
        _settings.MultiLeveled = MultiLeveled;
        _settings.WithPunchingCylinder = WithPunchingCylinder;
        _settings.DistanceFromBendingToPunching = DistanceFromBendingToPunching;
        _settings.IsElectricBendingDrive = IsElectricBendingDrive;
        _settings.AbsoluteUnitCoordinate = AbsoluteUnitCoordinate;
        _settings.SupportFirstLiftBan = SupportFirstLiftBan;
        _settings.SupportSecondLiftBan = SupportSecondLiftBan;
        _settings.SupportThirdLiftBanRear = SupportThirdLiftBanRear;
        _settings.SupportThirdLiftBanFront = SupportThirdLiftBanFront;
        _settings.SupportFourthLiftBan = SupportFourthLiftBan;
        _settings.BanPressWhenSupportIsLifted = BanPressWhenSupportIsLifted;
        _settings.SqueezeWorkTime = SqueezeWorkTime;
        _settings.SupplyStartRollingSpeed = SupplyStartRollingSpeed;
        _settings.IncompletePressMovement = IncompletePressMovement;
        _settings.HydraulicMovementWithoutSensors = HydraulicMovementWithoutSensors;
        _settings.ShowButtonFullAutomatic = ShowButtonFullAutomatic;
        _settings.InvertClampSensors = InvertClampSensors;
        _settings.SupplyCoefficient = SupplyCoefficient;
        _settings.RotationCoefficient = RotationCoefficient;
        _settings.ConsoleCoefficient = ConsoleCoefficient;
        _settings.BendCoefficient = BendCoefficient;
        _settings.SupplyAcceleration = SupplyAcceleration;
        _settings.RotationAcceleration = RotationAcceleration;
        _settings.ConsoleAcceleration = ConsoleAcceleration;
        _settings.BendAcceleration = BendAcceleration;
        _settings.SupplyBraking = SupplyBraking;
        _settings.RotationBraking = RotationBraking;
        _settings.ConsoleBraking = ConsoleBraking;
        _settings.BendBraking = BendBraking;
        _settings.SupplyJerk = SupplyJerk;
        _settings.RotationJerk = RotationJerk;
        _settings.ConsoleJerk = ConsoleJerk;
        _settings.BendJerk = BendJerk;

        await _settingsRepository.UpdateElementAsync(_settings).ConfigureAwait(false);
    }

    [RelayCommand]
    async Task DornAutomaticSend()
    {
        await _manualConfigurationService
            .WriteAsync(DornAutomatic, ControllerRequestStrings.DORN_AUTOMATIC)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task DornLeadWithdrawalBeforeBendSend()
    {
        await _manualConfigurationService
            .WriteAsync(DornLeadWithdrawalBeforeBend, ControllerRequestStrings.DORN_LEAD_WITHDRAWAL_BEFORE_BEND)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task DornLubricantTurnOnSend()
    {
        await _manualConfigurationService
            .WriteAsync(DornLubricantTurnOn, ControllerRequestStrings.DORN_LUBRICANT_LUBRICANT_TURN_ON)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendSynchronizationSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendSynchronization, ControllerRequestStrings.BEND_AND_SUPPLY_PUSHING_ENABLE)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendSynchronizationCoefficientSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendSynchronizationCoefficient, ControllerRequestStrings.BEND_AND_SUPPLY_COEFFICIENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendAndSupplySynchronizationSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendAndSupplySynchronization, ControllerRequestStrings.BEND_AND_SUPPLY_SYNCHRONIZATION)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task InterceptionModeSend()
    {
        await _manualConfigurationService
            .WriteAsync(InterceptionMode, ControllerRequestStrings.SETTINGS_INTERCEPTION_MODE)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ConsoleOutletForPipeInstallingSend()
    {
        await _manualConfigurationService
            .WriteAsync(ConsoleOutletForPipeInstalling, ControllerRequestStrings.CONSOLE_OUTLET_FOR_PIPE_INSTALLING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task PipeOutletCoordinateSend()
    {
        await _manualConfigurationService
            .WriteAsync(PipeOutletCoordinate, ControllerRequestStrings.PIPE_OUTLET_COORDINATE)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task MultiLeveledSend()
    {
        await _manualConfigurationService
            .WriteAsync(MultiLeveled, ControllerRequestStrings.SETTINGS_SINGLE_LEVELED)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task WithPunchingCylinderSend()
    {
        await _manualConfigurationService
            .WriteAsync(WithPunchingCylinder, ControllerRequestStrings.SETTINGS_WITH_PUNCHING_CYLINDER)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task DistanceFromBendingToPunchingSend()
    {
        await _manualConfigurationService
            .WriteAsync(DistanceFromBendingToPunching, ControllerRequestStrings.SETTINGS_DISTANCE_FROM_BENDING_TO_PUNCHING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task IsElectricBendingDriveSend()
    {
        await _manualConfigurationService
            .WriteAsync(IsElectricBendingDrive, "")
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task AbsoluteUnitCoordinateSend()
    {
        await _manualConfigurationService
            .WriteAsync(AbsoluteUnitCoordinate, "")
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupportFirstLiftBanSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupportFirstLiftBan, ControllerRequestStrings.SUPPORT_FIRST_COORDINATE_BAN)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupportSecondLiftBanSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupportSecondLiftBan, ControllerRequestStrings.SUPPORT_SECOND_COORDINATE_BAN)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupportThirdLiftBanRearSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupportThirdLiftBanRear, ControllerRequestStrings.SUPPORT_THIRD_BACKWARD_COORDINATE_BAN)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupportThirdLiftBanFrontSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupportThirdLiftBanFront, ControllerRequestStrings.SUPPORT_THIRD_FORWARD_COORDINATE_BAN)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupportFourthLiftBanSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupportFourthLiftBan, ControllerRequestStrings.SUPPORT_FOURTH_COORDINATE_BAN)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BanPressWhenSupportIsLiftedSend()
    {
        await _manualConfigurationService
            .WriteAsync(BanPressWhenSupportIsLifted, ControllerRequestStrings.SUPPORT_BAN_PRESS)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SqueezeWorkTimeSend()
    {
        await _manualConfigurationService
            .WriteAsync(SqueezeWorkTime, ControllerRequestStrings.SQUEEZE_WORK_TIME)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupplyStartRollingSpeedSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupplyStartRollingSpeed, ControllerRequestStrings.SUPPLY_START_ROLLING_SPEED)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task IncompletePressMovementSend()
    {
        await _manualConfigurationService
            .WriteAsync(IncompletePressMovement, ControllerRequestStrings.PRESS_INCOMPLETE_MOVEMENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task HydraulicMovementWithoutSensorsSend()
    {
        await _manualConfigurationService
            .WriteAsync(HydraulicMovementWithoutSensors, ControllerRequestStrings.HYDRAULICS_MOVEMENT_WITHOUT_SENSORS)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ShowButtonFullAutomaticSend()
    {
        await _manualConfigurationService
            .WriteAsync(ShowButtonFullAutomatic, "")
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task InvertClampSensorsSend()
    {
        await _manualConfigurationService
            .WriteAsync(InvertClampSensors, ControllerRequestStrings.CLAMP_INVERT_SENSORS)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupplyCoefficientSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupplyCoefficient, ControllerRequestStrings.SUPPLY_COEFFICIENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task RotationCoefficientSend()
    {
        await _manualConfigurationService
            .WriteAsync(RotationCoefficient, ControllerRequestStrings.ROTATION_COEFFICIENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ConsoleCoefficientSend()
    {
        await _manualConfigurationService
            .WriteAsync(ConsoleCoefficient, ControllerRequestStrings.CONSOLE_COEFFICIENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendCoefficientSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendCoefficient, ControllerRequestStrings.BEND_COEFFICIENT)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupplyAccelerationSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupplyAcceleration, ControllerRequestStrings.SUPPLY_ACCELERATION)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task RotationAccelerationSend()
    {
        await _manualConfigurationService
            .WriteAsync(RotationAcceleration, ControllerRequestStrings.ROTATION_ACCELERATION)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ConsoleAccelerationSend()
    {
        await _manualConfigurationService
            .WriteAsync(ConsoleAcceleration, ControllerRequestStrings.CONSOLE_ACCELERATION)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendAccelerationSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendAcceleration, ControllerRequestStrings.BEND_ACCELERATION)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupplyBrakingSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupplyBraking, ControllerRequestStrings.SUPPLY_BRAKING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task RotationBrakingSend()
    {
        await _manualConfigurationService
            .WriteAsync(RotationBraking, ControllerRequestStrings.ROTATION_BRAKING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ConsoleBrakingSend()
    {
        await _manualConfigurationService
            .WriteAsync(ConsoleBraking, ControllerRequestStrings.CONSOLE_BRAKING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendBrakingSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendBraking, ControllerRequestStrings.BEND_BRAKING)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task SupplyJerkSend()
    {
        await _manualConfigurationService
            .WriteAsync(SupplyJerk, ControllerRequestStrings.SUPPLY_JERK)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task RotationJerkSend()
    {
        await _manualConfigurationService
            .WriteAsync(RotationJerk, ControllerRequestStrings.ROTATION_JERK)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task ConsoleJerkSend()
    {
        await _manualConfigurationService
            .WriteAsync(ConsoleJerk, ControllerRequestStrings.CONSOLE_JERK)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    async Task BendJerkSend()
    {
        await _manualConfigurationService
            .WriteAsync(BendJerk, ControllerRequestStrings.BEND_JERK)
            .ConfigureAwait(true);
    }
}