using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.UoW;
using StarkCNC.MachineCommunication.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AutomaticViewModel : ViewModelBase
{
    private IConfiguration _configuration;
    private IManualConfigurationService _configurationService;

    [ObservableProperty]
    private string _programName;

    [ObservableProperty]
    private double _speed;

    [ObservableProperty]
    private double _cycleTime;

    [ObservableProperty]
    private double _pipeLength;

    [ObservableProperty]
    private double _setUpPoint;

    [ObservableProperty]
    private int _countCompletedDetails;

    [ObservableProperty]
    private int _taskDetails;

    [ObservableProperty]
    private double _pipeInstallationDelay;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; set; } = new ObservableCollection<BendingDataViewModel>();

    public AutomaticViewModel(IConfiguration configuration, IManualConfigurationService configurationService, IBendingDataUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _configurationService = configurationService;

        int i = 0;
        foreach (var item in unitOfWork.BendingDatas)
        {
            BendingDatas.Add(new BendingDataViewModel(i, item));
            i += 1;
        }
    }

    [RelayCommand]
    private async Task SetAutomaticMode()
    {
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var turnOnRequestString = automaticTagsSection
            .GetSection("TurnOn")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        await _configurationService
            .WriteAsync(true, turnOnRequestString)
            .ConfigureAwait(false);
    }
}
