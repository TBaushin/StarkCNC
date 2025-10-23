using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.DTO;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;

namespace StarkCNC.ViewModels;

public partial class ManualViewModel : ObservableObject
{
    private readonly IConfiguration _configuration;
    private readonly IManualConfigurationService _configurationService;
    private readonly SettingsDto _settings;

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

    public ManualViewModel(IConfiguration configuration, IManualConfigurationService configurationService, SettingsViewModel settingsViewModel)
    {
        _configuration = configuration;
        _configurationService = configurationService;
        _settings = settingsViewModel.Settings;

        Connect();

        ManualModeRequestString = _configuration.GetSection("MachineController").GetSection(nameof(ManualModeRequestString)).Get<string>() ?? string.Empty;
        FeedDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(FeedDrive), true);
        TurnDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(TurnDrive), true);
        ConsoleDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(ConsoleDrive), true);

        Clamp = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Clamp), true);
        Press = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Press), true);
        FirstSqueeze = SqueezeParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(FirstSqueeze), true);
        Bend = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Bend), true);
        Collet = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Collet), true);
        Dorn = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Dorn), true);
        Adjustment = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Adjustment), _moreThenOneLevel);
        Punching = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Punching), _punchingEnabled);

        FirstHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(FirstHydraulics), _firstHydraulicsEnabled);
        SecondHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(SecondHydraulics), _secondHydraulicsEnabled);
        Support = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(Support));
        DornLubricant = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(DornLubricant));
        BendAndSqueeze = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(BendAndSqueeze));

        DefineFirstHydraulicsStatus();
        DefineSecondHydraulicsStatus();
        DefinePunchingStatus();
        DefineMoreThanOneLevelStatus();

        _settings.PropertyChanged += _settingsService_PropertyChanged;
    }

    private async void Connect()
    {
        if (!_configurationService.Connected)
            await _configurationService.ConnectAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task ManualModeTurnOn() =>
        await _configurationService
            .WriteAsync<bool>(true, ManualModeRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task ManualModeTurnOff() =>
        await _configurationService
            .WriteAsync<bool>(false, ManualModeRequestString)
            .ConfigureAwait(false);

    private void DefineFirstHydraulicsStatus()
    {
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
        if (_settings.FloorType != FloorType.SingleLevel)
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

    private void _settingsService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        DefineFirstHydraulicsStatus();
        DefineSecondHydraulicsStatus();
        DefinePunchingStatus();
        DefineMoreThanOneLevelStatus();
    }
}
