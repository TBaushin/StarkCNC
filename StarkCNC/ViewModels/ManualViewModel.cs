using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;
using StarkCNC.Utilities;
using System.Diagnostics;

namespace StarkCNC.ViewModels;

public partial class ManualViewModel : ViewModelBase, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IManualConfigurationService _manualService;
    private readonly ISettingsRepository _settingsRepository;
    private Settings? _settings;

    private bool _disposed;
    private bool _subscribed;

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

    public ManualViewModel(IConfiguration configuration, IManualConfigurationService manualService, ISettingsRepository settingsRepository)
    {
        var watch = Stopwatch.StartNew();
        _configuration = configuration;
        _manualService = manualService;
        _settingsRepository = settingsRepository;

        _manualModeRequestString = _configuration.GetSection("MachineController").GetSection(nameof(_manualModeRequestString)).Get<string>() ?? string.Empty;
        FeedDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), _manualService, nameof(FeedDrive), true);
        TurnDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), _manualService, nameof(TurnDrive), true);
        ConsoleDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), _manualService, nameof(ConsoleDrive), true);

        Clamp = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Clamp), true);
        Press = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Press), true);
        FirstSqueeze = SqueezeParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(FirstSqueeze), true);
        Bend = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Bend), true);
        Collet = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Collet), true);
        Dorn = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Dorn), true);
        Adjustment = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Adjustment), _moreThenOneLevel);
        Punching = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), _manualService, nameof(Punching), _punchingEnabled);

        FirstHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), _manualService, nameof(FirstHydraulics), _firstHydraulicsEnabled);
        SecondHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), _manualService, nameof(SecondHydraulics), _secondHydraulicsEnabled);
        Support = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), _manualService, nameof(Support));
        DornLubricant = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), _manualService, nameof(DornLubricant));
        watch.Stop();
        Debug.WriteLine($"Constructor: {watch.Elapsed.ToString()}");
    }

    public async Task InitializeAsync()
    {
        var watch = Stopwatch.StartNew();
        _settings = await _settingsRepository.GetAsync().ConfigureAwait(true);

        var connected = _manualService.Connected;
        await Task.Run(() =>
        {
            while (connected != true || _subscribed != true)
            {
                connected = _manualService.Connected;
                if (!connected)
                {
                    Task.Delay(250);
                    continue;
                }

                Subscribe();

                DefineFirstHydraulicsStatus();
                DefineSecondHydraulicsStatus();
                DefinePunchingStatus();
                DefineMoreThanOneLevelStatus();

                FeedDrive.Subscribe();
                TurnDrive.Subscribe();
                ConsoleDrive.Subscribe();

                Clamp.Subscribe();
                Press.Subscribe();
                FirstSqueeze.Subscribe();
                Bend.Subscribe();
                Collet.Subscribe();
                Dorn.Subscribe();
                // Adjustment.Subscribe(); Подписывается в DefineMoreThanOneLevelStatus
                // Punching.Subscribe(); Подписывается в DefinePunchingStatus

                // FirstHydraulics.Subscribe(); Подписывается в DefineFirstHydraulicsStatus
                // SecondHydraulics.Subscribe(); Подписывается в DefineSecondHydraulicsStatus
                Support.Subscribe();
                DornLubricant.Subscribe();

                Task.Delay(250);
                _subscribed = true;
            }
        }).ConfigureAwait(false);
        
        watch.Stop();
        Debug.WriteLine($"InitializeAsync: {watch.Elapsed.ToString()}");
    }

    private void Subscribe()
    {
        _manualService.Subscribe<bool>(ControllerRequestStrings.ERRORS_HAS_ERRORS, value => HasErrors = value);
    }

    private void Unsubscribe()
    {
        _manualService.Unsubscribe(ControllerRequestStrings.ERRORS_HAS_ERRORS);
    }

    [RelayCommand]
    private async Task UpdateManualMode()
    {
        var manualMode = await _manualService.ReadAsync<bool>(ControllerRequestStrings.MANUAL_MODE).ConfigureAwait(true);
        await _manualService
            .WriteAsync<bool>(!manualMode, ControllerRequestStrings.MANUAL_MODE)
            .ConfigureAwait(true);
    }

    public async Task UpdateManualModeWithValue(bool value)
    {
        await _manualService
            .WriteAsync<bool>(value, ControllerRequestStrings.MANUAL_MODE)
            .ConfigureAwait(true);
    }

    [RelayCommand]
    private async Task BendAndSqueezeRun() =>
        await _manualService
            .WriteAsync<bool>(true, ControllerRequestStrings.BEND_AND_SQUEEZE_VALUE)
            .ConfigureAwait(true);

    [RelayCommand]
    private async Task BendAndSqueezeCancel() =>
        await _manualService
            .WriteAsync<bool>(false, ControllerRequestStrings.BEND_AND_SQUEEZE_VALUE)
            .ConfigureAwait(true);

    [RelayCommand]
    private async Task ClearActuatorErrors()
    {
        await _manualService
            .WriteAsync(true, ControllerRequestStrings.ERRORS_CLEAR_ACTUATOR_ERRORS)
            .ConfigureAwait(true);

        HasErrors = false;
    }

    private void DefineFirstHydraulicsStatus()
    {
        if (_settings is null)
            return;

        if (_settings.IsElectricBendingDrive)
        {
            FirstHydraulicsEnabled = false;
            FirstHydraulics.Unsubscribe();
        }
        else
        {
            FirstHydraulicsEnabled = true;
            FirstHydraulics.Subscribe();
        }
    }

    private void DefineSecondHydraulicsStatus()
    {
        if (_settings is null)
            return;

        if (_settings.IsElectricBendingDrive) // Был ещё IsElectricMachine
        {
            SecondHydraulicsEnabled = false;
            SecondHydraulics.Unsubscribe();
        }
        else
        {
            SecondHydraulicsEnabled = true;
            SecondHydraulics.Subscribe();
        }
    }

    private void DefinePunchingStatus()
    {
        if (_settings is null)
            return;

        if (_settings.WithPunchingCylinder)
        {
            PunchingEnabled = false;
            Punching.Unsubscribe();
        }
        else
        {
            PunchingEnabled = true;
            Punching.Subscribe();
        }
    }

    private void DefineMoreThanOneLevelStatus()
    {
        MoreThenOneLevel = _settings?.MultiLeveled ?? false;
        if (MoreThenOneLevel)
            Adjustment.Subscribe();
        else
            Adjustment.Unsubscribe();
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
            FeedDrive.Dispose();
            TurnDrive.Dispose();
            ConsoleDrive.Dispose();

            Clamp.Dispose();
            Press.Dispose();
            FirstSqueeze.Dispose();
            Bend.Dispose();
            Collet.Dispose();
            Dorn.Dispose();
            Adjustment.Dispose();
            Punching.Dispose();

            FirstHydraulics.Dispose();
            SecondHydraulics.Dispose();
            Support.Dispose();
            DornLubricant.Dispose();

            Unsubscribe();

            _subscribed = false;
        }

        _disposed = true;
    }
}