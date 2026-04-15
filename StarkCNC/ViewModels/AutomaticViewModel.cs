using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if !DEBUG
using System.Windows;
#endif

namespace StarkCNC.ViewModels;

public partial class AutomaticViewModel : ViewModelBase, IDisposable
{
    private IManualConfigurationService _configurationService;
    private IBendingDataUnitOfWork _unitOfWork;

    private bool _disposed;

    [ObservableProperty]
    private Settings _settings;

    [ObservableProperty]
    private string _programName = string.Empty;

    [ObservableProperty]
    private string _operator = string.Empty;

    [ObservableProperty]
    private double _speed;

    [ObservableProperty]
    private float _cycleTime;

    [ObservableProperty]
    private double _pipeLength;

    [ObservableProperty]
    private double _setUpPoint;

    [ObservableProperty]
    private int _countCompletedDetails;

    [ObservableProperty]
    private int _taskDetails;

    [ObservableProperty]
    private bool _canChangeCountDetails = true;

    [ObservableProperty]
    private bool _isFullAtomatic;

    [ObservableProperty]
    private double _pipeInstallationDelay;

    [ObservableProperty]
    private double _currentTaskSupply;

    [ObservableProperty]
    private double _currentTaskRotationAngle;

    [ObservableProperty]
    private double _currentTaskBendingAngle;

    [ObservableProperty]
    private double _facticalSupply;

    [ObservableProperty]
    private double _facticalRotationAngle;

    [ObservableProperty]
    private float _facticalBendingAngle;

    [ObservableProperty]
    private double _factialConsole;

    [ObservableProperty]
    private bool _sendData;

    [ObservableProperty]
    private bool _hasErrors;

    [ObservableProperty]
    private bool _showInputOutputTable;

    // Таблица входов // TODO: Вынести в отдельный TableInputOutputViewModel
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

    //Таблица выходов // TODO: Вынести в отдельный TableInputOutputViewModel
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

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _updateInputOutputSignals;
    private CancellationTokenSource? _cancellationTokenSourceInputOutputSignals;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable. Выполняется в Initialize()
    public AutomaticViewModel(IManualConfigurationService configurationService, IBendingDataUnitOfWork unitOfWork, IUserService userService, ISettingsRepository settingsRepository)
    {
        _configurationService = configurationService;
        _unitOfWork = unitOfWork;

        ProgramName = _unitOfWork.ProgramName;
        PipeLength = _unitOfWork.PipeLength;
        SetUpPoint = _unitOfWork.SetUpPoint;

        Operator = userService?.CurrentUser?.UserName ?? string.Empty;

        int i = 1;
        foreach (var item in _unitOfWork.BendingDatas)
        {
            BendingDatas.Add(new BendingDataViewModel(item) { Id = i });
            i++;
        }

        BendingDatas.CollectionChanged += BendingDatas_CollectionChanged;

        Initialize(settingsRepository);

        StartUpdateTask();
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable. Выполняется в Initialize()

    private async void Initialize(ISettingsRepository? settingsRepository)
    {
        if (settingsRepository is null)
            throw new ArgumentNullException(nameof(settingsRepository));

        Settings = await settingsRepository.GetAsync().ConfigureAwait(true) ?? new Settings();
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task SetAutomaticMode()
    {
        await _configurationService
            .WriteAsync(true, ControllerRequestStrings.AUTOMATIC_TAGS_TURN_ON)
            .ConfigureAwait(false);
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ClearActuatorErrors()
    {
        await _configurationService
            .WriteAsync(true, ControllerRequestStrings.ERRORS_CLEAR_ACTUATOR_ERRORS)
            .ConfigureAwait(false);

        HasErrors = false;
    }

    [RelayCommand]
    private void ShowOrHideInputOutputTable()
    {
        if (ShowInputOutputTable)
            ShowInputOutputTable = false;
        else
            ShowInputOutputTable = true;
    }

    private void StartUpdateTask()
    {
        if (TaskIsRunning())
            return;

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _updateTask = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    HasErrors = await _configurationService
                        .ReadAsync<bool>(ControllerRequestStrings.ERRORS_HAS_ERRORS)
                        .ConfigureAwait(false);
                    CycleTime = await _configurationService
                        .ReadAsync<float>(ControllerRequestStrings.AUTOMATIC_TAGS_CYCLE_TIME)
                        .ConfigureAwait(false);
                    SetSendData(await _configurationService
                        .ReadAsync<bool>(ControllerRequestStrings.AUTOMATIC_TAGS_SEND_DATA)
                        .ConfigureAwait(false), true);
                    CountCompletedDetails = await _configurationService
                        .ReadAsync<int>(ControllerRequestStrings.AUTOMATIC_TAGS_COUNT_COMPLETED_DETAILS)
                        .ConfigureAwait(false);

                    FacticalSupply = await _configurationService
                        .ReadAsync<double>(ControllerRequestStrings.SUPPLY_FACTICAL_POSITION)
                        .ConfigureAwait(false);

                    FacticalRotationAngle = await _configurationService
                        .ReadAsync<double>(ControllerRequestStrings.ROTATION_FACTICAL_POSITION)
                        .ConfigureAwait(false);

                    FacticalBendingAngle = await _configurationService
                        .ReadAsync<float>(ControllerRequestStrings.BEND_FACTICAL_POSITION)
                        .ConfigureAwait(false);

                    FactialConsole = await _configurationService
                        .ReadAsync<double>(ControllerRequestStrings.CONSOLE_FACTICAL_POSITION)
                        .ConfigureAwait(false);

                    await Task.Delay(150).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
                // Нормально: задача отменена
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateTask error: {ex}");
            }
        }, token);
    }

