using System.ComponentModel;

namespace StarkCNC.Core.Services;

public interface IStatusService : INotifyPropertyChanged
{
    string Status { get; set; }

    new event PropertyChangedEventHandler? PropertyChanged;
}
