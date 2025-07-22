using Microsoft.Extensions.Configuration;
using StarkCNC._3DViewer.Models;
using StarkCNC._3DViewer.Services;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.ViewModels
{
    public class VisualizationControllerViewModel
    {
        private IBendingModelsLoadingService _bendingModelsLoadingService;
        private IConfiguration _configuration;

        public VisualizationControllerViewModel(IBendingModelsLoadingService bendingModelsLoadingService, IConfiguration configuration)
        {
            _bendingModelsLoadingService = bendingModelsLoadingService;
            _configuration = configuration;

            LoadModels();
        }

        public ModelVisual3D GetModels()
        {
            return _bendingModelsLoadingService.GetModelVisual3D();
        }

        public ModelVisual3D GetPipe()
        {
            return _bendingModelsLoadingService.Pipe;
        }

        public void UpdatePositions(double consolePosX, double bendRotationX, double carriagePosY, double height, double clampPosX, double pressPosX)
        {
            _bendingModelsLoadingService.UpdatePositions(consolePosX, bendRotationX, carriagePosY, height, clampPosX, pressPosX);
        }

        public Dictionary<string, double> GetDefaults()
        {
            return _bendingModelsLoadingService.GetDefault();
        }

        private void LoadModels() 
        {
            ICollection<LoadingModel>? loadingModels = _configuration.GetSection("ModelsPath").Get<ICollection<LoadingModel>>();
            if (loadingModels is null)
                return;
            foreach (var item in loadingModels)
            {
                _bendingModelsLoadingService.Load(item.Path, item.Type);
            }
        }
    }
}
