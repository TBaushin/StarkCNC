using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Windows.Media;

namespace StarkCNC.Models;

public partial class SqueezeParameters : ObservableObject, IDisposable
{
    private bool _disposed;

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

    public SqueezeParameters(IManualConfigurationService manualConfigurationService, bool autoRunUpdate)
    {
        _manualConfigurationService = manualConfigurationService;

        Subscribe();
    }

    private void Subscribe()
    {
        _manualConfigurationService.Subscribe<bool>(RearPositionRequestString, value => RearPosition = value ? Colors.Green : Colors.DarkRed);
        _manualConfigurationService.Subscribe<bool>(RearSecondPositionRequestString, value => RearSecondPosition = value ? Colors.Green : Colors.DarkRed);
        _manualConfigurationService.Subscribe<bool>(FrontPositionRequestString, value => FrontPosition = value ? Colors.Green : Colors.DarkRed);
        _manualConfigurationService.Subscribe<bool>(FrontSecondPositionRequestString, value => FrontSecondPosition = value ? Colors.Green : Colors.DarkRed);
    }

    private void Unsubscribe()
    {
        _manualConfigurationService.Unsubscribe(RearPositionRequestString);
        _manualConfigurationService.Unsubscribe(RearSecondPositionRequestString);
        _manualConfigurationService.Unsubscribe(FrontPositionRequestString);
        _manualConfigurationService.Unsubscribe(FrontSecondPositionRequestString);
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
            section = configurationSection.GetSection(sectionName);

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
        {
            Unsubscribe();
        }

        _disposed = true;
    }
}