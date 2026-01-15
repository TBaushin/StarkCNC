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
    private double _currentTaskSupply;

    [ObservableProperty]
    private double _currentTaskRotationAngle;

    [ObservableProperty]
    private double _currentTaskBendingAngle;

    [ObservableProperty]
    private double _facticalSupply;

    [ObservableProperty]
    private double _facticalRotationAngle;

    [ObservableProperty]
    private double _facticalBendingAngle;

    [ObservableProperty]
    private double _factialConsole;

    [ObservableProperty]
    private bool _sendData;

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

        BendingDatas.CollectionChanged += BendingDatas_CollectionChanged;

        foreach (var item in unitOfWork.BendingDatas)
        {
            BendingDatas.Add(new BendingDataViewModel(item));
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
        var factialSection = automaticTagsSection.GetSection("Factial");

        var stopErrorRequestString = automaticTagsSection
            .GetSection("StopErrors")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var cycleTimeRequestString = automaticTagsSection
            .GetSection("CycleTime")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var sendDataRequestString = automaticTagsSection
            .GetSection("SendData")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalSupplyRequestString = factialSection
            .GetSection("Supply")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;
        
        var facticalRotationReuqestString = factialSection
            .GetSection("Rotation")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalBendingRequestString = factialSection
            .GetSection("Bending")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var facticalConsoleRequestString = factialSection
            .GetSection("Console")
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
                    CycleTime = await _configurationService
                        .ReadAsync<double>(cycleTimeRequestString)
                        .ConfigureAwait(false);
                    SetSendData(await _configurationService
                        .ReadAsync<bool>(sendDataRequestString)
                        .ConfigureAwait(false), true);

                    FacticalSupply = await _configurationService
                        .ReadAsync<double>(facticalSupplyRequestString)
                        .ConfigureAwait(false);

                    FacticalRotationAngle = await _configurationService
                        .ReadAsync<double>(facticalRotationReuqestString)
                        .ConfigureAwait(false);

                    FacticalBendingAngle = await _configurationService
                        .ReadAsync<double>(facticalBendingRequestString)
                        .ConfigureAwait(false);

                    FactialConsole = await _configurationService
                        .ReadAsync<double>(facticalConsoleRequestString)
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

    private void SetSendData(bool value, bool isUpdateTask = false)
    {
        SendData = value;
        if (!isUpdateTask)
            return;

        if (SendData == true)
            RunProgram();
    }

    private async void RunProgram()
    {
        // Prepare
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var currentTaskSection = automaticTagsSection.GetSection("CurrentTask");
        var allBendRequestString = automaticTagsSection
            .GetSection("AllGib")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var sendDataRequestString = automaticTagsSection
            .GetSection("SendData")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var stepNumberRequestString = automaticTagsSection
            .GetSection("StepNumber")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var supplyRequestString = currentTaskSection
            .GetSection("Supply")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var rotationAngleRequestString = currentTaskSection
            .GetSection("Rotation")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        var bendingAngleRequestString = currentTaskSection
            .GetSection("Bending")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        // Send data
        await _configurationService
            .WriteAsync<int>(BendingDatas.Count, allBendRequestString)
            .ConfigureAwait(false);

        foreach (var data in BendingDatas)
        {
            CurrentTaskSupply = data.Supply;
            CurrentTaskRotationAngle = data.RotationAngle;
            CurrentTaskBendingAngle = data.BendingAngle;

            await _configurationService
                .WriteAsync<int>(data.Id, stepNumberRequestString)
                .ConfigureAwait(false);

             await _configurationService
                .WriteAsync<double>(data.Supply, supplyRequestString)
                .ConfigureAwait(false);
            await _configurationService
                .WriteAsync<double>(data.RotationAngle, rotationAngleRequestString)
                .ConfigureAwait(false);
            await _configurationService
                .WriteAsync<double>(data.BendingAngle, bendingAngleRequestString)
                .ConfigureAwait(false);
        }

        // Finish
        await _configurationService
            .WriteAsync<bool>(false, sendDataRequestString)
            .ConfigureAwait(false);
        SetSendData(false);
    }

    private void BendingDatas_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        int i = 1;
        foreach (var item in BendingDatas)
        {
            item.Id = i;
            i++;
        }
    }

    async partial void OnIsFullAtomaticChanged(bool oldValue, bool newValue)
    {
        var automaticTagsSection = _configuration.GetSection("AutomaticTags");
        var fullAutomaticRequestString = automaticTagsSection
            .GetSection("FullAutomatic")
            .GetSection("RequestString")
            .Get<string>() ?? string.Empty;

        await _configurationService.WriteAsync<bool>(newValue, fullAutomaticRequestString)
            .ConfigureAwait(false);
    }

    async partial void OnPipeInstallationDelayChanged(double oldValue, double newValue)
    {
        if (IsFullAtomatic)
        {
            var automaticTagsSection = _configuration.GetSection("AutomaticTags");
            var delayRequestString = automaticTagsSection
                .GetSection("Delay")
                .GetSection("RequestString")
                .Get<string>() ?? string.Empty;

            await _configurationService.WriteAsync<double>(newValue, delayRequestString)
                .ConfigureAwait(false);
        }
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
