using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using System.Windows.Input;

namespace StarkCNC.ViewModels;

public partial class InputOutputTableViewModel : ViewModelBase, IDisposable
{
    private IManualConfigurationService _configurationService;
    private bool _disposed;

    [ObservableProperty]
    private Settings _settings;

    // Таблица входов
    [ObservableProperty]
    private bool _clampForwardSensorStatus;

    [ObservableProperty]
    private bool _clampBackwardSensorStatus;

    [ObservableProperty]
    private bool _pressForwardSensorStatus;

    [ObservableProperty]
    private bool _pressBackwardSensorStatus;

    [ObservableProperty]
    private bool _squeezeForwardSensorStatus;

    [ObservableProperty]
    private bool _squeezeBackwardSensorStatus;

    [ObservableProperty]
    private bool _secondSqueezeForwardSensorStatus;

    [ObservableProperty]
    private bool _secondSqueezeBackwardSensorStatus;

    [ObservableProperty]
    private bool _dornForwardSensorStatus;

    [ObservableProperty]
    private bool _dornBackwardSensorStatus;

    [ObservableProperty]
    private bool _bendForwardSensorStatus;

    [ObservableProperty]
    private bool _bendBackwardSensorStatus;

    [ObservableProperty]
    private bool _colletForwardSensorStatus;

    [ObservableProperty]
    private bool _colletBackwardSensorStatus;

    [ObservableProperty]
    private bool _consoleForwardSensorStatus;

    [ObservableProperty]
    private bool _consoleBackwardSensorStatus;

    [ObservableProperty]
    private bool _supplyBackwardSensorStatus;

    [ObservableProperty]
    private bool _supplyResetSensorStatus;

    [ObservableProperty]
    private bool _rotationResetSensorStatus;

    [ObservableProperty]
    private bool _supportLowerSensorStatus;

    [ObservableProperty]
    private bool _adjustmentUpperSensorStatus;

    [ObservableProperty]
    private bool _adjustmentMiddleSensorStatus;

    [ObservableProperty]
    private bool _adjustmentLowerSensorStatus;

    [ObservableProperty]
    private bool _punchingForwardSensorStatus;

    [ObservableProperty]
    private bool _punchingBackwardSensorStatus;

    [ObservableProperty]
    private bool _pedalSensorStatus;

    [ObservableProperty]
    private bool _stopButtonSensorStatus;

    [ObservableProperty]
    private bool _startButtonSensorStatus;

    [ObservableProperty]
    private bool _electricMotorsBreakerSensorStatus;

    [ObservableProperty]
    private bool _backwardProtectionPanelSensorStatus;

    [ObservableProperty]
    private bool _forwardProtectionPanelSensorStatus;

    [ObservableProperty]
    private bool _speedTumblerLeftwardSensorStatus;

    [ObservableProperty]
    private bool _speedTumblerRightwardSensorStatus;

    //Таблица выходов
    [ObservableProperty]
    private bool _clampForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _clampBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _pressForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _pressBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _squeezeForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _squeezeBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _adjustmentUpOutputSignalStatus;

    [ObservableProperty]
    private bool _adjustmentDownOutputSignalStatus;

    [ObservableProperty]
    private bool _punchingForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _punchingBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _firstHydraulicsOutputSignalStatus;

    [ObservableProperty]
    private bool _firstValveOutputSignalStatus;

    [ObservableProperty]
    private bool _dornForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _dornBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _bendForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _bendBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _colletForwardOutputSignalStatus;

    [ObservableProperty]
    private bool _colletBackwardOutputSignalStatus;

    [ObservableProperty]
    private bool _secondHydraulicsOutputSignalStatus;

    [ObservableProperty]
    private bool _secondValveOutputSignalStatus;

    [ObservableProperty]
    private bool _firstSupportUpOutputSignalStatus;

    [ObservableProperty]
    private bool _secondSupportUpOutputSignalStatus;

    [ObservableProperty]
    private bool _thirdSupportUpOutputSignalStatus;

    [ObservableProperty]
    private bool _fourthSupportUpOutputSignalStatus;

    [ObservableProperty]
    private bool _rotationBrakingOutputSignalStatus;

    [ObservableProperty]
    private bool _guideLubricantOutputSignalStatus;

    [ObservableProperty]
    private bool _dornLubricantOutputSignalStatus;

    [ObservableProperty]
    private ICommand _showOrHideInputOutputTableCommand;

    public InputOutputTableViewModel(IManualConfigurationService configurationService, ISettingsRepository settingsRepository)
    {
        ArgumentNullException.ThrowIfNull(settingsRepository);

        _configurationService = configurationService;

        Settings = settingsRepository.Get() ?? new Settings();
    }

