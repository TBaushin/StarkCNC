namespace StarkCNC.Core.Providers;

public interface IMachineCommandProvider<TModel>
{
    IEnumerable<MachineCommand> GetCommands(TModel model);
}
