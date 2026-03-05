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

    private string _manualModeRequestString = string.Empty;

    private string _bendAndSqueezeRequestString = string.Empty;

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

        _manualModeRequestString = _configuration.GetSection("MachineController").GetSection(nameof(_manualModeRequestString)).Get<string>() ?? string.Empty;
        FeedDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(FeedDrive), true);
        TurnDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(TurnDrive), true);
        ConsoleDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), manualService, nameof(ConsoleDrive), true);

        Clamp = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Clamp), true);
        Press = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Press), true);
        FirstSqueeze = SqueezeParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(FirstSqueeze), true);
        Bend = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Bend), true);
        Collet = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Collet), true);
        Dorn = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Dorn), true);
        Adjustment = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Adjustment), _moreThenOneLevel);
        Punching = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), manualService, nameof(Punching), _punchingEnabled);

        FirstHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(FirstHydraulics), _firstHydraulicsEnabled);
        SecondHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(SecondHydraulics), _secondHydraulicsEnabled);
        Support = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(Support));
        DornLubricant = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), manualService, nameof(DornLubricant));
        
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
            .WriteAsync<bool>(true, _manualModeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task ManualModeTurnOff() =>
        await _manualService
            .WriteAsync<bool>(false, _manualModeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task BendAndSqueezeRun() =>
        await _manualService
            .WriteAsync<bool>(true, _bendAndSqueezeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task BendAndSqueezeCancel() =>
        await _manualService
            .WriteAsync<bool>(false, _bendAndSqueezeRequestString)
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

        if (_settings.IsElectricBendingDrive)
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

        if (_settings.IsElectricBendingDrive || _settings.IsElectricBendingDrive)
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

        if (_settings.WithPunchingCylinder)
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
        MoreThenOneLevel = _settings?.MultiLeveled ?? false;
        if (MoreThenOneLevel)
            Adjustment.StartUpdateTask();
        else
            Adjustment.StopUpdateTask();
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