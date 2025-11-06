using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Windows.Media;

namespace StarkCNC.Models;

public partial class OutputsParametersSwitch : ObservableObject
{
    private readonly IManualConfigurationService _manualConfigurationService;

    public string RequestString { get; private set; } = string.Empty;

    [ObservableProperty]
    private Color _statusColor;

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public OutputsParametersSwitch(IManualConfigurationService manualConfigurationService, bool autoRunUpdate = false)
    {
        _manualConfigurationService = manualConfigurationService;

        if (autoRunUpdate)
            StartUpdateTask();
    }

    public void StartUpdateTask()
    {
        if (TaskIsRunning())
            return;

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _updateTask = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await GetStatus().ConfigureAwait(false);
                    await Task.Delay(150, token).ConfigureAwait(false);
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

    public void StopUpdateTask()
    {
        if (_updateTask is null)
            return;

        _cancellationTokenSource?.Cancel();
    }

    [RelayCommand]
    private async Task Run() =>
        await _manualConfigurationService
            .WriteAsync<bool>(true, RequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task Cancel() =>
        await _manualConfigurationService
            .WriteAsync<bool>(false, RequestString)
            .ConfigureAwait(false);

    private async Task GetStatus()
    {
        var value = await _manualConfigurationService
            .ReadAsync<bool>(RequestString)
            .ConfigureAwait(false);

        if (value)
            StatusColor = Colors.Green;
        else
            StatusColor = Colors.DarkRed;
    }

    public static OutputsParametersSwitch InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName, bool autoRunUpdate = false)
    {
        IConfigurationSection? section = null;

        if (configurationSection is not null)
            section = configurationSection.GetSection(sectionName);

        return new OutputsParametersSwitch(manualConfigurationService)
        {
            RequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RequestString))
        };
    }
}