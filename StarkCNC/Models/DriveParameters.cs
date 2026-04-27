using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Globalization;
using System.Windows.Media;

namespace StarkCNC.Models;

public partial class DriveParameters : ObservableObject, IDisposable
{
    private readonly IManualConfigurationService _manualConfigurationService;
    private bool _disposed;

    [ObservableProperty]
    private double? _speed = 0;

    [ObservableProperty]
    private double? _coordinate = 0;

    [ObservableProperty]
    private double? _relativeDisplacement = 0;

    [ObservableProperty]
    private double? _torque = 0;

    [ObservableProperty]
    private Color _rearPosition = Colors.DarkRed;

    [ObservableProperty]
    private Color _frontPosition = Colors.DarkRed;

    public string ForwardRequestString { get; private set; } = string.Empty;
    
    public string BackwardRequestString { get; private set; } = string.Empty;
    
    public string ActualCoordinateRequestString { get; private set; } = string.Empty;

    public string ActualRelativeDisplacementRequestString { get; private set; } = string.Empty;

    public string ResetRequestString { get; private set; } = string.Empty;

    public string SpeedRequestString { get; private set; } = string.Empty;

    public string TorqueRequestString { get; private set; } = string.Empty;

    public string RearPositionRequestString { get; private set; } = string.Empty;

    public string FrontPositionRequestString { get; private set; } = string.Empty;

    public string RelativeDispositionRequestString { get; private set; } = string.Empty;

    public DriveParameters(IManualConfigurationService manualConfigurationService, bool autoRunUpdate)
    {
        _manualConfigurationService = manualConfigurationService;

        PropertyChanged += DriveParameters_PropertyChanged;
    }

    public void Subscribe()
    {
        _manualConfigurationService.Subscribe<float>(SpeedRequestString, value => Speed = value);
        _manualConfigurationService.Subscribe<float>(ActualCoordinateRequestString, value => Coordinate = value);
        _manualConfigurationService.Subscribe<float>(ActualRelativeDisplacementRequestString, value => RelativeDisplacement = value);
        _manualConfigurationService.Subscribe<float>(TorqueRequestString, value => Torque = value);
        _manualConfigurationService.Subscribe<bool>(RearPositionRequestString, value => RearPosition = value ? Colors.Green : Colors.DarkRed);
        _manualConfigurationService.Subscribe<bool>(FrontPositionRequestString, value => FrontPosition = value ? Colors.Green : Colors.DarkRed);
    }

    public void Unsubscribe()
    {
        _manualConfigurationService.Unsubscribe(SpeedRequestString);
        _manualConfigurationService.Unsubscribe(ActualCoordinateRequestString);
        _manualConfigurationService.Unsubscribe(ActualRelativeDisplacementRequestString);
        _manualConfigurationService.Unsubscribe(TorqueRequestString);
        _manualConfigurationService.Unsubscribe(RearPositionRequestString);
        _manualConfigurationService.Unsubscribe(FrontPositionRequestString);
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
            .WriteAsync<bool>(false, BackwardRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task RelativeDisposition() =>
        await _manualConfigurationService
            .WriteAsync<bool>(true, RelativeDispositionRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task RelativeDispositionCancel() =>
        await _manualConfigurationService
            .WriteAsync<bool>(false, RelativeDispositionRequestString)
            .ConfigureAwait(false);

    [RelayCommand]
    private async Task Reset()
    {
        // await _manualConfigurationService.WriteAsync<bool>(true, ResetRequestString);
        Speed = 0;
        Coordinate = 0;
        RelativeDisplacement = 0;
    }

    [RelayCommand]
    private async Task GetSpeed()
    {
        try
        {
            var value = await _manualConfigurationService
                .ReadAsync<float>(SpeedRequestString)
                .ConfigureAwait(false);

            var culture = CultureInfo.CurrentCulture;
            double.TryParse(value.ToString(culture), culture, out var result);
            Speed = result;
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    [RelayCommand]
    private async Task GetCoordinate()
    {
        try
        {
            Coordinate = await _manualConfigurationService
                .ReadAsync<float>(ActualCoordinateRequestString)
                .ConfigureAwait(false);
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    [RelayCommand]
    private async Task GetRelativeDisplacement()
    {
        try
        {
            var value = await _manualConfigurationService
                .ReadAsync<float>(ActualRelativeDisplacementRequestString)
                .ConfigureAwait(false);

            var culture = CultureInfo.CurrentCulture;
            double.TryParse(value.ToString(culture), culture, out var result);
            RelativeDisplacement = result;
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    [RelayCommand]
    private async Task GetTorque()
    {
        try
        {
            var value = await _manualConfigurationService
                .ReadAsync<float>(TorqueRequestString)
                .ConfigureAwait(false);

            var culture = CultureInfo.CurrentCulture;
            double.TryParse(value.ToString(culture), culture, out var result);
            Torque = result;
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    [RelayCommand]
    private async Task GetRearPosition()
    {
        try
        {
            var result = await _manualConfigurationService
                .ReadAsync<bool>(RearPositionRequestString)
                .ConfigureAwait(false);

            if (result)
                RearPosition = Colors.Green;
            else
                RearPosition = Colors.DarkRed;
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    [RelayCommand]
    private async Task GetFrontPosition()
    {
        try
        {
            var result = await _manualConfigurationService
                .ReadAsync<bool>(FrontPositionRequestString)
                .ConfigureAwait(false);

            if (result)
                FrontPosition = Colors.Green;
            else
                FrontPosition = Colors.DarkRed;
        }
        catch (Opc.Ua.ServiceResultException)
        {
            return;
        }
    }

    private async void DriveParameters_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Speed))
        {
            double value = 0;
            if (Speed is not null)
                value = (double)Speed;

            await _manualConfigurationService
                .WriteAsync<float>(Convert.ToSingle(value), SpeedRequestString)
                .ConfigureAwait(false);
        }

        if (e.PropertyName == nameof(RelativeDisplacement))
        {
            double value = 0;
            if (RelativeDisplacement is not null)
                value = (double)RelativeDisplacement;

            await _manualConfigurationService
                .WriteAsync<float>(Convert.ToSingle(value), ActualRelativeDisplacementRequestString)
                .ConfigureAwait(false);
        }
    }

    public static DriveParameters InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName, bool autoRunUpdate)
    {
        IConfigurationSection? section = null;

        if (configurationSection is not null)
            section = configurationSection.GetSection(sectionName);

        return new DriveParameters(manualConfigurationService, autoRunUpdate)
        {
            ForwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ForwardRequestString)),
            BackwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(BackwardRequestString)),
            ActualCoordinateRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ActualCoordinateRequestString)),
            ActualRelativeDisplacementRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ActualRelativeDisplacementRequestString)),
            ResetRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ResetRequestString)),
            SpeedRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(SpeedRequestString)),
            TorqueRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(TorqueRequestString)),
            RearPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RearPositionRequestString)),
            FrontPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(FrontPositionRequestString)),
            RelativeDispositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RelativeDispositionRequestString))
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
            PropertyChanged -= DriveParameters_PropertyChanged;
        }

        _disposed = true;
    }
}