using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using StarkCNC.Utilities;

namespace StarkCNC.ViewModels;

public partial class AdjustmentParametersViewModel : ViewModelBase
{
    private readonly IRouter _router;
    private IAdjustmentRepository _repository;
    private IManualConfigurationService _manualConfigurationService;
    private AdjustmentParametersConstructor _constructor;
    private Guid? _id;

    private AdjustmentParameters? _adjustment;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private AdjustmentType _adjustmentType = AdjustmentType.Winding;

    [ObservableProperty]
    private float _pipeDiameter;

    [ObservableProperty]
    private float _radius;

    [ObservableProperty]
    private float _distanceFromCenter;

    [ObservableProperty]
    private float _clampLength;

    [ObservableProperty]
    private float _pressLength;

    [ObservableProperty]
    private bool _squeezeTurnOn;

    [ObservableProperty]
    private float _clampRollerOuterRadius;

    [ObservableProperty]
    private float _clampRollerInnerRadius;

    public AdjustmentParametersViewModel(
        IRouter router,
        IAdjustmentRepository repository,
        IManualConfigurationService manualConfigurationService,
        AdjustmentParametersConstructor constructor,
        Guid? id)
    {
        _router = router;
        _repository = repository;
        _manualConfigurationService = manualConfigurationService;
        _constructor = constructor;
        _id = id;
    }

    public async Task InitializeAsync()
    {
        await SetSelectedAdjustment(_id).ConfigureAwait(true);
    }

    private async Task SetSelectedAdjustment(Guid? id)
    {
        if (id is not Guid guid)
            throw new ArgumentNullException(nameof(id));

        var adjustment = await _repository.FindByIdAsync(guid).ConfigureAwait(true);
        if (adjustment is not null)
        {
            _adjustment = adjustment;
            Name = _adjustment.Name;
            AdjustmentType = _adjustment.Type;
            PipeDiameter = _adjustment.PipeDiameter;
            Radius = _adjustment.Radius;
            DistanceFromCenter = _adjustment.DistanceFromCenter;
            ClampLength = _adjustment.Clamp.Length;
            PressLength = _adjustment.Press.Length;
            SqueezeTurnOn = _adjustment.Squeeze.TurnOn;
            ClampRollerOuterRadius = _adjustment.ClampRoller.OuterRadius;
            ClampRollerInnerRadius = _adjustment.ClampRoller.InnerRadius;
            return;
        }
        
        throw new InvalidOperationException("Не удалось найти оснастку");
    }

    [RelayCommand]
    private void GoToSettingCoordinates()
    {
        _router.Navigate("/adjustment/list/edit/coordinates", _adjustment?.Id);
    }

    [RelayCommand]
    private async Task EditAdjustment()
    {
        if (_adjustment is null)
            return;

        var adjustment = (AdjustmentParameters)_adjustment.Clone();
        adjustment.Name = Name;
        adjustment.Type = AdjustmentType;
        adjustment.PipeDiameter = PipeDiameter;
        adjustment.Radius = Radius;

        var settingsWindow = new AdjustmentSettingsWindow(
            await _repository.GetAllAsync().ConfigureAwait(true),
            adjustment,
            _constructor,
            "Редактирование оснастки");
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is null)
            return;

        Name = result.Name;
        AdjustmentType = result.Type;
        PipeDiameter = result.PipeDiameter;
        Radius = result.Radius;
    }

    async partial void OnNameChanged(string? oldValue, string newValue)
    {
        if (_adjustment?.Name != newValue)
        {
            _adjustment?.Name = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
        }
    }

    async partial void OnAdjustmentTypeChanged(AdjustmentType oldValue, AdjustmentType newValue)
    {
        if (_adjustment?.Type != newValue)
        {
            _adjustment?.Type = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync((int)newValue, ControllerRequestStrings.GET_ADJUSTMENT_TYPE(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnPipeDiameterChanged(float oldValue, float newValue)
    {
        if (_adjustment?.PipeDiameter != newValue)
        {
            _adjustment?.PipeDiameter = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_ADJUSTMENT_PIPE_DIAMETER(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Radius != newValue)
        {
            _adjustment?.Radius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_ADJUSTMENT_RADIUS(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnDistanceFromCenterChanged(float oldValue, float newValue)
    {
        if (_adjustment?.DistanceFromCenter != newValue)
        {
            _adjustment?.DistanceFromCenter = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_ADJUSTMENT_DISTANCE_FROM_CENTER(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnClampLengthChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Clamp.Length != newValue)
        {
            _adjustment?.Clamp.Length = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_CLAMP_LENGTH(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnPressLengthChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Press.Length != newValue)
        {
            _adjustment?.Press.Length = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_PRESS_LENGTH(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnSqueezeTurnOnChanged(bool oldValue, bool newValue)
    {
        if (_adjustment?.Squeeze.TurnOn != newValue)
        {
            _adjustment?.Squeeze.TurnOn = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_SQUEEZE_TURN_ON(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnClampRollerOuterRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.ClampRoller.OuterRadius != newValue)
        {
            _adjustment?.ClampRoller.OuterRadius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_CLAMP_ROLLER_OUTER_RADIUS(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    async partial void OnClampRollerInnerRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.ClampRoller.InnerRadius != newValue)
        {
            _adjustment?.ClampRoller.InnerRadius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            if (_adjustment is not null && _adjustment.InstalledLevel > 0 && _adjustment.IsEnabled)
                await _manualConfigurationService
                    .WriteAsync(newValue, ControllerRequestStrings.GET_CLAMP_ROLLER_INNTER_RADIUS(_adjustment.InstalledLevel))
                    .ConfigureAwait(true);
        }
    }

    private async Task UpdateAdjustment()
    {
        if (_adjustment is null)
            return;

        await _repository.UpdateElementAsync(_adjustment).ConfigureAwait(true);
    }
}
