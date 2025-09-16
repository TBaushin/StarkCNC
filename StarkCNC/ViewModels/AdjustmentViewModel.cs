using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentViewModel : ObservableObject
{
    private readonly AdjustmentListView _adjustmentListPage;

    private readonly INavigationService _navigationService;
    private readonly IAdjustmentRepository _repository;

    public ObservableCollection<AdjustmentParametersDto> Adjustments { get; } = new ObservableCollection<AdjustmentParametersDto>();
    public IEnumerable<StarkCNC.Core.Models.AdjustmentType> Types { get; }

    [ObservableProperty]
    private AdjustmentParametersDto? _selectedAdjustment;
    public ObservableCollection<AdjustmentParametersDto> SetUpAdjustments
    {
        get => GetSetUpAdjustments();
    }

    public AdjustmentViewModel(INavigationService navigationService, IAdjustmentRepository adjustmentRepository) 
    {
        _adjustmentListPage = new AdjustmentListView(this);

        _navigationService = navigationService;
        _repository = adjustmentRepository;

        foreach (var item in _repository.GetAll())
        {
            Adjustments.Add(new AdjustmentParametersDto(item));
        }

        Types = _repository.GetTypes();
    }

    [RelayCommand]
    private void CreateAdjustment()
    {
        var item = new StarkCNC.Core.Models.AdjustmentParameters("", _repository.GetTypes().First());
        _repository.AddElement(item);

        var adjustment = new AdjustmentParametersDto(item);
        Adjustments.Add(adjustment);

        SelectedAdjustment = adjustment;
        var settingsWindow = new AdjustmentSettingsWindow(this);
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Radius = result.Radius;
            SelectedAdjustment.Type = result.Type;
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
        var settingsWindow = new AdjustmentSettingsWindow(this, "Редактирование оснастки");
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null && SelectedAdjustment is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Radius = result.Radius;
            SelectedAdjustment.Type = result.Type;
            SelectedAdjustment.ClampLength = result.ClampLength;
            SelectedAdjustment.PressLength = result.PressLength;
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
        SelectedAdjustment = Adjustments.FirstOrDefault(a => a.Cast().Equals(adjustment.Cast()));
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
    }

    private ObservableCollection<AdjustmentParametersDto> GetSetUpAdjustments()
    {
        var result = new ObservableCollection<AdjustmentParametersDto>();
        var adjustmentsWithLevel = _repository
            .GetAdjustmentsWithLevel()
            .ToList();
        adjustmentsWithLevel.ForEach(awl =>
        {
            Adjustments
                    .Where(a => a.Cast().Equals(awl))
                    .ToList()
                    .ForEach(a => result.Add(a));
        });

        return result;
    }
}
