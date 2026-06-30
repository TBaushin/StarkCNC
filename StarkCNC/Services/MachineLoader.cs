using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Extensions.Configuration;
using StarkCNC.Models;
using System.IO;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services;

public class MachineLoader : IMachineLoader
{
    private readonly IConfiguration _configuration;

    public SceneNodeGroupModel3D Group { get; } = new SceneNodeGroupModel3D();

    public ModelVisual3D Pipe { get; private set; } = new ModelVisual3D();

    public Model Carriage { get; private set; }
    public Model Console { get; private set; }
    public Model Bend { get; private set; }
    public Model Clamp { get; private set; }
    public Model Press { get; private set; }
    public Model Roller { get; private set; }

    public MachineLoader(IConfiguration configuration)
    {
        _configuration = configuration;
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public void Load(IEffectsManager effectsManager, bool renderEnvironmentMap)
    {
        Carriage = new Model(_configuration, nameof(Carriage), effectsManager, renderEnvironmentMap);
        Console = new Model(_configuration, nameof(Console), effectsManager, renderEnvironmentMap);
        Bend = new Model(_configuration, nameof(Bend), effectsManager, renderEnvironmentMap, Console);
        Clamp = new Model(_configuration, nameof(Clamp), effectsManager, renderEnvironmentMap, Bend);
        Press = new Model(_configuration, nameof(Press), effectsManager, renderEnvironmentMap, Console);
        Roller = new Model(_configuration, nameof(Roller), effectsManager, renderEnvironmentMap, Bend);

        Bend.Children.Add(Clamp);
        Bend.Children.Add(Roller);
        Console.Children.Add(Bend);
        Console.Children.Add(Press);

        AddNode(Carriage);
        AddNode(Console);
        AddNode(Bend);
        AddNode(Press);
        AddNode(Roller);
        AddNode(Clamp);
    }

    public void SetForAllModelsDefaultPositions()
    {
        Carriage.SetDefault();
        Console.SetDefault();
    }

    private void AddNode(Model model)
    {
        if (model.Figure is not null)
            Group.AddNode(model.Figure.Root);
    }
}
