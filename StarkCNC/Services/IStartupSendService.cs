namespace StarkCNC.Services;

public interface IStartupSendService
{
    Task SendAllAsync(IProgress<(int current, int total, string name)>? progress = null);
}
