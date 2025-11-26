using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.MachineCommunication.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class AdjustmentListViewModel : ViewModelBase
{
    private readonly IAdjustmentRepository _repository;
    private readonly IManualConfigurationService _manualConfigurationService;

    private string _typeRequestString = string.Empty;

    private string _pipeDiameterRequestString = string.Empty;

    private string _radiusRequestString = string.Empty;

    ObservableCollection<AdjustmentParametersVisibleDto> Adjustments = new ObservableCollection<AdjustmentParametersVisibleDto>();

    public AdjustmentListViewModel(IAdjustmentRepository repository, IManualConfigurationService manualConfigurationService)
    {
        _repository = repository;
        _manualConfigurationService = manualConfigurationService;
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
        var adjustments = await _repository.GetAllAsync().ConfigureAwait(false);
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
        var settingsWindow = new AdjustmentSettingsWindow(null);
        settingsWindow.ShowDialog();
        var result = settingsWindow.Result;

        if (result is null)
            return;

        // Create new adjustment
        LoadAdjustmentsAsync();
    }

    [RelayCommand]
    private async Task DeleteAdjustment(AdjustmentParametersVisibleDto? adjustment)
    {
        if (adjustment is null)
            return;

        var adjustmentsToDelete = await _repository.FindByNameAsync(adjustment.Name).ConfigureAwait(false);
        if (adjustmentsToDelete is null)
            return;

        var adjustmentToDelete = adjustmentsToDelete.FirstOrDefault();
        if (adjustmentToDelete is not null)
        {
            await _repository.RemoveElementAsync(adjustmentToDelete.Id).ConfigureAwait(false);
            LoadAdjustmentsAsync();
        }
    }

    [RelayCommand]
    private async Task EditAdjustment(AdjustmentParametersVisibleDto? adjustment)
    {
        if (adjustment is null)
            return;

        var settingsWindow = new AdjustmentSettingsWindow(adjustment, "Редактирование оснастки");
        settingsWindow.ShowDialog();

        var result = settingsWindow.Result;
        if (result is null)
            return;

        var adjustmentsToUpdate = await _repository.FindByNameAsync(adjustment.Name).ConfigureAwait(false);
        if (adjustmentsToUpdate is null)
            return;

        var adjustmentToUpdate = adjustmentsToUpdate.FirstOrDefault();
        if (adjustmentToUpdate is not null)
        {
            adjustmentToUpdate.Name = result.Name;
            adjustmentToUpdate.Type = result.AdjustmentType;
            adjustmentToUpdate.PipeDiameter = result.PipeDiameter;
            adjustmentToUpdate.Radius = result.Radius;

            await _repository.UpdateElementAsync(adjustmentToUpdate).ConfigureAwait(false);

            // TODO: Обновлять только если выбранная оснастка совпадает с редактируемой
            await _manualConfigurationService.WriteAsync(adjustmentToUpdate.Type, _typeRequestString).ConfigureAwait(false);
            await _manualConfigurationService.WriteAsync(adjustmentToUpdate.PipeDiameter, _pipeDiameterRequestString).ConfigureAwait(false);

            LoadAdjustmentsAsync();
        }
    }
}
