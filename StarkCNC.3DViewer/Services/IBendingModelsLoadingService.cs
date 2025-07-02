using StarkCNC._3DViewer.Models;
using System.ComponentModel;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Services
{
    public interface IBendingModelsLoadingService : INotifyPropertyChanged
    {
        ModelVisual3D Pipe { get; }

        Model? Bend { get; }

        Model? Carriage { get; }

        Model? Clamp { get; }

        Model? Console { get; }

        Model? Press { get; }

        new event PropertyChangedEventHandler? PropertyChanged;

        void Load(string path);

        void Load(string path, ModelType type);

        ModelVisual3D GetModelVisual3D();

        void UpdatePipeBend(ICollection<BendPositions> positions);

        void UpdatePositions(double consolePosX, double bendRotationZ, double carriagePosY, double height, double clampPosX, double pressPosX);

        public Coordinates? GetModelPosition(ModelType modelType);

        Dictionary<string, double> GetDefault();
    }
}
