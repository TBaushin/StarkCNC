using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Windows.Media;

namespace StarkCNC.Models;

public partial class SqueezeParameters : ObservableObject
{
    private readonly IManualConfigurationService _manualConfigurationService;

    [ObservableProperty]
    private Color _rearPosition = Colors.DarkRed;

    [ObservableProperty]
    private Color _rearSecondPosition = Colors.DarkRed;
    
    [ObservableProperty]
    private Color _frontPosition = Colors.DarkRed;

    [ObservableProperty]
    private Color _frontSecondPosition = Colors.DarkRed;

    public string ForwardRequestString { get; private set; } = string.Empty;

    public string BackwardRequestString { get; private set; } = string.Empty;

    public string RearPositionRequestString { get; private set; } = string.Empty;

    public string RearSecondPositionRequestString { get; private set; } = string.Empty;

    public string FrontPositionRequestString { get; private set; } = string.Empty;

    public string FrontSecondPositionRequestString { get; private set; } = string.Empty;

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public SqueezeParameters(IManualConfigurationService manualConfigurationService, bool autoRunUpdate)
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
                    await GetRearPosition().ConfigureAwait(false);
                    await GetRearSecondPosition().ConfigureAwait(false);
                    await GetFrontPosition().ConfigureAwait(false);
                    await GetFrontSecondPosition().ConfigureAwait(false);
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
    private async Task ForwardStart() =>
        await _manualConfigurationService
            .WriteAsync<bool>(true, ForwardRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task ForwardCancel() =>
        await _manualConfigurationService
            .WriteAsync<bool>(false, ForwardRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task BackwardStart() =>
        await _manualConfigurationService
            .WriteAsync<bool>(true, BackwardRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task BackwardCancel() =>
        await _manualConfigurationService
            .WriteAsync(false, BackwardRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task GetRearPosition() 
    { 
        var value = await _manualConfigurationService
            .ReadAsync<bool>(RearPositionRequestString)
            .ConfigureAwait(false);

        if (value)
            RearPosition = Colors.Green;
        else
            RearPosition = Colors.DarkRed;
    }

    [RelayCommand]
    private async Task GetRearSecondPosition()
    {
        var value = await _manualConfigurationService
            .ReadAsync<bool>(RearSecondPositionRequestString)
            .ConfigureAwait(false);

        if (value)
            RearSecondPosition = Colors.Green;
        else
            RearSecondPosition = Colors.DarkRed;
    }

    [RelayCommand]
    private async Task GetFrontPosition()
    {
        var value = await _manualConfigurationService
            .ReadAsync<bool>(FrontPositionRequestString)
            .ConfigureAwait(false);

        if (value)
            FrontPosition = Colors.Green;
        else
            FrontPosition = Colors.DarkRed;
    }

    [RelayCommand]
    private async Task GetFrontSecondPosition()
    {
        var value = await _manualConfigurationService
            .ReadAsync<bool>(FrontSecondPositionRequestString)
            .ConfigureAwait(false);

        if (value)
            FrontSecondPosition = Colors.Green;
        else
            FrontSecondPosition = Colors.DarkRed;
    }

    public static SqueezeParameters InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName, bool autoRunUpdate)
    {
        IConfigurationSection? section = null;

        if (configurationSection is not null)
            configurationSection.GetSection(sectionName);

        return new SqueezeParameters(manualConfigurationService, autoRunUpdate)
        {
            ForwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ForwardRequestString)),
            BackwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(BackwardRequestString)),
            RearPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RearPositionRequestString)),
            RearSecondPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RearSecondPositionRequestString)),
            FrontPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(FrontPositionRequestString)),
            FrontSecondPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(FrontSecondPositionRequestString))
        };
    }
}
