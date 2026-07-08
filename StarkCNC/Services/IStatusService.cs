using StarkCNC.Core.Models;
using System.Collections.ObjectModel;

namespace StarkCNC.Services;

public interface IStatusService
{
    public ObservableCollection<Status> CurrentStatuses { get; }

    public ObservableCollection<Status> History { get; }

    public void AddStatus(Status status);

    public void RemoveStatus(Status status);

    public void RemoveStatusThatsContains(Status status);
}
