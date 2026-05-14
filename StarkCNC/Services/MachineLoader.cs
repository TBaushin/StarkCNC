using Microsoft.Extensions.Configuration;
using StarkCNC.Models;
using System.IO;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services;

public class MachineLoader : IMachineLoader
{
    private readonly IConfiguration _configuration;

    public Model3DGroup Group { get; } = new Model3DGroup();

    public ModelVisual3D Pipe { get; private set; } = new ModelVisual3D();

    public Model Carriage { get; }
    public Model Console { get; }
    public Model Bend { get; }
    public Model Clamp { get; }
    public Model Press { get; }
    public Model Roller { get; }

    public MachineLoader(IConfiguration configuration)
    {
        _configuration = configuration;
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
            .Build();

        Carriage = new Model(_configuration, nameof(Carriage));
        Console = new Model(_configuration, nameof(Console));
        Bend = new Model(_configuration, nameof(Bend), Console, -90);
        Clamp = new Model(_configuration, nameof(Clamp), Bend);
        Press = new Model(_configuration, nameof(Press), Console);
        Roller = new Model(_configuration, nameof(Roller), Bend);

        Bend.Children.Add(Clamp);
        Bend.Children.Add(Roller);
        Console.Children.Add(Bend);
        Console.Children.Add(Press);

        Group.Children.Add(Carriage.Figure);
        Group.Children.Add(Console.Figure);
        Group.Children.Add(Bend.Figure);
        Group.Children.Add(Press.Figure);
        Group.Children.Add(Roller.Figure);
        Group.Children.Add(Clamp.Figure);
    }

    public void SetForAllModelsDefaultPositions()
    {
        Carriage.SetDefault();
        Console.SetDefault();
    }
}
