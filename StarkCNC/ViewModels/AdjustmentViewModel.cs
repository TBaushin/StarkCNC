using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.DTO;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentViewModel : ObservableObject
{
    private readonly IRouter _router;
    private readonly IAdjustmentRepository _repository;

    public ObservableCollection<AdjustmentParameters> Adjustments { get; } = new ObservableCollection<AdjustmentParameters>();

    [ObservableProperty]
    private AdjustmentParameters? _selectedAdjustment;

    public ObservableCollection<AdjustmentParameters> SetUpAdjustments { get; private set; } = new ObservableCollection<AdjustmentParameters>();

    [ObservableProperty]
    private bool _firstFloorEnabled;

    [ObservableProperty]
    private bool _secondFloorEnabled;

    [ObservableProperty]
    private bool _thirdFloorEnabled;

    public AdjustmentViewModel(IRouter router, IAdjustmentRepository adjustmentRepository) 
    {
        _router = router;
        _repository = adjustmentRepository;

        UpdateAdjustments();

        GetSetUpAdjustments();
    }

    public void SelectAdjustment(AdjustmentParameters adjustment)
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
    private void GoToAdjustmentList()
    {
        _router.Navigate("/adjustment/list");
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
    private void NavigateToFirstFloorEdit()
    {
        var firstFloorAdjustment = SetUpAdjustments.Where(a => a.InstalledLevel == 1).FirstOrDefault();
        if (firstFloorAdjustment is null)
            return;

        _router.Navigate("/adjustment/list/edit", firstFloorAdjustment.Id);
    }

    [RelayCommand]
    private void NavigateToSecondFloorEdit()
    {
        var firstFloorAdjustment = SetUpAdjustments.Where(a => a.InstalledLevel == 2).FirstOrDefault();
        if (firstFloorAdjustment is null)
            return;

        _router.Navigate("/adjustment/list/edit", firstFloorAdjustment.Id);
    }

    [RelayCommand]
    private void NavigateToThirdFloorEdit()
    {
        var firstFloorAdjustment = SetUpAdjustments.Where(a => a.InstalledLevel == 3).FirstOrDefault();
        if (firstFloorAdjustment is null)
            return;

        _router.Navigate("/adjustment/list/edit", firstFloorAdjustment.Id);
    }

    private async void UpdateAdjustments()
    {
        Adjustments.Clear();
        var adjustments = await _repository.GetAllAsync().ConfigureAwait(false);
        foreach (var item in adjustments)
        {
            Adjustments.Add(item);
        }
    }

    private async void GetSetUpAdjustments()
    {
        SetUpAdjustments.Clear();
        var adjustmentsWithLevel = await _repository
            .GetAdjustmentsWithLevelAsync().ConfigureAwait(false);
        adjustmentsWithLevel.ToList().ForEach(awl =>
        {
            Adjustments
                    .Where(a => a.Id.Equals(awl.Id))
                    .ToList()
                    .ForEach(a => SetUpAdjustments.Add(a));

            switch (awl.InstalledLevel)
            {
                case 1:
                    FirstFloorEnabled = awl.IsEnabled;
                    break;
                case 2:
                    SecondFloorEnabled = awl.IsEnabled;
                    break;
                case 3:
                    ThirdFloorEnabled = awl.IsEnabled;
                    break;
            }
        });
    }

    partial void OnFirstFloorEnabledChanged(bool oldValue, bool newValue)
    {
        var adjustment = SetUpAdjustments.FirstOrDefault(a => a.InstalledLevel == 1);
        if (adjustment is not null)
        {
            adjustment.IsEnabled = newValue;
            try
            {
                _repository.UpdateElementAsync(adjustment).GetAwaiter().GetResult();
            }
            catch
            {
                // Ignore
            }
        }
    }

    partial void OnSecondFloorEnabledChanged(bool oldValue, bool newValue)
    {
        var adjustment = SetUpAdjustments.FirstOrDefault(a => a.InstalledLevel == 2);
        if (adjustment is not null)
        {
            adjustment.IsEnabled = newValue;
            try
            {
                _repository.UpdateElementAsync(adjustment).GetAwaiter().GetResult();
            }
            catch
            {
                // Ignore
            }
        }
    }

    partial void OnThirdFloorEnabledChanged(bool oldValue, bool newValue)
    {
        var adjustment = SetUpAdjustments.FirstOrDefault(a => a.InstalledLevel == 3);
        if (adjustment is not null)
        {
            adjustment.IsEnabled = newValue;
            try
            {
                _repository.UpdateElementAsync(adjustment).GetAwaiter().GetResult();
            }
            catch
            {
                // Ignore
            }
        }
    }
}