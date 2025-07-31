using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;

namespace StarkCNC.ViewModels
{
    public partial class ManualViewModel : ObservableObject
    {
        private readonly IConfiguration _configuration;
        private readonly IManualConfigurationService _configurationService;

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

        public ManualViewModel(IConfiguration configuration, IManualConfigurationService configurationService)
        {
            _configuration = configuration;
            _configurationService = configurationService;

            Connect();

            ManualModeRequestString = _configuration.GetSection("MachineController").GetSection(nameof(ManualModeRequestString)).Get<string>() ?? string.Empty;
            FeedDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(FeedDrive));
            TurnDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(TurnDrive));
            ConsoleDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), configurationService, nameof(ConsoleDrive));

            Clamp = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Clamp));
            Press = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Press));
            FirstSqueeze = SqueezeParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(FirstSqueeze));
            Bend = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Bend));
            Collet = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Collet));
            Dorn = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Dorn));
            Adjustment = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Adjustment));
            Punching = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), configurationService, nameof(Punching));

            FirstHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(FirstHydraulics));
            SecondHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(SecondHydraulics));
            Support = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(Support));
            DornLubricant = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(DornLubricant));
            BendAndSqueeze = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), configurationService, nameof(BendAndSqueeze));
        }

        private async void Connect()
        {
            if (!_configurationService.Connected)
                await _configurationService.ConnectAsync();
        }

        [RelayCommand]
        private async Task ManualModeTurnOn() => await _configurationService.WriteAsync<bool>(true, ManualModeRequestString);

        [RelayCommand]
        private async Task ManualModeTurnOff() => await _configurationService.WriteAsync<bool>(false, ManualModeRequestString);
    }
}
