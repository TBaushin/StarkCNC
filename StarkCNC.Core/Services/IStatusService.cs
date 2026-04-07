using StarkCNC.Core.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public interface IStatusService : INotifyPropertyChanged
{
    List<Status> History { get; }

    Status? CurrentStatus { get; set; }

    bool ShowStatus { get; set; }

    new event PropertyChangedEventHandler? PropertyChanged;

    new event NotifyCollectionChangedEventHandler? NotifyCollectionChanged;
}