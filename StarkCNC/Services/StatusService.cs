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
}