using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Windows.Media;

namespace StarkCNC.Models;

public partial class OutputsParametersSwitch : ObservableObject, IDisposable
{
    private readonly IManualConfigurationService _manualConfigurationService;
    private bool _disposed;

    private bool _status;

    public string RequestString { get; private set; } = string.Empty;

    [ObservableProperty]
    private Color _statusColor;

    public OutputsParametersSwitch(IManualConfigurationService manualConfigurationService, bool autoRunUpdate = false)
    {
        _manualConfigurationService = manualConfigurationService;

        Subscribe();
    }

    public void Subscribe()
    {
        _manualConfigurationService.Subscribe<bool>(RequestString, value =>
        {
            _status = value;
            StatusColor = value ? Colors.Green : Colors.DarkRed;
        });
    }

    public void Unsubscribe()
    {
        _manualConfigurationService.Unsubscribe(RequestString);
    }

    [RelayCommand]
    private async Task Run() =>
        await _manualConfigurationService
            .WriteAsync<bool>(!_status, RequestString)
            .ConfigureAwait(false);

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