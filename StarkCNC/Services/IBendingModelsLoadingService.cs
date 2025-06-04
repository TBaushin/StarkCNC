using System.Windows.Media.Media3D;

namespace StarkCNC.Services
{
    public interface IBendingModelsLoadingService
    {
        void Load();

        void Load(string path);

        ModelVisual3D GetModelVisual3D();
    }
}