    private bool TaskIsRunning() =>
        _updateTask is not null && !_updateTask.IsCompleted && !_updateTask.IsCanceled && !_updateTask.IsFaulted;

    private void StopUpdateTask()
    {
        if (_updateTask is null)
            return;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    private void SetSendData(bool value, bool isUpdateTask = false)
    {
        SendData = value;
        if (!isUpdateTask)
            return;

        if (SendData == true)
        {
            CanChangeCountDetails = false;
            RunProgram();
        }
    }

    private async void RunProgram()
    {
        // Send data
        await _configurationService
            .WriteAsync<int>(BendingDatas.Count, ControllerRequestStrings.AUTOMATIC_TAGS_ALL_BEND)
            .ConfigureAwait(false);

        int step = 1;
        foreach (var data in BendingDatas)
        {
            CurrentTaskSupply = data.Supply;
            CurrentTaskRotationAngle = data.RotationAngle;
            CurrentTaskBendingAngle = data.BendingAngle;

            await _configurationService
                .WriteAsync<int>(step, ControllerRequestStrings.AUTOMATIC_TAGS_STEP_NUMBER)
                .ConfigureAwait(false);

            await _configurationService
               .WriteAsync<double>(data.Supply, ControllerRequestStrings.SUPPLY_VALUE)
               .ConfigureAwait(false);
            await _configurationService
                .WriteAsync<double>(data.RotationAngle, ControllerRequestStrings.ROTATION_VALUE)
                .ConfigureAwait(false);
            await _configurationService
                .WriteAsync<double>(data.BendingAngle, ControllerRequestStrings.BEND_VALUE)
                .ConfigureAwait(false);
            step += 1;
        }

        // Finish
        await _configurationService
            .WriteAsync<bool>(true, ControllerRequestStrings.AUTOMATIC_TAGS_END_PROGRAM)
            .ConfigureAwait(false);

        await _configurationService
            .WriteAsync<bool>(false, ControllerRequestStrings.AUTOMATIC_TAGS_SEND_DATA)
            .ConfigureAwait(false);
        SetSendData(false);
    }

    private void BendingDatas_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        int i = 1;
        foreach (var item in BendingDatas)
        {
            item.Id = i;
            i++;
        }
    }

    async partial void OnIsFullAtomaticChanged(bool oldValue, bool newValue)
    {
        await _configurationService.WriteAsync<bool>(newValue, ControllerRequestStrings.AUTOMATIC_TAGS_FULL_AUTOMATIC)
            .ConfigureAwait(false);
    }

    async partial void OnPipeInstallationDelayChanged(double oldValue, double newValue)
    {
        if (IsFullAtomatic)
        {
            await _configurationService.WriteAsync<double>(newValue, ControllerRequestStrings.AUTOMATIC_TAGS_DELAY)
                .ConfigureAwait(false);
        }
    }

    partial void OnPipeLengthChanged(double oldValue, double newValue)
    {
        _unitOfWork.PipeLength = newValue;
        _unitOfWork.HasUnsavedData = true;
        _unitOfWork.SaveFile();
    }

    partial void OnSetUpPointChanged(double oldValue, double newValue)
    {
        _unitOfWork.SetUpPoint = newValue;
        _unitOfWork.HasUnsavedData = false;
        _unitOfWork.SaveFile();
    }

    partial void OnCountCompletedDetailsChanged(int oldValue, int newValue)
    {
        if (newValue >= TaskDetails)
        {
#if !DEBUG
            var doneMessageBox = MessageBox.Show("Задание выполнено", "Задание выполнено", MessageBoxButton.OK, MessageBoxImage.Information);
#endif
            CanChangeCountDetails = true;
        }
    }

    partial void OnShowInputOutputTableChanged(bool value)
    {
        if (value)
            StartReadInputOutputSignals();
        else
            StopReadInputOutputSignals();
    }

