using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentViewModel : ObservableObject
{
    private readonly AdjustmentListView _adjustmentListPage;

    private readonly INavigationService _navigationService;
    private readonly IAdjustmentRepository _repository;

    public ObservableCollection<AdjustmentParameters> Adjustments { get; } = new ObservableCollection<AdjustmentParameters>();
    public IEnumerable<StarkCNC.Core.Models.AdjustmentType> Types { get; }

    [ObservableProperty]
    private AdjustmentParameters? _selectedAdjustment;

    public AdjustmentViewModel(INavigationService navigationService, IAdjustmentRepository adjustmentRepository) 
    {
        _adjustmentListPage = new AdjustmentListView(this);

        _navigationService = navigationService;
        _repository = adjustmentRepository;

        foreach (var item in _repository.GetAll())
        {
            Adjustments.Add(new AdjustmentParameters(item));
        }

        Types = _repository.GetTypes();
    }

    [RelayCommand]
    private void CreateAdjustment()
    {
        var item = new StarkCNC.Core.Models.AdjustmentParameters("", _repository.GetTypes().First());
        _repository.AddElement(item);

        var adjustment = new AdjustmentParameters(item);
        Adjustments.Add(adjustment);

        SelectedAdjustment = adjustment;
        var settingsWindow = new AddAdjustmentWindow(this);
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Type = result.Type;
        }
    }

    [RelayCommand]
    private void DeleteAdjustment(AdjustmentParameters adjustment)
    {
        _repository.RemoveElement(adjustment.Cast());
        Adjustments.Remove(adjustment);
    }

    [RelayCommand]
    private void EditAdjustment(AdjustmentParameters adjustment)
    {
        SelectedAdjustment = adjustment;

        var settingsWindow = new AddAdjustmentWindow(this);
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is not null)
        {
            SelectedAdjustment.Name = result.Name;
            SelectedAdjustment.PipeDiameter = result.PipeDiameter;
            SelectedAdjustment.Type = result.Type;
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
    private void SetLevelToAdjustment(int level)
    {
        if (SelectedAdjustment is not null)
            _repository.SetLevel(SelectedAdjustment.Cast(), level);
    }
}
