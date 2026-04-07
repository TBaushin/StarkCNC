using StarkCNC.Core.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public class StatusService : IStatusService
{
    private Status? _status;

    private bool _showStatus = true;

    public List<Status> History { get; } = new List<Status>();

    public Status? CurrentStatus
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(CurrentStatus));

            if (value is not null)
            {
                History.Add(value);
                OnCollectionChanged(value);
            }
        }
    }

    public bool ShowStatus
    {
        get => _showStatus;
        set
        {
            _showStatus = value;
            OnPropertyChanged(nameof(ShowStatus));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public event NotifyCollectionChangedEventHandler? NotifyCollectionChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnCollectionChanged(Status newItem)
    {
        NotifyCollectionChanged?.Invoke(History, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem));
    }
}