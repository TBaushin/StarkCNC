using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Models
{
    public partial class SqueezeParameters : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private bool _rearPosition = false;

        [ObservableProperty]
        private bool _rearSecondPosition = false;
        
        [ObservableProperty]
        private bool _frontPosition = false;

        [ObservableProperty]
        private bool _frontSecondPosition = false;

        public string ForwardRequestString { get; private set; } = string.Empty;

        public string BackwardRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string RearSecondPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        public string FrontSecondPositionRequestString { get; private set; } = string.Empty;

        public SqueezeParameters(IManualConfigurationService manualConfigurationService)
        {
            _manualConfigurationService = manualConfigurationService;

            Task.Run(async () =>
            {
                while (true)
                {
                    await GetRearPosition();
                    await GetRearSecondPosition();
                    await GetFrontPosition();
                    await GetFrontSecondPosition();

                    await Task.Delay(150);
                }
            });
        }

        [RelayCommand]
        private async Task ForwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, ForwardRequestString);

        [RelayCommand]
        private async Task ForwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, ForwardRequestString);

        [RelayCommand]
        private async Task BackwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, BackwardRequestString);

        [RelayCommand]
        private async Task BackwardCancel() => await _manualConfigurationService.WriteAsync(false, BackwardRequestString);

        [RelayCommand]
        private async Task GetRearPosition() 
        { 
            var value = await _manualConfigurationService.ReadAsync<bool>(RearPositionRequestString);
            if (RearPosition != value)
                RearPosition = value;
        }

        [RelayCommand]
        private async Task GetRearSecondPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(RearSecondPositionRequestString);
            if (RearSecondPosition != value)
                RearSecondPosition = value;
        }

        [RelayCommand]
        private async Task GetFrontPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(FrontPositionRequestString);
            if (FrontPosition != value)
                FrontPosition = value;
        }

        [RelayCommand]
        private async Task GetFrontSecondPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(FrontSecondPositionRequestString);
            if (FrontSecondPosition != value)
                FrontSecondPosition = value;
        }

        public static SqueezeParameters InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new SqueezeParameters(manualConfigurationService)
            {
                ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty,
                BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty,
                RearPositionRequestString = section.GetValue<string>(nameof(RearPositionRequestString)) ?? string.Empty,
                RearSecondPositionRequestString = section.GetValue<string>(nameof(RearSecondPositionRequestString)) ?? string.Empty,
                FrontPositionRequestString = section.GetValue<string>(nameof(FrontPositionRequestString)) ?? string.Empty,
                FrontSecondPositionRequestString = section.GetValue<string>(nameof(FrontSecondPositionRequestString)) ?? string.Empty
            };
        }
    }
}
