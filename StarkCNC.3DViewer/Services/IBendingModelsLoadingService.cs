using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Services
{
    public interface IBendingModelsLoadingService
    {
        GeometryModel3D Bend { get; }

        GeometryModel3D Carriage { get; }

        GeometryModel3D Clamp { get; }

        GeometryModel3D Console { get; }

        GeometryModel3D Press { get; }

        void Load(string path);

        void Load(string path, ModelType type);

        ModelVisual3D GetModelVisual3D();
    }
}
