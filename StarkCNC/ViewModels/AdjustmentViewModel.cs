using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
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

    private readonly INavigationService _navigationService;
    private readonly IRouter _router;
    private readonly IAdjustmentRepository _repository;

    private readonly SettingsViewModel _settingsViewModel;

    public ObservableCollection<AdjustmentParameters> Adjustments { get; } = new ObservableCollection<AdjustmentParameters>();

    [ObservableProperty]
    private AdjustmentParameters? _selectedAdjustment;

    public ObservableCollection<AdjustmentParameters> SetUpAdjustments { get; private set; } = new ObservableCollection<AdjustmentParameters>();

    public AdjustmentViewModel(
        INavigationService navigationService,
        IRouter router,
        IAdjustmentRepository adjustmentRepository,
        SettingsViewModel settingsViewModel) 
    {
        _navigationService = navigationService;
        _router = router;
        _repository = adjustmentRepository;

        _settingsViewModel = settingsViewModel;

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

    //[RelayCommand]
    //private async Task CreateAdjustment()
    //{
    //    var adjustment = AdjustmentParametersDto.CreateFromConfiguration();
    //    if (adjustment is null)
    //        throw new ArgumentNullException(nameof(adjustment));

    //    Adjustments.Add(adjustment);

    //    SelectedAdjustment = adjustment;
    //    //var settingsWindow = new AdjustmentSettingsWindow(this);
    //    //settingsWindow.ShowDialog();

    //    //var result = settingsWindow.Result;
    //    //if (result is not null)
    //    //{
    //    //    SelectedAdjustment.Name = result.Name;
    //    //    SelectedAdjustment.PipeDiameter = result.PipeDiameter;
    //    //    SelectedAdjustment.Radius = result.Radius;
    //    //    SelectedAdjustment.Type = result.Type;
    //    //    SelectedAdjustment.ForwardDangerZoneCoordinate = result.ForwardDangerZoneCoordinate;
    //    //    SelectedAdjustment.DistanceFromCenter = result.DistanceFromCenter;
    //    //    SelectedAdjustment.Bend = result.Bend;
    //    //    SelectedAdjustment.BendRoller = result.BendRoller;
    //    //    SelectedAdjustment.Clamp = result.Clamp;
    //    //    SelectedAdjustment.ClampRoller = result.ClampRoller;
    //    //    SelectedAdjustment.Console = result.Console;
    //    //    SelectedAdjustment.Dorn = result.Dorn;
    //    //    SelectedAdjustment.Lift = result.Lift;
    //    //    SelectedAdjustment.Press = result.Press;
    //    //    SelectedAdjustment.Rotation = result.Rotation;
    //    //    SelectedAdjustment.Squeeze = result.Squeeze;
    //    //    SelectedAdjustment.Supply = result.Supply;

    //    //    var newAdjustment = SelectedAdjustment.Parse(SelectedAdjustment.Id);
    //    //    if (newAdjustment is not null)
    //    //    {
    //    //        var item = await _repository.AddElementAsync(newAdjustment).ConfigureAwait(false);
    //    //        if (item is null)
    //    //            return;

    //    //        SelectedAdjustment = item.ToDto();
    //    //    }
    //    //}
        
    //    UpdateAdjustments();
    //}

    //[RelayCommand]
    //private async Task DeleteAdjustment(AdjustmentParametersDto adjustment)
    //{
    //    if (adjustment is null)
    //        return;

    //    if (adjustment.Id is not Guid id)
    //    {
    //        Adjustments.Remove(adjustment);
    //    }
    //    else
    //    {
    //        Adjustments.Remove(adjustment);
    //        await _repository.RemoveElementAsync(id).ConfigureAwait(false);
    //    }
        
    //    UpdateAdjustments();
    //}

    //[RelayCommand]
    //private async Task EditAdjustment(AdjustmentParametersDto adjustment)
    //{
    //    SelectedAdjustment = Adjustments.FirstOrDefault(a => a.Equals(adjustment));
    //    //var settingsWindow = new AdjustmentSettingsWindow(this, "Редактирование оснастки");
    //    //settingsWindow.ShowDialog();

    //    //var result = settingsWindow.Result;
    //    //if (result is not null && SelectedAdjustment is not null)
    //    //{
    //    //    SelectedAdjustment.Name = result.Name;
    //    //    SelectedAdjustment.PipeDiameter = result.PipeDiameter;
    //    //    SelectedAdjustment.Radius = result.Radius;
    //    //    SelectedAdjustment.Type = result.Type;
    //    //    SelectedAdjustment.ForwardDangerZoneCoordinate = result.ForwardDangerZoneCoordinate;
    //    //    SelectedAdjustment.DistanceFromCenter = result.DistanceFromCenter;
    //    //    SelectedAdjustment.Bend = result.Bend;
    //    //    SelectedAdjustment.BendRoller = result.BendRoller;
    //    //    SelectedAdjustment.Clamp = result.Clamp;
    //    //    SelectedAdjustment.ClampRoller = result.ClampRoller;
    //    //    SelectedAdjustment.Console = result.Console;
    //    //    SelectedAdjustment.Dorn = result.Dorn;
    //    //    SelectedAdjustment.Lift = result.Lift;
    //    //    SelectedAdjustment.Press = result.Press;
    //    //    SelectedAdjustment.Rotation = result.Rotation;
    //    //    SelectedAdjustment.Squeeze = result.Squeeze;
    //    //    SelectedAdjustment.Supply = result.Supply;

    //    //    AdjustmentParameters? adjustmentParameter = SelectedAdjustment.Parse(SelectedAdjustment.Id);
    //    //    if (adjustmentParameter is not null)
    //    //        await _repository.UpdateElementAsync(adjustmentParameter).ConfigureAwait(false);
    //    //}
        
    //    UpdateAdjustments();
    //}

    //[RelayCommand]
    //private void SaveAdjustment()
    //{
    //    SelectedAdjustment = null;
    //    _navigationService.Navigate(_adjustmentListPage);
    //}

    [RelayCommand]
    private void GoToAdjustmentList()
    {
        _router.Navigate("/adjustments/list");
    }

    [RelayCommand]
    private void GoToEditSettings(AdjustmentParametersDto adjustment)
    {
        SelectedAdjustment = Adjustments.FirstOrDefault(a => a.Equals(adjustment));
        //_navigationService.Navigate(new AdjustmentParametersView(this, adjustment));
    }

    [RelayCommand]
    private void GoToCoordinateSettings(AdjustmentParametersDto adjustment)
    {
        _navigationService.Navigate(new AdjustmentCoordinateSettingsView(
            _settingsViewModel.Settings,
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
    private async Task GoToEditParametersSettings(string parameter)
    {
        if (SelectedAdjustment is null)
            return;

        //var parametersSettingsWindow = new AdjustmentParametersSettingsWindow(SelectedAdjustment, parameter);
        //parametersSettingsWindow.ShowDialog();

        //AdjustmentParameters? adjustment = SelectedAdjustment.Parse(SelectedAdjustment.Id);
        //if (adjustment is not null)
        //    await _repository.UpdateElementAsync(adjustment).ConfigureAwait(false);
    }

    private async void UpdateAdjustments()
    {
        //Adjustments.Clear();
        //try
        //{
        //    foreach (var item in await _repository.GetAllAsync().ConfigureAwait(false))
        //    {
        //        var dto = item.ToDto();
        //        if (dto is null)
        //            continue;

        //        Adjustments.Add(dto);
        //    }
        //}
        //catch (Exception)
        //{
        //    // Ignore
        //}
    }

    private async void GetSetUpAdjustments()
    {
        try
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
            });
        }
        catch (Exception)
        {
            // Ignore
        }
    }

    private async Task<AdjustmentParametersDto?> SaveOrUpdateSettings(AdjustmentParametersDto adjustment)
    {
        AdjustmentParameters? item = adjustment.Parse(adjustment.Id);

        if (item is not null)
        {
            if (_repository.Count() == 0)
            {
                var settings = await _repository.AddElementAsync(item).ConfigureAwait(false);
                if (settings is not null)
                    return settings.ToDto();
            }
            else
                await _repository.UpdateElementAsync(item).ConfigureAwait(false);

            return item.ToDto();
        }

        return null;
    }

    private async void Adjustment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var adjustment = sender as AdjustmentParametersDto;
        if (adjustment is null)
            return;

        await SaveOrUpdateSettings(adjustment).ConfigureAwait(false);
    }
}