using CommunityToolkit.Mvvm.Input;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.ViewModels;

public partial class AutomaticViewModel : ViewModelBase
{
    private IManualConfigurationService _configurationService;

    public AutomaticViewModel(IManualConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [RelayCommand]
    private async Task SetAutomaticMode()
    {
        await _configurationService.WriteAsync(true, "GVL.Auto").ConfigureAwait(false);
    }
}