    private void StartReadInputOutputSignals()
    {
        if (TaskUpdateInputOutputSignalsIsRunning())
            return;

        _cancellationTokenSourceInputOutputSignals?.Dispose();

        _cancellationTokenSourceInputOutputSignals = new CancellationTokenSource();
        var token = _cancellationTokenSourceInputOutputSignals.Token;

        _updateInputOutputSignals = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // Входные сигналы
                    ClampForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CLAMP_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ClampBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CLAMP_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    PressForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PRESS_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    PressBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PRESS_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SqueezeForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SqueezeBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    if (Settings.MultiLeveled)
                    {
                        SecondSqueezeForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SECOND_SQUEEZE_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                        SecondSqueezeBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SECOND_SQUEEZE_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    }
                    DornForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.DORN_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    DornBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.DORN_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    BendForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.BEND_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    BendBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.BEND_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ColletForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.COLLET_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ColletBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.COLLET_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ConsoleForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CONSOLE_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ConsoleBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CONSOLE_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SupplyBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SUPPLY_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SupplyResetSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SUPPLY_RESET_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    RotationResetSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ROTATION_RESET_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SupportLowerSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SUPPORT_LOWER_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    AdjustmentUpperSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ADJUSTMENT_UPPER_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    AdjustmentMiddleSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ADJUSTMENT_MIDDLE_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    AdjustmentLowerSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ADJUSTMENT_LOWER_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    PunchingForwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PUNCHING_FORWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    PunchingBackwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PUNCHING_BACKWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    PedalSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PEDAL_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    StopButtonSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.STOP_BUTTON_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    StartButtonSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.START_BUTTON_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ElectricMotorsBreakerSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ELECTRIC_MOTORS_BREAKER_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    ForwardProtectionPanelSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FORWARD_PROTECTION_PANEL_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    BackwardProtectionPanelSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.BACKWARD_PROTECTION_PANEL_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SpeedTumblerLeftwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SPEED_TUMBLER_LEFTWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);
                    SpeedTumblerRightwardSensorStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SPEED_TUMBLER_RIGHTWARD_SENSOR, StatusPage.Automatic).ConfigureAwait(true);

                    // Выходные сигналы
                    ClampForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CLAMP_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    ClampBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.CLAMP_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    PressForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PRESS_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    PressBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PRESS_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    SqueezeForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_SQUEEZE_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    SqueezeBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_SQUEEZE_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    if (Settings.MultiLeveled)
                    {
                        AdjustmentUpOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ADJUSTMENT_UP_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                        AdjustmentDownOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ADJUSTMENT_DOWN_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    }
                    if (Settings.WithPunchingCylinder)
                    {
                        PunchingForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PUNCHING_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                        PunchingBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.PUNCHING_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    }
                    FirstHydraulicsOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_HYDRAULICS_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    FirstValveOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_VALVE_OUPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    DornForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.DORN_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    DornBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.DORN_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    if (!Settings.IsElectricBendingDrive)
                    {
                        BendForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.BEND_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                        BendBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.BEND_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                        SecondHydraulicsOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SECOND_HYDRAULICS_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                        SecondValveOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SECOND_VALVE_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    }
                    ColletForwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.COLLET_FORWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    ColletBackwardOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.COLLET_BACKWARD_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    FirstSupportUpOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FIRST_SUPPORT_UP_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    SecondSupportUpOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.SECOND_SUPPORT_UP_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    ThirdSupportUpOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.THIRD_SUPPORT_UP_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    FourthSupportUpOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.FOURTH_SUPPORT_UP_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    RotationBrakingOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.ROTATION_BRAKING_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    GuideLubricantOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.GUIDE_LUBRICANT_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);
                    DornLubricantOutputSignalStatus = await _configurationService.ReadAsync<bool>(ControllerRequestStrings.DORN_LUBRICANT_OUTPUT_SIGNAL, StatusPage.Automatic).ConfigureAwait(true);

                    await Task.Delay(150).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
                // Нормально: задача отменена
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReadInputOutputSignals error: {ex}");
            }
        });
    }

    private bool TaskUpdateInputOutputSignalsIsRunning() =>
        _updateInputOutputSignals is not null && !_updateInputOutputSignals.IsCompleted && !_updateInputOutputSignals.IsCanceled && !_updateInputOutputSignals.IsFaulted;

    private void StopReadInputOutputSignals()
    {
        if (_updateInputOutputSignals is null)
            return;

        _cancellationTokenSourceInputOutputSignals?.Cancel();
        _cancellationTokenSourceInputOutputSignals?.Dispose();
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
            StopUpdateTask();
            StopReadInputOutputSignals();
        }

        _disposed = true;
    }
}
