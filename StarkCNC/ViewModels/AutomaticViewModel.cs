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
    private string _programName = string.Empty;

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
    private bool _isFullAtomatic;

    [ObservableProperty]
    private double _pipeInstallationDelay;

    [ObservableProperty]
    private bool _hasErrors;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    public AutomaticViewModel(IConfiguration configuration, IManualConfigurationService configurationService, IBendingDataUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _configurationService = configurationService;

        if (unitOfWork is null)
            throw new ArgumentNullException(nameof(unitOfWork));

        ProgramName = unitOfWork.ProgramName;

        int i = 0;
        foreach (var item in unitOfWork.BendingDatas)
        {
            BendingDatas.Add(new BendingDataViewModel(i, item));
            i += 1;
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
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

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ClearActuatorErrors()
    {
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var clearActuatorErrorsRequestString = automaticTagsSection
            .GetSection("ClearActuatorErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        await _configurationService
            .WriteAsync(true, clearActuatorErrorsRequestString)
            .ConfigureAwait(false);

        HasErrors = false;
    }
}
