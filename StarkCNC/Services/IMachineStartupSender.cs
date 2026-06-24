namespace StarkCNC.Services;

public interface IMachineStartupSender
{
    string Name { get; }

    Task SendAllAsync();
}
