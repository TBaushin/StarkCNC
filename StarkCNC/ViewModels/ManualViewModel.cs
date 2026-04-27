using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;
using System.Diagnostics;

namespace StarkCNC.ViewModels;

public partial class ManualViewModel : ViewModelBase, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IManualConfigurationService _manualService;
    private readonly ISettingsRepository _settingsRepository;
    private Settings? _settings;

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
        watch.Stop();
        Debug.WriteLine($"InitializeAsync: {watch.Elapsed.ToString()}");
    }

    private void Subscribe()
    {
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var factialSection = automaticTagsSection.GetSection("Factial");

        var stopErrorRequestString = automaticTagsSection
            .GetSection("StopErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        _manualService.Subscribe<bool>(stopErrorRequestString, value => HasErrors = value);
    }

    private void Unsubscribe()
    {
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var factialSection = automaticTagsSection.GetSection("Factial");

        var stopErrorRequestString = automaticTagsSection
            .GetSection("StopErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        _manualService.Unsubscribe(stopErrorRequestString);
    }

    [RelayCommand]
    private async Task ManualModeTurnOn() =>
        await _manualService
            .WriteAsync<bool>(true, _manualModeRequestString)
            .ConfigureAwait(true);

    [RelayCommand]
    private async Task ManualModeTurnOff() =>
        await _manualService
            .WriteAsync<bool>(false, _manualModeRequestString)
            .ConfigureAwait(true);

    [RelayCommand]
    private async Task BendAndSqueezeRun() =>
        await _manualService
            .WriteAsync<bool>(true, _bendAndSqueezeRequestString)
            .ConfigureAwait(true);

    [RelayCommand]
    private async Task BendAndSqueezeCancel() =>
        await _manualService
            .WriteAsync<bool>(false, _bendAndSqueezeRequestString)
            .ConfigureAwait(true);

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

        if (_settings.IsElectricBendingDrive || _settings.IsElectricBendingDrive)
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
        }

        _disposed = true;
    }
}