using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace StarkCNC.ViewModels;

public partial class AdjustmentListViewModel : ViewModelBase
{
    private readonly IAdjustmentRepository _repository;

    ObservableCollection<AdjustmentParametersVisibleDto> Adjustments = new ObservableCollection<AdjustmentParametersVisibleDto>();

    public AdjustmentListViewModel(IAdjustmentRepository repository)
    {
        _repository = repository;

        LoadAdjustmentsAsync();
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
            LoadAdjustmentsAsync();
        }
    }
}
