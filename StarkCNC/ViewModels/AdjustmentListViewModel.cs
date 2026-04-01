using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.DTO;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentListViewModel : ViewModelBase
{
    private readonly IRouter _router;
    private readonly IAdjustmentRepository _repository;
    private readonly IManualConfigurationService _manualConfigurationService;
    private readonly AdjustmentParametersConstructor _adjustmentConstructor;

    private string _typeRequestString = string.Empty;

    private string _pipeDiameterRequestString = string.Empty;

    private string _radiusRequestString = string.Empty;

    public ObservableCollection<AdjustmentParametersVisibleDto> Adjustments { get; } = new ObservableCollection<AdjustmentParametersVisibleDto>();

    public AdjustmentListViewModel(
        IRouter router,
        IAdjustmentRepository repository,
        IManualConfigurationService manualConfigurationService,
        AdjustmentParametersConstructor adjustmentConstructor)
    {
        _router = router;
        _repository = repository;
        _manualConfigurationService = manualConfigurationService;
        _adjustmentConstructor = adjustmentConstructor;
        ReadRequestsFromConfiguration();

        LoadAdjustmentsAsync();
    }

    private void ReadRequestsFromConfiguration()
    {
        var adjustmentSection = App.Configuration.GetSection("Adjustment");

        var adjustmentTypeSection = adjustmentSection.GetSection("AdjustmentType");
        _typeRequestString = adjustmentTypeSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeDiameterSection = adjustmentSection.GetSection("PipeDiameter");
        _pipeDiameterRequestString = pipeDiameterSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        //var radius
    }

    public async void LoadAdjustmentsAsync()
    {
        var adjustments = await _repository.GetAllAsync().ConfigureAwait(true);
        Adjustments.Clear();

        foreach (var adjustment in adjustments)
        {
            Adjustments.Add(
                new AdjustmentParametersVisibleDto()
                {
                    Name = adjustment.Name,
                    AdjustmentType = adjustment.Type,
                    PipeDiameter = adjustment.PipeDiameter,
                    Radius = adjustment.Radius
                });
        }
    }

    [RelayCommand]
    private async Task CreateAdjustment()
    {
        var settingsWindow = new AdjustmentSettingsWindow(null, _adjustmentConstructor);
        settingsWindow.ShowDialog();
        var result = settingsWindow.Result;

        if (result is null)
            return;

        var adjustment = await _adjustmentConstructor
            .Build(result.Name, result.Type, result.PipeDiameter, result.Radius)
            .ConfigureAwait(true);

        await _repository.AddElementAsync(adjustment).ConfigureAwait(true);

        LoadAdjustmentsAsync();
    }

    [RelayCommand]
    private async Task DeleteAdjustment(AdjustmentParametersVisibleDto? adjustment)
    {
        if (adjustment is null)
            return;

        var adjustmentsToDelete = await _repository.FindByNameAsync(adjustment.Name).ConfigureAwait(true);
        if (adjustmentsToDelete is null)
            return;

        var adjustmentToDelete = adjustmentsToDelete.FirstOrDefault();
        if (adjustmentToDelete is not null)
        {
            await _repository.RemoveElementAsync(adjustmentToDelete.Id).ConfigureAwait(true);
            LoadAdjustmentsAsync();
        }
    }

    [RelayCommand]
    private async Task EditAdjustment(AdjustmentParametersVisibleDto? adjustment)
    {
        if (adjustment is null)
            return;

        var adjustmentsToEdit = await _repository.FindByNameAsync(adjustment.Name).ConfigureAwait(true);
        if (adjustmentsToEdit is null)
            return;

        var adjustmentToEdit = adjustmentsToEdit.FirstOrDefault();
        if (adjustmentToEdit is not null)
        {
            _router.Navigate("/adjustment/list/edit", adjustmentToEdit.Id);
        }
    }

    //[RelayCommand]
    //private async Task EditAdjustment(AdjustmentParametersVisibleDto? adjustment)
    //{
    //    if (adjustment is null)
    //        return;

    //    var settingsWindow = new AdjustmentSettingsWindow(adjustment, "Редактирование оснастки");
    //    settingsWindow.ShowDialog();

    //    var result = settingsWindow.Result;
    //    if (result is null)
    //        return;

    //    var adjustmentsToUpdate = await _repository.FindByNameAsync(adjustment.Name).ConfigureAwait(false);
    //    if (adjustmentsToUpdate is null)
    //        return;

    //    var adjustmentToUpdate = adjustmentsToUpdate.FirstOrDefault();
    //    if (adjustmentToUpdate is not null)
    //    {
    //        adjustmentToUpdate.Name = result.Name;
    //        adjustmentToUpdate.Type = result.AdjustmentType;
    //        adjustmentToUpdate.PipeDiameter = result.PipeDiameter;
    //        adjustmentToUpdate.Radius = result.Radius;

    //        await _repository.UpdateElementAsync(adjustmentToUpdate).ConfigureAwait(false);

    //        // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
    //        await _manualConfigurationService.WriteAsync(adjustmentToUpdate.Type, _typeRequestString).ConfigureAwait(false);
    //        await _manualConfigurationService.WriteAsync(adjustmentToUpdate.PipeDiameter, _pipeDiameterRequestString).ConfigureAwait(false);

    //        LoadAdjustmentsAsync();
    //    }
    //}
}
