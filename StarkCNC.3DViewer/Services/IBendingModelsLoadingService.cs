using StarkCNC._3DViewer.Models;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Services
{
    public interface IBendingModelsLoadingService
    {
        ModelVisual3D Pipe { get; }

        Model3DGroup? Bend { get; }

        Model3DGroup? Carriage { get; }

        Model3DGroup? Clamp { get; }

        Model3DGroup? Console { get; }

        Model3DGroup? Press { get; }

        void Load(string path);

        void Load(string path, ModelType type);

        ModelVisual3D GetModelVisual3D();

        void UpdatePipeBend(ICollection<BendPositions> positions);

        void UpdatePositions(double consolePosX, double bendRotationZ, double carriagePosY, double height, double clampPosX, double pressPosX);

        Dictionary<string, double> GetDefault();
    }
}
