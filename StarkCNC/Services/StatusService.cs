using StarkCNC.Core.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace StarkCNC.Core.Services;

public class StatusService : IStatusService
{
    private Status? _status;

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
                OnCollectionChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public event NotifyCollectionChangedEventHandler? NotifyCollectionChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnCollectionChanged()
    {
        NotifyCollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
    }
}