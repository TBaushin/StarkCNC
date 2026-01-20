using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;

namespace StarkCNC.ViewModels;

public partial class ManualViewModel : ObservableObject, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IManualConfigurationService _manualService;
    private readonly Settings? _settings;

    private bool _disposed;

    public string ManualModeRequestString { get; set; } = string.Empty;

    public DriveParameters FeedDrive { get; set; }

    public DriveParameters TurnDrive { get; set; }

    public DriveParameters ConsoleDrive { get; set; }

    public OutputsParametersTwoButtons Clamp { get; set; }
    
    public OutputsParametersTwoButtons Press { get; set; }

    public SqueezeParameters FirstSqueeze { get; set; }

    public OutputsParametersTwoButtons Bend { get; set; }

    public OutputsParametersTwoButtons Collet { get; set; }

    public OutputsParametersTwoButtons Dorn { get; set; }

    public OutputsParametersTwoButtons Adjustment { get; set; }

    public OutputsParametersTwoButtons Punching { get; set; }

    public OutputsParametersSwitch FirstHydraulics { get; set; }
    
    public OutputsParametersSwitch SecondHydraulics { get; set; }

    public OutputsParametersSwitch Support { get; set; }

    public OutputsParametersSwitch DornLubricant { get; set; }

    public OutputsParametersSwitch BendAndSqueeze { get; set; }

    [ObservableProperty]
    private bool _firstHydraulicsEnabled = false;

    [ObservableProperty]
    private bool _secondHydraulicsEnabled = false;

    [ObservableProperty]
    private bool _punchingEnabled = false;

    [ObservableProperty]
    private bool _moreThenOneLevel = false;

    [ObservableProperty]
    private bool _hasErrors = false;

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public ManualViewModel(IConfiguration configuration, IManualConfigurationService manualService, ISettingsRepository settingsRepository)
    {
        _configuration = configuration;
        _manualService = manualService;
        _settings = settingsRepository?.GetAsync().Result;

        Connect();

        ManualModeRequestString = App.Configuration.GetSection("MachineController").GetSection(nameof(ManualModeRequestString)).Get<string>() ?? string.Empty;
        FeedDrive = DriveParameters.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(FeedDrive), true);
        TurnDrive = DriveParameters.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(TurnDrive), true);
        ConsoleDrive = DriveParameters.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(ConsoleDrive), true);

        Clamp = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Clamp), true);
        Press = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Press), true);
        FirstSqueeze = SqueezeParameters.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(FirstSqueeze), true);
        Bend = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Bend), true);
        Collet = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Collet), true);
        Dorn = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Dorn), true);
        Adjustment = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Adjustment), _moreThenOneLevel);
        Punching = OutputsParametersTwoButtons.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Punching), _punchingEnabled);

        FirstHydraulics = OutputsParametersSwitch.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(FirstHydraulics), _firstHydraulicsEnabled);
        SecondHydraulics = OutputsParametersSwitch.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(SecondHydraulics), _secondHydraulicsEnabled);
        Support = OutputsParametersSwitch.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(Support));
        DornLubricant = OutputsParametersSwitch.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(DornLubricant));
        BendAndSqueeze = OutputsParametersSwitch.InitializeParameters(App.Configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(BendAndSqueeze));

        DefineFirstHydraulicsStatus();
        DefineSecondHydraulicsStatus();
        DefinePunchingStatus();
        DefineMoreThanOneLevelStatus();

        StartUpdateTask();
    }

    private async void Connect()
    {
        if (!_manualService.Connected)
            await _manualService.ConnectAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task ManualModeTurnOn() =>
        await _manualService
            .WriteAsync<bool>(true, ManualModeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task ManualModeTurnOff() =>
        await _manualService
            .WriteAsync<bool>(false, ManualModeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task ClearActuatorErrors()
    {
        var machineController = _configuration.GetSection("MachineController");
        var clearActuatorErrorsRequestString = machineController
            .GetSection("ClearActuatorErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        await _manualService
            .WriteAsync(true, clearActuatorErrorsRequestString)
            .ConfigureAwait(false);

        HasErrors = false;
    }

    private void StartUpdateTask()
    {
        if (TaskIsRunning())
            return;

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var factialSection = automaticTagsSection.GetSection("Factial");

        var stopErrorRequestString = automaticTagsSection
            .GetSection("StopErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var cycleTimeRequestString = automaticTagsSection
            .GetSection("CycleTime")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var sendDataRequestString = automaticTagsSection
            .GetSection("SendData")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalSupplyRequestString = factialSection
            .GetSection("Supply")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalRotationReuqestString = factialSection
            .GetSection("Rotation")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalBendingRequestString = factialSection
            .GetSection("Bending")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalConsoleRequestString = factialSection
            .GetSection("Console")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        _updateTask = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    HasErrors = await _manualService
                        .ReadAsync<bool>(stopErrorRequestString)
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
                Console.WriteLine($"UpdateTask error: {ex}");
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

    private void DefineFirstHydraulicsStatus()
    {
        if (_settings is null)
            return;

        if (_settings.IsElectricMachine)
        {
            FirstHydraulicsEnabled = false;
            FirstHydraulics.StopUpdateTask();
        }
        else
        {
            FirstHydraulicsEnabled = true;
            FirstHydraulics.StartUpdateTask();
        }
    }

    private void DefineSecondHydraulicsStatus()
    {
        if (_settings is null)
            return;

        if (_settings.IsElectricMachine || _settings.IsElectricBendingDrive)
        {
            SecondHydraulicsEnabled = false;
            SecondHydraulics.StopUpdateTask();
        }
        else
        {
            SecondHydraulicsEnabled = true;
            SecondHydraulics.StartUpdateTask();
        }
    }

    private void DefinePunchingStatus()
    {
        if (_settings is null)
            return;

        if (_settings.IsPunchingCylinder)
        {
            PunchingEnabled = false;
            Punching.StopUpdateTask();
        }
        else
        {
            PunchingEnabled = true;
            Punching.StartUpdateTask();
        }
    }

    private void DefineMoreThanOneLevelStatus()
    {
        if (_settings?.FloorType != FloorType.SingleLevel)
        {
            MoreThenOneLevel = true;
            Adjustment.StartUpdateTask();
        }
        else
        {
            MoreThenOneLevel = false;
            Adjustment.StopUpdateTask();
        }
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
            StopUpdateTask();

        _disposed = true;
    }
}