    public void Subscribe()
    {
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CLAMP_FORWARD_SENSOR, value => ClampForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CLAMP_BACKWARD_SENSOR, value => ClampBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PRESS_FORWARD_SENSOR, value => PressForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PRESS_BACKWARD_SENSOR, value => PressBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_SENSOR, value => SqueezeForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_SENSOR, value => SqueezeBackwardSensorStatus = value);
        if (Settings.MultiLeveled)
        {
            _configurationService.Subscribe<bool>(ControllerRequestStrings.SECOND_SQUEEZE_FORWARD_SENSOR, value => SecondSqueezeForwardSensorStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.SECOND_SQUEEZE_BACKWARD_SENSOR, value => SecondSqueezeBackwardSensorStatus = value);
        }
        _configurationService.Subscribe<bool>(ControllerRequestStrings.DORN_FORWARD_SENSOR, value => DornForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.DORN_BACKWARD_SENSOR, value => DornBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.BEND_FORWARD_SENSOR, value => BendForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.BEND_BACKWARD_SENSOR, value => BendBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.COLLET_FORWARD_SENSOR, value => ColletForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.COLLET_BACKWARD_SENSOR, value => ColletBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CONSOLE_FORWARD_SENSOR, value => ConsoleForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CONSOLE_BACKWARD_SENSOR, value => ConsoleBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SUPPLY_BACKWARD_SENSOR, value => SupplyBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SUPPLY_RESET_SENSOR, value => SupplyResetSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ROTATION_RESET_SENSOR, value => RotationResetSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SUPPORT_LOWER_SENSOR, value => SupportLowerSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ADJUSTMENT_UPPER_SENSOR, value => AdjustmentUpperSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ADJUSTMENT_MIDDLE_SENSOR, value => AdjustmentMiddleSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ADJUSTMENT_LOWER_SENSOR, value => AdjustmentLowerSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PUNCHING_FORWARD_SENSOR, value => PunchingForwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PUNCHING_BACKWARD_SENSOR, value => PunchingBackwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PEDAL_SENSOR, value => PedalSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.STOP_BUTTON_SENSOR, value => StopButtonSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.START_BUTTON_SENSOR, value => StartButtonSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ELECTRIC_MOTORS_BREAKER_SENSOR, value => ElectricMotorsBreakerSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FORWARD_PROTECTION_PANEL_SENSOR, value => ForwardProtectionPanelSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.BACKWARD_PROTECTION_PANEL_SENSOR, value => BackwardProtectionPanelSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SPEED_TUMBLER_LEFTWARD_SENSOR, value => SpeedTumblerLeftwardSensorStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SPEED_TUMBLER_RIGHTWARD_SENSOR, value => SpeedTumblerRightwardSensorStatus = value);

