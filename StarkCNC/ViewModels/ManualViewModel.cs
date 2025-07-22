using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;

namespace StarkCNC.ViewModels
{
    internal partial class ManualViewModel : ObservableObject
    {
        private readonly IConfiguration _configuration;
        private readonly IManualConfigurationService _configurationService;

        public DriveParameters FeedDrive { get; set; }

        public DriveParameters TurnDrive { get; set; }

        public DriveParameters ConsoleDrive { get; set; }

        public OutputsParametersTwoButtons Clamp { get; set; }
        
        public OutputsParametersTwoButtons Press { get; set; }

        public OutputsParametersTwoButtons Squeeze { get; set; }

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

            FeedDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), nameof(FeedDrive));
            TurnDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), nameof(TurnDrive));
            ConsoleDrive = DriveParameters.InitializeParameters(_configuration.GetSection("MachineController").GetSection("Drive"), nameof(ConsoleDrive));

            Clamp = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Clamp));
            Press = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Press));
            Squeeze = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Squeeze));
            Bend = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Bend));
            Collet = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Collet));
            Dorn = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Dorn));
            Adjustment = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Adjustment));
            Punching = OutputsParametersTwoButtons.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsFB"), nameof(Punching));

            FirstHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), nameof(FirstHydraulics));
            SecondHydraulics = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), nameof(SecondHydraulics));
            Support = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), nameof(Support));
            DornLubricant = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), nameof(DornLubricant));
            BendAndSqueeze = OutputsParametersSwitch.InitializeParameters(_configuration.GetSection("MachineController").GetSection("OutputsTF"), nameof(BendAndSqueeze));
        }

        [RelayCommand]
        private async Task FeedForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, FeedDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task FeedForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, FeedDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task FeedBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, FeedDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task FeedBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, FeedDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task TurnForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, TurnDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task TurnForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, TurnDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task TurnBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, TurnDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task TurnBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, TurnDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ConsoleForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ConsoleForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ConsoleBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ConsoleBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ClampForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Clamp.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ClampForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Clamp.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ClampBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Clamp.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ClampBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Clamp.BackwardRequestString);
        }

        [RelayCommand]
        private async Task PressForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Press.ForwardRequestString);
        }

        [RelayCommand]
        private async Task PressForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Press.ForwardRequestString);
        }

        [RelayCommand]
        private async Task PressBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Press.BackwardRequestString);
        }

        [RelayCommand]
        private async Task PressBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Press.BackwardRequestString);
        }

        [RelayCommand]
        private async Task SqueezeForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Squeeze.ForwardRequestString);
        }

        [RelayCommand]
        private async Task SqueezeForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Squeeze.ForwardRequestString);
        }

        [RelayCommand]
        private async Task SqueezeBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Squeeze.BackwardRequestString);
        }

        [RelayCommand]
        private async Task SqueezeBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Squeeze.BackwardRequestString);
        }

        [RelayCommand]
        private async Task BendForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Bend.ForwardRequestString);
        }

        [RelayCommand]
        private async Task BendForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Bend.ForwardRequestString);
        }

        [RelayCommand]
        private async Task BendBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Bend.BackwardRequestString);
        }

        [RelayCommand]
        private async Task BendBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Bend.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ColletForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Collet.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ColletForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Collet.ForwardRequestString);
        }

        [RelayCommand]
        private async Task ColletBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Collet.BackwardRequestString);
        }

        [RelayCommand]
        private async Task ColletBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Collet.BackwardRequestString);
        }

        [RelayCommand]
        private async Task DornForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Dorn.ForwardRequestString);
        }

        [RelayCommand]
        private async Task DornForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Dorn.ForwardRequestString);
        }

        [RelayCommand]
        private async Task DornBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Dorn.BackwardRequestString);
        }

        [RelayCommand]
        private async Task DornBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Dorn.BackwardRequestString);
        }

        [RelayCommand]
        private async Task AdjustmentForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Adjustment.ForwardRequestString);
        }

        [RelayCommand]
        private async Task AdjustmentForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Adjustment.ForwardRequestString);
        }

        [RelayCommand]
        private async Task AdjustmentBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Adjustment.BackwardRequestString);
        }

        [RelayCommand]
        private async Task AdjustmentBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Adjustment.BackwardRequestString);
        }

        [RelayCommand]
        private async Task PunchingForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Punching.ForwardRequestString);
        }

        [RelayCommand]
        private async Task PunchingForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Punching.ForwardRequestString);
        }

        [RelayCommand]
        private async Task PunchingBackwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, Punching.BackwardRequestString);
        }

        [RelayCommand]
        private async Task PunchingBackwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, Punching.BackwardRequestString);
        }

        [RelayCommand]
        private async Task HydraulicsFirstTurnOn()
        {
            await _configurationService.WriteAsync<bool>(true, Punching.ForwardRequestString);
        }

        [RelayCommand]
        private async Task HydraulicsFirstTurnOff()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task HydraulicsSecondTurnOn()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task HydraulicsSecondTurnOff()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task SupportUp()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task SupportUpStop()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.ForwardRequestString);
        }

        [RelayCommand]
        private async Task DornLubricantTurnOn()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task DornLubricantTurnOff()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task BendAndSqueezeForwardStart()
        {
            await _configurationService.WriteAsync<bool>(true, ConsoleDrive.BackwardRequestString);
        }

        [RelayCommand]
        private async Task BendAndSqueezeForwardStop()
        {
            await _configurationService.WriteAsync<bool>(false, ConsoleDrive.BackwardRequestString);
        }

        private async void Connect()
        {
            if (!_configurationService.Connected)
                await _configurationService.ConnectAsync();
        }
    }
}
