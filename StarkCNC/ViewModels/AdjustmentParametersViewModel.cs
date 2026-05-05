using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;

namespace StarkCNC.ViewModels;

public partial class AdjustmentParametersViewModel : ViewModelBase
{
    private readonly IRouter _router;
    private IAdjustmentRepository _repository;
    private IManualConfigurationService _manualConfigurationService;
    private AdjustmentParametersConstructor _constructor;
    private Guid? _id;

    private AdjustmentParameters? _adjustment;

    private string _typeRequestString = string.Empty;

    private string _pipeDiameterRequestString = string.Empty;

    private string _radiusRequestString = string.Empty;

    private string _distanceFromCenterRequestString = string.Empty;

    private string _clampLengthRequestString = string.Empty;

    private string _pressLengthRequestString = string.Empty;

    private string _squeezeTurnOnRequestString = string.Empty;

    private string _clampRollerOuterRadiusRequestString = string.Empty;

    private string _clampRollerInnerRadiusRequestString = string.Empty;

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
        ReadRequestsFromConfiguration();
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

    private void ReadRequestsFromConfiguration()
    {
        var adjustmentSection = App.Configuration.GetSection("Adjustment");

        var adjustmentTypeSection = adjustmentSection.GetSection("AdjustmentType");
        _typeRequestString = adjustmentTypeSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeDiameterSection = adjustmentSection.GetSection("PipeDiameter");
        _pipeDiameterRequestString = pipeDiameterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        //TODO: var radius 

        var distanceFromCenterSection = adjustmentSection.GetSection("DistanceFromCenter");
        _distanceFromCenterRequestString = distanceFromCenterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var clampSection = adjustmentSection.GetSection("Clamp");
        var clampLengthSection = clampSection.GetSection("Length");
        _clampLengthRequestString = clampLengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pressSection = adjustmentSection.GetSection("Press");
        var pressLengthSection = pressSection.GetSection("Length");
        _pressLengthRequestString = pressLengthSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var squeezeSection = adjustmentSection.GetSection("Squeeze");
        var squeezeTurnOnSection = squeezeSection.GetSection("TurnOn");
        _squeezeTurnOnRequestString = squeezeTurnOnSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var clampRollerSection = adjustmentSection.GetSection("ClampRoller");
        var clampRollerOuterRadiusSection = clampRollerSection.GetSection("OuterRadius");
        _clampRollerOuterRadiusRequestString = clampRollerOuterRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var clampRollerInnerRadiusSection = clampRollerSection.GetSection("InnerRadius");
        _clampRollerInnerRadiusRequestString = clampRollerInnerRadiusSection.GetSection("RequestString").Get<string>() ?? string.Empty;
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
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _typeRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnAdjustmentTypeChanged(AdjustmentType oldValue, AdjustmentType newValue)
    {
        if (_adjustment?.Type != newValue)
        {
            _adjustment?.Type = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _typeRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnPipeDiameterChanged(float oldValue, float newValue)
    {
        if (_adjustment?.PipeDiameter != newValue)
        {
            _adjustment?.PipeDiameter = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _pipeDiameterRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Radius != newValue)
        {
            _adjustment?.Radius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _radiusRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnDistanceFromCenterChanged(float oldValue, float newValue)
    {
        if (_adjustment?.DistanceFromCenter != newValue)
        {
            _adjustment?.DistanceFromCenter = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _distanceFromCenterRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnClampLengthChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Clamp.Length != newValue)
        {
            _adjustment?.Clamp.Length = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _clampLengthRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnPressLengthChanged(float oldValue, float newValue)
    {
        if (_adjustment?.Press.Length != newValue)
        {
            _adjustment?.Press.Length = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _pressLengthRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnSqueezeTurnOnChanged(bool oldValue, bool newValue)
    {
        if (_adjustment?.Squeeze.TurnOn != newValue)
        {
            _adjustment?.Squeeze.TurnOn = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _squeezeTurnOnRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnClampRollerOuterRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.ClampRoller.OuterRadius != newValue)
        {
            _adjustment?.ClampRoller.OuterRadius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _clampRollerOuterRadiusRequestString).ConfigureAwait(true);
        }
    }

    async partial void OnClampRollerInnerRadiusChanged(float oldValue, float newValue)
    {
        if (_adjustment?.ClampRoller.InnerRadius != newValue)
        {
            _adjustment?.ClampRoller.InnerRadius = newValue;
            await UpdateAdjustment().ConfigureAwait(true);
            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(newValue, _clampRollerInnerRadiusRequestString).ConfigureAwait(true);
        }
    }

    private async Task UpdateAdjustment()
    {
        if (_adjustment is null)
            return;

        await _repository.UpdateElementAsync(_adjustment).ConfigureAwait(true);
    }
}
