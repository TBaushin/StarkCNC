using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Models;

namespace StarkCNC.ViewModels
{
    public partial class AdjustmentViewModel : ObservableObject
    {
        [ObservableProperty]
        private AdjustmentParameters _parameters;

        public AdjustmentViewModel() 
        {
            _parameters = new AdjustmentParameters();
        }
    }
}
