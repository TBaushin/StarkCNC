using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using System.Globalization;
using System.Windows.Media;

namespace StarkCNC.Models
{
    public partial class DriveParameters : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private double? _speed = 0;

        [ObservableProperty]
        private double? _coordinate = 0;

        [ObservableProperty]
        private double? _relativeDisplacement = 0;

        [ObservableProperty]
        private double? _torque = 0;

        [ObservableProperty]
        private Color _rearPosition = Colors.DarkRed;

        [ObservableProperty]
        private Color _frontPosition = Colors.DarkRed;

        public string ForwardRequestString { get; private set; } = string.Empty;
        
        public string BackwardRequestString { get; private set; } = string.Empty;
        
        public string ActualCoordinateRequestString { get; private set; } = string.Empty;

        public string ActualRelativeDisplacementRequestString { get; private set; } = string.Empty;

        public string ResetRequestString { get; private set; } = string.Empty;

        public string SpeedRequestString { get; private set; } = string.Empty;

        public string TorqueRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        public string RelativeDispositionRequestString { get; private set; } = string.Empty;

        public DriveParameters(IManualConfigurationService manualConfigurationService)
        {
            _manualConfigurationService = manualConfigurationService;

            Task.Run(async () =>
            {
                while (true)
                {
                    await GetSpeed();
                    await GetCoordinate();
                    await GetRelativeDisplacement();
                    await GetTorque();
                    await GetRearPosition();
                    await GetFrontPosition();
                    await Task.Delay(150);
                }
            });

            PropertyChanged += DriveParameters_PropertyChanged;
        }

        [RelayCommand]
        private async Task ForwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, ForwardRequestString);

        [RelayCommand]
        private async Task ForwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, ForwardRequestString);

        [RelayCommand]
        private async Task BackwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, BackwardRequestString);

        [RelayCommand]
        private async Task BackwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, BackwardRequestString);

        [RelayCommand]
        private async Task RelativeDisposition() => await _manualConfigurationService.WriteAsync<bool>(true, RelativeDispositionRequestString);

        [RelayCommand]
        private async Task RelativeDispositionCancel() => await _manualConfigurationService.WriteAsync<bool>(false, RelativeDispositionRequestString);

        [RelayCommand]
        private async Task Reset()
        {
            // await _manualConfigurationService.WriteAsync<bool>(true, ResetRequestString);
            Speed = 0;
            Coordinate = 0;
            RelativeDisplacement = 0;
        }

        [RelayCommand]
        private async Task GetSpeed()
        {
            try
            {
                var value = await _manualConfigurationService.ReadAsync<float>(SpeedRequestString);
                double.TryParse(value.ToString(), CultureInfo.CurrentCulture, out var result);
                Speed = result;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetCoordinate()
        {
            try
            {
                Coordinate = await _manualConfigurationService.ReadAsync<double>(ActualCoordinateRequestString);
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetRelativeDisplacement()
        {
            try
            {
                var value = await _manualConfigurationService.ReadAsync<float>(ActualRelativeDisplacementRequestString);
                double.TryParse(value.ToString(), CultureInfo.CurrentCulture, out var result);
                RelativeDisplacement = result;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetTorque()
        {
            try
            {
                var value = await _manualConfigurationService.ReadAsync<float>(TorqueRequestString);
                double.TryParse(value.ToString(), CultureInfo.CurrentCulture, out var result);
                Torque = result;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetRearPosition()
        {
            try
            {
                var result = await _manualConfigurationService.ReadAsync<bool>(RearPositionRequestString);
                if (result)
                    RearPosition = Colors.Green;
                else
                    RearPosition = Colors.DarkRed;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetFrontPosition()
        {
            try
            {
                var result = await _manualConfigurationService.ReadAsync<bool>(FrontPositionRequestString);
                if (result)
                    FrontPosition = Colors.Green;
                else
                    FrontPosition = Colors.DarkRed;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        private async void DriveParameters_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Speed))
            {
                double value = 0;
                if (Speed is not null)
                    value = (double)Speed;

                await _manualConfigurationService.WriteAsync<float>(Convert.ToSingle(value), SpeedRequestString);
            }
        }

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
                FrontPositionRequestString = section.GetValue<string>(nameof(FrontPositionRequestString)) ?? string.Empty,
                RelativeDispositionRequestString = section.GetValue<string>(nameof(RelativeDispositionRequestString)) ?? string.Empty
            };
        }
    }
}
