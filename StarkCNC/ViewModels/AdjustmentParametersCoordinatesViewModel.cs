using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;

namespace StarkCNC.ViewModels;

public partial class AdjustmentParametersCoordinatesViewModel : ViewModelBase
{
    private readonly IAdjustmentRepository _repository;
    private readonly ISettingsRepository _settingsRepository;

    private AdjustmentParameters _adjustment;

    [ObservableProperty]
    private bool _isElectricMachine;

    [ObservableProperty]
    private AdjustmentParametersBendVisibleDto _bend;

    [ObservableProperty]
    private AdjustmentParametersClampVisibleDto _clamp;

    [ObservableProperty]
    private AdjustmentParametersConsoleVisibleDto _console;

    [ObservableProperty]
    private AdjustmentParametersDornVisibleDto _dorn;

    [ObservableProperty]
    private AdjustmentParametersLiftVisibleDto _lift;

    [ObservableProperty]
    private AdjustmentParametersPressVisibleDto _press;

    [ObservableProperty]
    private AdjustmentParametersRotationVisibleDto _rotation;

    [ObservableProperty]
    private AdjustmentParametersSqueezeVisibleDto _squeeze;

    [ObservableProperty]
    private AdjustmentParametersSupplyVisibleDto _supply;

    public AdjustmentParametersCoordinatesViewModel(
        IAdjustmentRepository repository,
        ISettingsRepository settingsRepository,
        Guid? id)
    {
        _repository = repository;
        _settingsRepository = settingsRepository;

        LoadSettings();
        SetSelectedAdjustment(id);

        if (_adjustment is null)
            throw new InvalidOperationException("Не удалось найти оснастку");

        Bend = new AdjustmentParametersBendVisibleDto()
        {
            ForwardPositionLimitation = _adjustment.Bend.ForwardPositionLimitation,
            SpeedCoefficient = _adjustment.Bend.SpeedCoefficient,
            SlowdownSpeed = _adjustment.Bend.SlowdownSpeed
        };

        Clamp = new AdjustmentParametersClampVisibleDto()
        {
            ForwardPosition = _adjustment.Clamp.ForwardPosition,
            MiddlePosition = _adjustment.Clamp.MiddlePosition,
            BackwardPosition = _adjustment.Clamp.BackwardPosition,
            SpeedCoefficient = _adjustment.Clamp.SpeedCoefficient
        };

        Console = new AdjustmentParametersConsoleVisibleDto()
        {
            BendPosition = _adjustment.Console.BendPosition,
            SecondFloorPosition = _adjustment.Console.SecondFloorPosition,
            ThirdFloorPosition = _adjustment.Console.ThirdFloorPosition,
            PipeRotationDepartureDistance = _adjustment.Console.PipeRotationDepartureDistance,
            SpeedCoefficient = _adjustment.Console.SpeedCoefficient
        };

        Dorn = new AdjustmentParametersDornVisibleDto()
        {
            ForwardPosition = _adjustment.Dorn.ForwardPosition,
            MiddlePosition = _adjustment.Dorn.MiddlePosition,
            BackwardPosition = _adjustment.Dorn.BackwardPosition,
            SpeedCoefficient = _adjustment.Dorn.SpeedCoefficient
        };

        Lift = new AdjustmentParametersLiftVisibleDto()
        {
            UpperPosition = _adjustment.Lift.UpperPosition,
            MiddlePosition = _adjustment.Lift.MiddlePosition,
            LowerPosition = _adjustment.Lift.LowerPosition,
            SpeedCoefficient = _adjustment.Lift.SpeedCoefficient
        };

        Press = new AdjustmentParametersPressVisibleDto()
        {
            ForwardPosition = _adjustment.Press.ForwardPosition,
            MiddlePosition = _adjustment.Press.MiddlePosition,
            BackwardPosition = _adjustment.Press.BackwardPosition,
            SpeedCoefficient = _adjustment.Press.SpeedCoefficient
        };

        Rotation = new AdjustmentParametersRotationVisibleDto()
        {
            OffsetAfterZeroSearch = _adjustment.Rotation.OffsetAfterZeroSearch,
            SpeedCoefficient = _adjustment.Rotation.SpeedCoefficient
        };

        Squeeze = new AdjustmentParametersSqueezeVisibleDto()
        {
            FrontPositionLimitation = _adjustment.Squeeze.FrontPositionLimitation,
            SpeedCoefficient = _adjustment.Squeeze.SpeedCoefficient
        };

        Supply = new AdjustmentParametersSupplyVisibleDto()
        {
            PressZonePosition = _adjustment.Supply.PressZonePosition,
            ForwardDangerZonePosition = _adjustment.Supply.ForwardDangerZonePosition,
            ColletJawsDepth = _adjustment.Supply.ColletJawsDepth,
            SpeedCoefficient = _adjustment.Supply.SpeedCoefficient
        };
    }

    private async void SetSelectedAdjustment(Guid? id)
    {
        if (id is not Guid guid)
            throw new ArgumentNullException(nameof(id));

        var adjustment = await _repository.FindByIdAsync(guid).ConfigureAwait(false);
        if (adjustment is null)
            throw new InvalidOperationException("Не удалось найти оснастку");

        _adjustment = adjustment;
    }

    private async void LoadSettings()
    {
        var settings = await _settingsRepository.GetAsync().ConfigureAwait(false);
        IsElectricMachine = settings?.IsElectricMachine ?? false;
    }

    [RelayCommand]
    private async Task EditParameters(string parameter)
    {
        var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(_adjustment, parameter);
        parametersSettingsWindow.ShowDialog();

        var adjustment = parametersSettingsWindow.Adjustment;
        if (adjustment.Id == _adjustment.Id)
            await _repository.UpdateElementAsync(adjustment).ConfigureAwait(false);
    }
}
