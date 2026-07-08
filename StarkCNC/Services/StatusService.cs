using StarkCNC.Core.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.Services;

public class StatusService : IStatusService
{
    public ObservableCollection<Status> CurrentStatuses { get; } = new ObservableCollection<Status>();

    public ObservableCollection<Status> History { get; } = new ObservableCollection<Status>();

    public void AddStatus(Status status)
    {
        CurrentStatuses.Add(status);
    }

    public void RemoveStatus(Status status)
    {
        if (CurrentStatuses.Contains(status))
            CurrentStatuses.Remove(status);
        History.Add(status);
    }

    public void RemoveStatusThatsContains(Status status)
    {
        if (status is null)
            return;

        Status? removeItem = null;
        foreach (var item in CurrentStatuses)
        {
            if (item.Text.Contains(status.Text, StringComparison.CurrentCulture) && item.Type == status.Type)
                removeItem = item;
        }
        if (removeItem is not null)
        {
            CurrentStatuses.Remove(removeItem);
            History.Add(removeItem);
        }
    }
}