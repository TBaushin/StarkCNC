using StarkCNC.Core.Repository;

namespace StarkCNC.ViewModels;

public class AdjustmentParametersCoordinatesViewModel : ViewModelBase
{
    private readonly IAdjustmentRepository _repository;

    public AdjustmentParametersCoordinatesViewModel(IAdjustmentRepository repository)
    {
        _repository = repository;
    }

    public async Task SetSelectedAdjustment(Guid? id)
    {
        if (id is not Guid guid)
            throw new ArgumentNullException(nameof(id));

        var adjustment = await _repository.FindByIdAsync(guid).ConfigureAwait(false);
        if (adjustment is not null)
        {
            return;
        }

        throw new InvalidOperationException("Не удалось найти оснастку");
    }
}
