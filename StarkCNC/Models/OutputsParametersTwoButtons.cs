using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Models
{
    public partial class OutputsParametersTwoButtons : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private bool _rearPosition = false;

        [ObservableProperty]
        private bool _frontPosition = false;

        public string ForwardRequestString { get; private set; } = string.Empty;

        public string BackwardRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        public OutputsParametersTwoButtons(IManualConfigurationService manualConfigurationService) 
        {
            _manualConfigurationService = manualConfigurationService;

            Task.Run(async () =>
            {
                while (true)
                {
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
        private async Task BackwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, BackwardRequestString);

        [RelayCommand]
        private async Task BackwardCancel() => await _manualConfigurationService.WriteAsync(false, BackwardRequestString);

        [RelayCommand]
        private async Task GetRearPosition()
        {
            try
            {
                var a = await _manualConfigurationService.ReadAsync<bool>(RearPositionRequestString);
                RearPosition = a;
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
                FrontPosition = await _manualConfigurationService.ReadAsync<bool>(FrontPositionRequestString);
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        public static OutputsParametersTwoButtons InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new OutputsParametersTwoButtons(manualConfigurationService)
            {
                ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty,
                BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty,
                RearPositionRequestString = section.GetValue<string>(nameof(RearPositionRequestString)) ?? string.Empty,
                FrontPositionRequestString = section.GetValue<string>(nameof(FrontPositionRequestString)) ?? string.Empty
            };
        }
    }
}
