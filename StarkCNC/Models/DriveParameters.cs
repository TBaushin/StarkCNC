using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Models
{
    public partial class DriveParameters : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private double _speed = 0;

        [ObservableProperty]
        private double _coordinate = 0;

        [ObservableProperty]
        private double _relativeDisplacement = 0;

        [ObservableProperty]
        private double _torque = 0;

        [ObservableProperty]
        private bool _rearPosition = false;

        [ObservableProperty]
        private bool _frontPosition = false;

        public string ForwardRequestString { get; private set; } = string.Empty;
        
        public string BackwardRequestString { get; private set; } = string.Empty;
        
        public string ActualCoordinateRequestString { get; private set; } = string.Empty;

        public string ActualRelativeDisplacementRequestString { get; private set; } = string.Empty;

        public string ResetRequestString { get; private set; } = string.Empty;

        public string SpeedRequestString { get; private set; } = string.Empty;

        public string TorqueRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        public DriveParameters(IManualConfigurationService manualConfigurationService)
        {
            _manualConfigurationService = manualConfigurationService;

            Task.Run(async () =>
            {
                while (true)
                {
                    await GetSpeed();
                    await Task.Delay(150);

                    await GetCoordinate();
                    await Task.Delay(150);

                    await GetRelativeDisplacement();
                    await Task.Delay(150);

                    await GetTorque();
                    await Task.Delay(150);

                    await GetRearPosition();
                    await Task.Delay(150);

                    await GetFrontPosition();
                    await Task.Delay(150);
                }
            });
        }

        [RelayCommand]
        private async Task ForwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, ForwardRequestString);

        [RelayCommand]
        private async Task ForwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, ForwardRequestString);

        [RelayCommand]
        private async Task BackwardStart() => await _manualConfigurationService.WriteAsync<bool>(false, BackwardRequestString);

        [RelayCommand]
        private async Task BackwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, BackwardRequestString);

        [RelayCommand]
        private async Task Reset()
        {
            // await _manualConfigurationService.WriteAsync<bool>(true, ResetRequestString);
            Speed = 0;
            Coordinate = 0;
            RelativeDisplacement = 0;
        }

        [RelayCommand]
        private async Task GetSpeed() => Speed = await _manualConfigurationService.ReadAsync<double>(SpeedRequestString);

        [RelayCommand]
        private async Task GetCoordinate() => Coordinate = await _manualConfigurationService.ReadAsync<double>(ActualCoordinateRequestString);

        [RelayCommand]
        private async Task GetRelativeDisplacement() => RelativeDisplacement = await _manualConfigurationService.ReadAsync<double>(ActualRelativeDisplacementRequestString);

        [RelayCommand]
        private async Task GetTorque() => Torque = await _manualConfigurationService.ReadAsync<double>(TorqueRequestString);

        [RelayCommand]
        private async Task GetRearPosition() => RearPosition = await _manualConfigurationService.ReadAsync<bool>(RearPositionRequestString);

        [RelayCommand]
        private async Task GetFrontPosition() => FrontPosition = await _manualConfigurationService.ReadAsync<bool>(FrontPositionRequestString);

        public static DriveParameters InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new DriveParameters(manualConfigurationService)
            {
                ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty,
                BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty,
                ActualCoordinateRequestString = section.GetValue<string>(nameof(ActualCoordinateRequestString)) ?? string.Empty,
                ActualRelativeDisplacementRequestString = section.GetValue<string>(nameof(ActualRelativeDisplacementRequestString)) ?? string.Empty,
                ResetRequestString = section.GetValue<string>(nameof(ResetRequestString)) ?? string.Empty,
                SpeedRequestString = section.GetValue<string>(nameof(SpeedRequestString)) ?? string.Empty,
                TorqueRequestString = section.GetValue<string>(nameof(TorqueRequestString)) ?? string.Empty,
                RearPositionRequestString = section.GetValue<string>(nameof(RearPositionRequestString)) ?? string.Empty,
                FrontPositionRequestString = section.GetValue<string>(nameof(FrontPositionRequestString)) ?? string.Empty
            };
        }
    }
}
