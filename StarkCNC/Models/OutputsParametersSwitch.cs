using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Models
{
    public partial class OutputsParametersSwitch
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        public string RequestString { get; private set; } = string.Empty;

        public OutputsParametersSwitch(IManualConfigurationService manualConfigurationService)
        {
            _manualConfigurationService = manualConfigurationService;
        }

        [RelayCommand]
        private async Task Run() => await _manualConfigurationService.WriteAsync<bool>(true, RequestString);

        [RelayCommand]
        private async Task Cancel() => await _manualConfigurationService.WriteAsync<bool>(false, RequestString);

        public static OutputsParametersSwitch InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new OutputsParametersSwitch(manualConfigurationService)
            {
                RequestString = section.GetValue<string>(nameof(RequestString)) ?? string.Empty
            };
        }
    }
}
