using StarkCNC.Core.Providers;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Services;

public class CollectionStartupSender<TModel> : IMachineStartupSender where TModel : class
{
    private readonly Func<Task<IEnumerable<TModel>>> _loadModels;
    private readonly IManualConfigurationService _manualService;
    private readonly IMachineCommandProvider<TModel> _provider;

    public string Name => typeof(TModel).Name;

    public CollectionStartupSender(
        Func<Task<IEnumerable<TModel>>> loadModels,
        IManualConfigurationService manualService,
        IMachineCommandProvider<TModel> provider)
    {
        _loadModels = loadModels;
        _manualService = manualService;
        _provider = provider;
    }

    public async Task SendAllAsync()
    {
        var models = await _loadModels().ConfigureAwait(false);

        var task = models
            .SelectMany(m => _provider.GetCommands(m))
            .Select(cmd => _manualService.WriteAsync(cmd.Value, cmd.Address));

        await Task.WhenAll(task).ConfigureAwait(false);
    }
}
