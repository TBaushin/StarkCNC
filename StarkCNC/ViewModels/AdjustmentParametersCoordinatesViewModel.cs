using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models.Adjustment;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;

namespace StarkCNC.ViewModels;

public partial class AdjustmentParametersCoordinatesViewModel : ViewModelBase
{
    private readonly IAdjustmentRepository _repository;
    private readonly ISettingsRepository _settingsRepository;

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
    }

    private async void SetSelectedAdjustment(Guid? id)
    {
        if (id is not Guid guid)
            throw new ArgumentNullException(nameof(id));

        var adjustment = await _repository.FindByIdAsync(guid).ConfigureAwait(false);
        if (adjustment is null)
            throw new InvalidOperationException("Не удалось найти оснастку");

        Bend = new AdjustmentParametersBendVisibleDto()
        {
            ForwardPositionLimitation = adjustment.Bend.ForwardPositionLimitation,
            SpeedCoefficient = adjustment.Bend.SpeedCoefficient,
            SlowdownSpeed = adjustment.Bend.SlowdownSpeed
        };

        Clamp = new AdjustmentParametersClampVisibleDto()
        {
            ForwardPosition = adjustment.Clamp.ForwardPosition,
            MiddlePosition = adjustment.Clamp.MiddlePosition,
            BackwardPosition = adjustment.Clamp.BackwardPosition,
            SpeedCoefficient = adjustment.Clamp.SpeedCoefficient
        };

        Console = new AdjustmentParametersConsoleVisibleDto()
        {
            BendPosition = adjustment.Console.BendPosition,
            SecondFloorPosition = adjustment.Console.SecondFloorPosition,
            ThirdFloorPosition = adjustment.Console.ThirdFloorPosition,
            PipeRotationDepartureDistance = adjustment.Console.PipeRotationDepartureDistance,
            SpeedCoefficient = adjustment.Console.SpeedCoefficient
        };

        Dorn = new AdjustmentParametersDornVisibleDto()
        {
            ForwardPosition = adjustment.Dorn.ForwardPosition,
            MiddlePosition = adjustment.Dorn.MiddlePosition,
            BackwardPosition = adjustment.Dorn.BackwardPosition,
            SpeedCoefficient = adjustment.Dorn.SpeedCoefficient
        };

        Lift = new AdjustmentParametersLiftVisibleDto()
        {
            UpperPosition = adjustment.Lift.UpperPosition,
            MiddlePosition = adjustment.Lift.MiddlePosition,
            LowerPosition = adjustment.Lift.LowerPosition,
            SpeedCoefficient = adjustment.Lift.SpeedCoefficient
        };

        Press = new AdjustmentParametersPressVisibleDto()
        {
            ForwardPosition = adjustment.Press.ForwardPosition,
            MiddlePosition = adjustment.Press.MiddlePosition,
            BackwardPosition = adjustment.Press.BackwardPosition,
            SpeedCoefficient = adjustment.Press.SpeedCoefficient
        };

        Rotation = new AdjustmentParametersRotationVisibleDto()
        {
            OffsetAfterZeroSearch = adjustment.Rotation.OffsetAfterZeroSearch,
            SpeedCoefficient = adjustment.Rotation.SpeedCoefficient
        };

        Squeeze = new AdjustmentParametersSqueezeVisibleDto()
        {
            FrontPositionLimitation = adjustment.Squeeze.FrontPositionLimitation,
            SpeedCoefficient = adjustment.Squeeze.SpeedCoefficient
        };

        Supply = new AdjustmentParametersSupplyVisibleDto()
        {
            PressZonePosition = adjustment.Supply.PressZonePosition,
            ForwardDangerZonePosition = adjustment.Supply.ForwardDangerZonePosition,
            ColletJawsDepth = adjustment.Supply.ColletJawsDepth,
            SpeedCoefficient = adjustment.Supply.SpeedCoefficient
        };
    }

    private async void LoadSettings()
    {
        var settings = await _settingsRepository.GetAsync().ConfigureAwait(false);
        IsElectricMachine = settings?.IsElectricMachine ?? false;
    }

    [RelayCommand]
    private async Task EditParameters(string parameter)
    {
        //var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(SelectedAdjustment, parameter);
        //parametersSettingsWindow.ShowDialog();

        //AdjustmentParameters? adjustment = SelectedAdjustment.Parse(SelectedAdjustment.Id);
        //if (adjustment is not null)
        //    await _repository.UpdateElementAsync(adjustment).ConfigureAwait(false);
    }
}
