using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.Models;
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
            SelectedAdjustment.BendRoller = result.BendRoller;
            SelectedAdjustment.Clamp = result.Clamp;
            SelectedAdjustment.ClampRoller = result.ClampRoller;
            SelectedAdjustment.Console = result.Console;
            SelectedAdjustment.Press = result.Press;
            SelectedAdjustment.Squeeze = result.Squeeze;
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
            SelectedAdjustment.BendRoller = result.BendRoller;
            SelectedAdjustment.Clamp = result.Clamp;
            SelectedAdjustment.ClampRoller = result.ClampRoller;
            SelectedAdjustment.Console = result.Console;
            SelectedAdjustment.Press = result.Press;
            SelectedAdjustment.Squeeze = result.Squeeze;
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
        _navigationService.Navigate(new AdjustmentCoordinateSettingsView(adjustment));
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

        AdjustmentParametersSettingsWindow? parametersSettingsWindow;
        IEnumerable<TitleValue>? result;
        switch (parameter)
        {
            case nameof(SelectedAdjustment.BendRoller):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Гибочный ролик",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.Radius), "Радиус", SelectedAdjustment.BendRoller.Radius),
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.OuterRadius), "Внешний радиус", SelectedAdjustment.BendRoller.OuterRadius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.Radius))
                            SelectedAdjustment.BendRoller.Radius = (double)e.Value;
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.OuterRadius))
                            SelectedAdjustment.BendRoller.OuterRadius = (double)e.Value;
                    });
                break;
            case nameof(SelectedAdjustment.Clamp):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Зажим",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.Radius), "Радиус", SelectedAdjustment.BendRoller.Radius),
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.OuterRadius), "Внешний радиус", SelectedAdjustment.BendRoller.OuterRadius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.Radius))
                            SelectedAdjustment.BendRoller.Radius = (double)e.Value;
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.OuterRadius))
                            SelectedAdjustment.BendRoller.OuterRadius = (double)e.Value;
                    });
                break;
            case nameof(SelectedAdjustment.ClampRoller):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Ролик зажима",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.Radius), "Радиус", SelectedAdjustment.BendRoller.Radius),
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.OuterRadius), "Внешний радиус", SelectedAdjustment.BendRoller.OuterRadius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.Radius))
                            SelectedAdjustment.BendRoller.Radius = (double)e.Value;
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.OuterRadius))
                            SelectedAdjustment.BendRoller.OuterRadius = (double)e.Value;
                    });
                break;
            case nameof(SelectedAdjustment.Console):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Консоль",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.Radius), "Радиус", SelectedAdjustment.BendRoller.Radius),
                        new TitleValue(nameof(SelectedAdjustment.BendRoller.OuterRadius), "Внешний радиус", SelectedAdjustment.BendRoller.OuterRadius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.Radius))
                            SelectedAdjustment.BendRoller.Radius = (double)e.Value;
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.OuterRadius))
                            SelectedAdjustment.BendRoller.OuterRadius = (double)e.Value;
                    });
                break;
            case nameof(SelectedAdjustment.Press):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Прижим",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.Press.DangerZoneCoordinate), "Радиус", SelectedAdjustment.BendRoller.Radius),
                        new TitleValue(nameof(SelectedAdjustment.Press.Length), "Внешний радиус", SelectedAdjustment.BendRoller.OuterRadius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.Radius))
                            SelectedAdjustment.BendRoller.Radius = (double)e.Value;
                        if (e.PropertyName == nameof(SelectedAdjustment.BendRoller.OuterRadius))
                            SelectedAdjustment.BendRoller.OuterRadius = (double)e.Value;
                    });
                break;
            case nameof(SelectedAdjustment.Squeeze):
                parametersSettingsWindow = new AdjustmentParametersSettingsWindow(
                    "Дожим",
                    new List<TitleValue>()
                    {
                        new TitleValue(nameof(SelectedAdjustment.Squeeze.TurnOn), "Радиус", SelectedAdjustment.BendRoller.Radius)
                    });
                parametersSettingsWindow.Show();

                result = parametersSettingsWindow.Result;
                if (result is not null)
                    result.ToList().ForEach(e =>
                    {
                        if (e.PropertyName == nameof(SelectedAdjustment.Squeeze.TurnOn))
                            SelectedAdjustment.Squeeze.TurnOn = (bool)e.Value;
                    });
                break;

        }
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
