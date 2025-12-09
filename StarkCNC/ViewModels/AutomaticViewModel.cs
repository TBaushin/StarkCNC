using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.UoW;
using StarkCNC.MachineCommunication.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AutomaticViewModel : ViewModelBase, IDisposable
{
    private IConfiguration _configuration;
    private IManualConfigurationService _configurationService;

    private bool _disposed;

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

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;

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

        StartUpdateTask();
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

    private void StartUpdateTask()
    {
        if (TaskIsRunning())
            return;

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var stopErrorRequestString = automaticTagsSection
            .GetSection("StopErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        _updateTask = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    HasErrors = await _configurationService
                    .ReadAsync<bool>(stopErrorRequestString)
                    .ConfigureAwait(false);
                    await Task.Delay(150).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
                // Нормально: задача отменена
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateTask error: {ex}");
            }
        }, token);
    }

    private bool TaskIsRunning() =>
        _updateTask is not null && !_updateTask.IsCompleted && !_updateTask.IsCanceled && !_updateTask.IsFaulted;

    private void StopUpdateTask()
    {
        if (_updateTask is null)
            return;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            StopUpdateTask();

        _disposed = true;
    }
}
