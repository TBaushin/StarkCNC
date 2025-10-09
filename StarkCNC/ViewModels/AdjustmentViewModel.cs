using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.DTO;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentViewModel : ObservableObject
{
    private readonly AdjustmentListView _adjustmentListPage;

    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly INavigationService _navigationService;
    private readonly IAdjustmentRepository _repository;

    public ObservableCollection<AdjustmentParametersDto> Adjustments { get; } = new ObservableCollection<AdjustmentParametersDto>();

    [ObservableProperty]
    private AdjustmentParametersDto? _selectedAdjustment;
    public ObservableCollection<AdjustmentParametersDto> SetUpAdjustments { get; private set; } = new ObservableCollection<AdjustmentParametersDto>();

    public AdjustmentViewModel(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        INavigationService navigationService,
        IAdjustmentRepository adjustmentRepository) 
    {
        _adjustmentListPage = new AdjustmentListView(this);

        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _navigationService = navigationService;
        _repository = adjustmentRepository;

        foreach (var item in _repository.GetAll())
        {
            var dto = item.ToDto(configuration);
            if (dto is null)
                continue;

            Adjustments.Add(dto);
        }

        GetSetUpAdjustments();
    }

    public void SelectAdjustment(AdjustmentParametersDto adjustment)
    {
        foreach(var item in Adjustments)
        {
            if (!item.Equals(adjustment))
                continue;

            SelectedAdjustment = item;
            return;
        }
    }

    [RelayCommand]
    private async Task CreateAdjustment()
    {
        var adjustment = AdjustmentParametersDto.CreateFromConfiguration(_configuration);
        if (adjustment is null)
            throw new ArgumentNullException(nameof(adjustment));

        Adjustments.Add(adjustment);

        SelectedAdjustment = adjustment;
        var settingsWindow = new AdjustmentSettingsWindow(_configuration, this);
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Radius = result.Radius;
            SelectedAdjustment.Type = result.Type;
            SelectedAdjustment.ForwardDangerZoneCoordinate = result.ForwardDangerZoneCoordinate;
            SelectedAdjustment.DistanceFromCenter = result.DistanceFromCenter;
            SelectedAdjustment.Bend = result.Bend;
            SelectedAdjustment.BendRoller = result.BendRoller;
            SelectedAdjustment.Clamp = result.Clamp;
            SelectedAdjustment.ClampRoller = result.ClampRoller;
            SelectedAdjustment.Console = result.Console;
            SelectedAdjustment.Dorn = result.Dorn;
            SelectedAdjustment.Lift = result.Lift;
            SelectedAdjustment.Press = result.Press;
            SelectedAdjustment.Rotation = result.Rotation;
            SelectedAdjustment.Squeeze = result.Squeeze;
            SelectedAdjustment.Supply = result.Supply;

            var newAdjustment = SelectedAdjustment.Parse(SelectedAdjustment.Id);
            if (newAdjustment is not null)
                await _repository.AddElementAsync(newAdjustment).ConfigureAwait(false);
        }
    }

    [RelayCommand]
    private async Task DeleteAdjustment(AdjustmentParametersDto adjustment)
    {
        if (adjustment is null)
            return;

        if (adjustment.Id is not Guid id)
        {
            Adjustments.Remove(adjustment);
        }
        else
        {
            Adjustments.Remove(adjustment);
            await _repository.RemoveElementAsync(id).ConfigureAwait(false);
        }
    }

    [RelayCommand]
    private void EditAdjustment(AdjustmentParametersDto adjustment)
    {
        SelectedAdjustment = Adjustments.FirstOrDefault(a => a.Equals(adjustment));
        var settingsWindow = new AdjustmentSettingsWindow(_configuration, this, "Редактирование оснастки");
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null && SelectedAdjustment is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Radius = result.Radius;
            SelectedAdjustment.Type = result.Type;
            SelectedAdjustment.ForwardDangerZoneCoordinate = result.ForwardDangerZoneCoordinate;
            SelectedAdjustment.DistanceFromCenter = result.DistanceFromCenter;
            SelectedAdjustment.Bend = result.Bend;
            SelectedAdjustment.BendRoller = result.BendRoller;
            SelectedAdjustment.Clamp = result.Clamp;
            SelectedAdjustment.ClampRoller = result.ClampRoller;
            SelectedAdjustment.Console = result.Console;
            SelectedAdjustment.Dorn = result.Dorn;
            SelectedAdjustment.Lift = result.Lift;
            SelectedAdjustment.Press = result.Press;
            SelectedAdjustment.Rotation = result.Rotation;
            SelectedAdjustment.Squeeze = result.Squeeze;
            SelectedAdjustment.Supply = result.Supply;
        }
    }

    [RelayCommand]
    private void SaveAdjustment()
    {
        SelectedAdjustment = null;
        _navigationService.Navigate(_adjustmentListPage);
    }

    [RelayCommand]
    private void GoToAdjustmentList()
    {
        _navigationService.Navigate(_adjustmentListPage);
    }

    [RelayCommand]
    private void GoToEditSettings(AdjustmentParametersDto adjustment)
    {
        SelectedAdjustment = Adjustments.FirstOrDefault(a => a.Equals(adjustment));
        _navigationService.Navigate(new AdjustmentSettingsView(this, adjustment));
    }

    [RelayCommand]
    private void GoToCoordinateSettings(AdjustmentParametersDto adjustment)
    {
        _navigationService.Navigate(new AdjustmentCoordinateSettingsView(
            _serviceProvider.GetRequiredService<ISettingsService>(),
            this,
            adjustment));
    }

    [RelayCommand]
    private async Task SetLevelToAdjustment(int level)
    {
        if (SelectedAdjustment is not null && SelectedAdjustment.Id is Guid id)
        {
            await _repository.SetLevelAsync(id, level).ConfigureAwait(false);
        }

        GetSetUpAdjustments();
    }

    [RelayCommand]
    private void GoToEditParametersSettings(string parameter)
    {
        if (SelectedAdjustment is null)
            return;

        var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(SelectedAdjustment, parameter);
        parametersSettingsWindow.ShowDialog();
    }

    private void GetSetUpAdjustments()
    {
        SetUpAdjustments.Clear();
        var adjustmentsWithLevel = _repository
            .GetAdjustmentsWithLevel()
            .ToList();
        adjustmentsWithLevel.ForEach(awl =>
        {
            Adjustments
                    .Where(a => a.Id.Equals(awl.Id))
                    .ToList()
                    .ForEach(a => SetUpAdjustments.Add(a));
        });
    }
}
