using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        if (adjustment is not null)
        {
            return;
        }

        throw new InvalidOperationException("Не удалось найти оснастку");
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
