using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Models
{
    public class AdjustmentParameters : INotifyPropertyChanged
    {
        private readonly StarkCNC.Core.Models.AdjustmentParameters _adjustment;

        public string Name
        {
            get => _adjustment.Name;
            set
            {
                _adjustment.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public double PipeDiameter
        {
            get => _adjustment.PipeDiameter;
            set
            {
                _adjustment.PipeDiameter = value;
                OnPropertyChanged(nameof(PipeDiameter));
            }
        }

        public AdjustmentType Type
        {
            get => _adjustment.Type;
            set
            {
                _adjustment.Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public AdjustmentParameters(StarkCNC.Core.Models.AdjustmentParameters adjustment)
        {
            _adjustment = adjustment;
        }

        public AdjustmentParameters(string name, AdjustmentType type)
        {
            _adjustment = new StarkCNC.Core.Models.AdjustmentParameters(name, type);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StarkCNC.Core.Models.AdjustmentParameters Cast() => _adjustment;
    }
}
