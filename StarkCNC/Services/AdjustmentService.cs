using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;

namespace StarkCNC.Services;

public class AdjustmentService : IAdjustmentService
{
    private IAdjustmentRepository _repository;
    private IManualConfigurationService _manualConfigurationService;

    public AdjustmentParameters? FirstLevelAdjustment => GetAdjustmentWithLevelAsync(1).Result;

    public AdjustmentParameters? SecondLevelAdjustment => GetAdjustmentWithLevelAsync(2).Result;

    public AdjustmentParameters? ThirdLevelAdjustment => GetAdjustmentWithLevelAsync(3).Result;

    public int CurrentLevel { get; private set; } = 1;

    public AdjustmentService(IAdjustmentRepository repository, IManualConfigurationService manualConfigurationService)
    {
        _repository = repository;
        _manualConfigurationService = manualConfigurationService;
    }

    public async Task<AdjustmentParameters?> AddElementAsync(AdjustmentParameters adjustment) =>
        await _repository.AddElementAsync(adjustment).ConfigureAwait(false);

    public async Task AdjustmentStartDown()
    {
        await _manualConfigurationService
            .WriteAsync(true, ControllerRequestStrings.ADJUSTMENT_BACKWARD)
            .ConfigureAwait(false);

        if (CurrentLevel > 1)
            CurrentLevel -= 1;
    }

    public async Task AdjustmentStartUp()
    {
        await _manualConfigurationService
            .WriteAsync(true, ControllerRequestStrings.ADJUSTMENT_FORWARD)
            .ConfigureAwait(false);

        if (CurrentLevel < 3)
            CurrentLevel += 1;
    }

    public async Task AdjustmentStopDown()
    {
        await _manualConfigurationService
            .WriteAsync(false, ControllerRequestStrings.ADJUSTMENT_BACKWARD)
            .ConfigureAwait(false);
    }

    public async Task AdjustmentStopUp()
    {
        await _manualConfigurationService
            .WriteAsync(false, ControllerRequestStrings.ADJUSTMENT_FORWARD)
            .ConfigureAwait(false);
    }

    public int Count() => _repository.Count();

    public async Task<AdjustmentParameters?> FindByIdAsync(Guid id) =>
        await _repository.FindByIdAsync(id).ConfigureAwait(false);

    public async Task<IEnumerable<AdjustmentParameters>> FindByNameAsync(string name) =>
        await _repository.FindByNameAsync(name).ConfigureAwait(false);

    public async Task<IEnumerable<AdjustmentParameters>> GetAdjustmentsWithLevelAsync() =>
        await _repository.GetAdjustmentsWithLevelAsync().ConfigureAwait(false);

    public async Task<AdjustmentParameters?> GetAdjustmentWithLevelAsync(int level) =>
        await _repository.GetAdjustmentWithLevelAsync(level).ConfigureAwait(false);

    public async Task<IEnumerable<AdjustmentParameters>> GetAllAsync() =>
        await _repository.GetAllAsync().ConfigureAwait(false);

    public async Task RemoveElementAsync(Guid id) =>
        await _repository.RemoveElementAsync(id).ConfigureAwait(false);

    public async Task RemoveElementAsync(AdjustmentParameters adjustment) =>
        await _repository.RemoveElementAsync(adjustment).ConfigureAwait(false);

    public async Task SetLevelAsync(Guid id, int level)
    {
        var item = await _repository.FindByIdAsync(id).ConfigureAwait(false);
        if (item is null)
            return;

        var adjustments = await _repository.GetAdjustmentsWithLevelAsync().ConfigureAwait(false);
        foreach (var adjustment in adjustments)
        {
            if (adjustment.InstalledLevel == level)
            {
                adjustment.InstalledLevel = 0;
                await _repository.UpdateElementAsync(adjustment).ConfigureAwait(false);
            }
        }

        item.InstalledLevel = level;
        await _repository.UpdateElementAsync(item).ConfigureAwait(false);
    }

    public async Task UpdateElementAsync(AdjustmentParameters adjustment) =>
        await _repository.UpdateElementAsync(adjustment).ConfigureAwait(false);
}
