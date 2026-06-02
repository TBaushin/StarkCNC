using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StarkCNC.Core.Models;
using StarkCNC.Models;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services;

public interface IMachineLoader
{
    SceneNodeGroupModel3D Group { get; }

    ModelVisual3D Pipe { get; }

    Model Carriage { get; }
    Model Console { get; }
    Model Bend { get; }
    Model Clamp { get; }
    Model Press { get; }
    Model Roller { get; }

    void SetForAllModelsDefaultPositions();

    public void Load(IEffectsManager effectsManager, bool renderEnvironmentMap);

    static Dictionary<string, double> GetDefault()
    {
        var posDefault = new Dictionary<string, double>();
        posDefault.Add("console", 90);
        posDefault.Add("height", 25);
        posDefault.Add("bend", 90);
        posDefault.Add("carriage", 1000);
        posDefault.Add("clamp", 0);
        posDefault.Add("press", 0);
        return posDefault;
    }
}
