using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentViewModel : ObservableObject
{
    private readonly AdjustmentListView _adjustmentListPage;

    private readonly IConfiguration _configuration;
    private readonly INavigationService _navigationService;
    private readonly IAdjustmentRepository _repository;

    public ObservableCollection<AdjustmentParametersDto> Adjustments { get; } = new ObservableCollection<AdjustmentParametersDto>();

    [ObservableProperty]
    private AdjustmentParametersDto? _selectedAdjustment;
    public ObservableCollection<AdjustmentParametersDto> SetUpAdjustments { get; private set; } = new ObservableCollection<AdjustmentParametersDto>();

    public AdjustmentViewModel(IConfiguration configuration, INavigationService navigationService, IAdjustmentRepository adjustmentRepository) 
    {
        _adjustmentListPage = new AdjustmentListView(this);

        _configuration = configuration;
        _navigationService = navigationService;
        _repository = adjustmentRepository;

        foreach (var item in _repository.GetAll())
        {
            Adjustments.Add(new AdjustmentParametersDto(item));
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
    private void CreateAdjustment()
    {
        var item = new StarkCNC.Core.Models.AdjustmentParameters(_configuration, "");
        _repository.AddElement(item);

        var adjustment = new AdjustmentParametersDto(item);
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
        }
    }

    [RelayCommand]
    private void DeleteAdjustment(AdjustmentParametersDto adjustment)
    {
        _repository.RemoveElement(adjustment.Cast());
        Adjustments.Remove(adjustment);
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
        _navigationService.Navigate(new AdjustmentCoordinateSettingsView(this, adjustment));
    }

    [RelayCommand]
    private void SetLevelToAdjustment(int level)
    {
        if (SelectedAdjustment is not null)
            _repository.SetLevel(SelectedAdjustment.Cast(), level);

        GetSetUpAdjustments();
    }

    [RelayCommand]
    private void GoToEditParametersSettings(string parameter)
    {
        if (SelectedAdjustment is null)
            return;

        var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(string.Empty, SelectedAdjustment, parameter);
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
                    .Where(a => a.Cast().Equals(awl))
                    .ToList()
                    .ForEach(a => SetUpAdjustments.Add(a));
        });
    }
}
