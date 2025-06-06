using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Services
{
    public interface IBendingModelsLoadingService
    {
        Model3DGroup? Bend { get; }

        Model3DGroup? Carriage { get; }

        Model3DGroup? Clamp { get; }

        Model3DGroup? Console { get; }

        Model3DGroup? Press { get; }

        void Load(string path);

        void Load(string path, ModelType type);

        ModelVisual3D GetModelVisual3D();
    }
}
