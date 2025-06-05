using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        public ObservableCollection<BendingData> bendingDatas { get; set; } = new ObservableCollection<BendingData>();

        private IBendingModelsLoadingService _bendingModelsLoadingService;

        public ProgramViewModel(IBendingModelsLoadingService bendingModelsLoadingService)
        {
            _bendingModelsLoadingService = bendingModelsLoadingService;
            _bendingModelsLoadingService.Load();
        }

        public ModelVisual3D GetModels()
        {
            return _bendingModelsLoadingService.GetModelVisual3D();
        }
    }
}