        // Выходные сигналы
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CLAMP_FORWARD_OUTPUT_SIGNAL, value => ClampForwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.CLAMP_BACKWARD_OUTPUT_SIGNAL, value => ClampBackwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PRESS_FORWARD_OUTPUT_SIGNAL, value => PressForwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.PRESS_BACKWARD_OUTPUT_SIGNAL, value => PressBackwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_OUTPUT_SIGNAL, value => SqueezeForwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_OUTPUT_SIGNAL, value => SqueezeBackwardOutputSignalStatus = value);
        if (Settings.MultiLeveled)
        {
            _configurationService.Subscribe<bool>(ControllerRequestStrings.ADJUSTMENT_UP_OUTPUT_SIGNAL, value => AdjustmentUpOutputSignalStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.ADJUSTMENT_DOWN_OUTPUT_SIGNAL, value => AdjustmentDownOutputSignalStatus = value);
        }
        if (Settings.WithPunchingCylinder)
        {
            _configurationService.Subscribe<bool>(ControllerRequestStrings.PUNCHING_FORWARD_OUTPUT_SIGNAL, value => PunchingForwardOutputSignalStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.PUNCHING_BACKWARD_OUTPUT_SIGNAL, value => PunchingBackwardOutputSignalStatus = value);
        }
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_HYDRAULICS_OUTPUT_SIGNAL, value => FirstHydraulicsOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_VALVE_OUPUT_SIGNAL, value => FirstValveOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.DORN_FORWARD_OUTPUT_SIGNAL, value => DornForwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.DORN_BACKWARD_OUTPUT_SIGNAL, value => DornBackwardOutputSignalStatus = value);
        if (!Settings.IsElectricBendingDrive)
        {
            _configurationService.Subscribe<bool>(ControllerRequestStrings.BEND_FORWARD_OUTPUT_SIGNAL, value => BendForwardOutputSignalStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.BEND_BACKWARD_OUTPUT_SIGNAL, value => BendBackwardOutputSignalStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.SECOND_HYDRAULICS_OUTPUT_SIGNAL, value => SecondHydraulicsOutputSignalStatus = value);
            _configurationService.Subscribe<bool>(ControllerRequestStrings.SECOND_VALVE_OUTPUT_SIGNAL, value => SecondValveOutputSignalStatus = value);
        }
        _configurationService.Subscribe<bool>(ControllerRequestStrings.COLLET_FORWARD_OUTPUT_SIGNAL, value => ColletForwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.COLLET_BACKWARD_OUTPUT_SIGNAL, value => ColletBackwardOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FIRST_SUPPORT_UP_OUTPUT_SIGNAL, value => FirstSupportUpOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.SECOND_SUPPORT_UP_OUTPUT_SIGNAL, value => SecondSupportUpOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.THIRD_SUPPORT_UP_OUTPUT_SIGNAL, value => ThirdSupportUpOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.FOURTH_SUPPORT_UP_OUTPUT_SIGNAL, value => FourthSupportUpOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.ROTATION_BRAKING_OUTPUT_SIGNAL, value => RotationBrakingOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.GUIDE_LUBRICANT_OUTPUT_SIGNAL, value => GuideLubricantOutputSignalStatus = value);
        _configurationService.Subscribe<bool>(ControllerRequestStrings.DORN_LUBRICANT_OUTPUT_SIGNAL, value => DornLubricantOutputSignalStatus = value);
    }

    public void Unsubscribe()
    {
        _configurationService.Unsubscribe(ControllerRequestStrings.CLAMP_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.CLAMP_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.PRESS_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.PRESS_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_SENSOR);
        if (Settings.MultiLeveled)
        {
            _configurationService.Unsubscribe(ControllerRequestStrings.SECOND_SQUEEZE_FORWARD_SENSOR);
            _configurationService.Unsubscribe(ControllerRequestStrings.SECOND_SQUEEZE_BACKWARD_SENSOR);
        }
        _configurationService.Unsubscribe(ControllerRequestStrings.DORN_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.DORN_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.BEND_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.BEND_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.COLLET_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.COLLET_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.CONSOLE_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.CONSOLE_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.SUPPLY_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.SUPPLY_RESET_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.ROTATION_RESET_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.SUPPORT_LOWER_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.ADJUSTMENT_UPPER_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.ADJUSTMENT_MIDDLE_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.ADJUSTMENT_LOWER_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.PUNCHING_FORWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.PUNCHING_BACKWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.PEDAL_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.STOP_BUTTON_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.START_BUTTON_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.ELECTRIC_MOTORS_BREAKER_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.FORWARD_PROTECTION_PANEL_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.BACKWARD_PROTECTION_PANEL_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.SPEED_TUMBLER_LEFTWARD_SENSOR);
        _configurationService.Unsubscribe(ControllerRequestStrings.SPEED_TUMBLER_RIGHTWARD_SENSOR);

        // Выходные сигналы
        _configurationService.Unsubscribe(ControllerRequestStrings.CLAMP_FORWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.CLAMP_BACKWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.PRESS_FORWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.PRESS_BACKWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_OUTPUT_SIGNAL);
        if (Settings.MultiLeveled)
        {
            _configurationService.Unsubscribe(ControllerRequestStrings.ADJUSTMENT_UP_OUTPUT_SIGNAL);
            _configurationService.Unsubscribe(ControllerRequestStrings.ADJUSTMENT_DOWN_OUTPUT_SIGNAL);
        }
        if (Settings.WithPunchingCylinder)
        {
            _configurationService.Unsubscribe(ControllerRequestStrings.PUNCHING_FORWARD_OUTPUT_SIGNAL);
            _configurationService.Unsubscribe(ControllerRequestStrings.PUNCHING_BACKWARD_OUTPUT_SIGNAL);
        }
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_HYDRAULICS_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_VALVE_OUPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.DORN_FORWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.DORN_BACKWARD_OUTPUT_SIGNAL);
        if (!Settings.IsElectricBendingDrive)
        {
            _configurationService.Unsubscribe(ControllerRequestStrings.BEND_FORWARD_OUTPUT_SIGNAL);
            _configurationService.Unsubscribe(ControllerRequestStrings.BEND_BACKWARD_OUTPUT_SIGNAL);
            _configurationService.Unsubscribe(ControllerRequestStrings.SECOND_HYDRAULICS_OUTPUT_SIGNAL);
            _configurationService.Unsubscribe(ControllerRequestStrings.SECOND_VALVE_OUTPUT_SIGNAL);
        }
        _configurationService.Unsubscribe(ControllerRequestStrings.COLLET_FORWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.COLLET_BACKWARD_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.FIRST_SUPPORT_UP_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.SECOND_SUPPORT_UP_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.THIRD_SUPPORT_UP_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.FOURTH_SUPPORT_UP_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.ROTATION_BRAKING_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.GUIDE_LUBRICANT_OUTPUT_SIGNAL);
        _configurationService.Unsubscribe(ControllerRequestStrings.DORN_LUBRICANT_OUTPUT_SIGNAL);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            Unsubscribe();
        }

        _disposed = true;
    }
}
