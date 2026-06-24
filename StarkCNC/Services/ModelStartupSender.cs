using StarkCNC.Core.Providers;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Services;

public class ModelStartupSender<TModel> : IMachineStartupSender where TModel : class
{
    private readonly Func<Task<TModel>> _loadModel;
    private readonly IManualConfigurationService _manualService;
    private readonly IMachineCommandProvider<TModel> _provider;

    public string Name => typeof(TModel).Name;

    public ModelStartupSender(
        Func<Task<TModel>> loadModel,
        IManualConfigurationService manualService,
        IMachineCommandProvider<TModel> provider)
    {
        _loadModel = loadModel;
        _manualService = manualService;
        _provider = provider;
    }

    public async Task SendAllAsync()
    {
        var model = await _loadModel().ConfigureAwait(false);
        if (model is null)
            return;

        var task = _provider
            .GetCommands(model)
            .Select(cmd => _manualService.WriteAsync(cmd.Value, cmd.Address));

        await Task.WhenAll(task).ConfigureAwait(false);
    }
}
