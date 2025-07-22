using StarkCNC._3DViewer.Services;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.ViewModels
{
    public class ProgramControllerViewModel
    {
        private readonly IBendingModelsLoadingService _bendingModelsLoadingService;

        public ProgramControllerViewModel(IBendingModelsLoadingService bendingModelsLoadingService)
        {
            _bendingModelsLoadingService = bendingModelsLoadingService;
        }

        public ModelVisual3D GetPipe()
        {
            return _bendingModelsLoadingService.Pipe;
        }
    }
}
