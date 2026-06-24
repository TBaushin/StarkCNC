namespace StarkCNC.Services;

public class StartupSendService : IStartupSendService
{
    private readonly IEnumerable<IMachineStartupSender> _senders;

    public StartupSendService(IEnumerable<IMachineStartupSender> senders)
    {
        _senders = senders;
    }

    public async Task SendAllAsync(IProgress<(int current, int total, string name)>? progress = null)
    {
        var sendersList = _senders.ToList();
        for (int i = 0; i < sendersList.Count; i++)
        {
            var sender = sendersList[i];
            progress?.Report((i, sendersList.Count, sender.Name));
            await sender.SendAllAsync().ConfigureAwait(false);
        }
        progress?.Report((sendersList.Count, sendersList.Count, "Готово"));
    }
}
