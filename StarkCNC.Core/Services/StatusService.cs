using System.ComponentModel;

namespace StarkCNC.Core.Services;

public class StatusService : IStatusService
{
    private string _status = string.Empty;

    public string Status 
    { 
        get =>  _status; 
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
