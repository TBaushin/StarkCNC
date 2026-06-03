using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.SharpDX.Assimp;
using HelixToolkit.SharpDX.Model;
using HelixToolkit.SharpDX.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Extensions.Configuration;
using StarkCNC.Utilities;
using System.Windows.Media.Media3D;

namespace StarkCNC.Models;

/// <summary>
/// Класс 3D модели
/// </summary>
public class Model
{
    private readonly IConfiguration _configuration;
    private bool _renderEnvironmentMap;
    private readonly double _defaultAroundTransformAngle;
    private double _currentAroundTransformAngle;

    public string Name { get; private set; }

    public Model? AroundTransform { get; private set; }

    public ICollection<Model> Children { get; } = new List<Model>();

    public Coordinate Coordinates { get; } = new Coordinate();

    public Vector3D Axis { get; private set; }

    public HelixToolkitScene? Figure { get; private set; }

    public Model(
        IConfiguration configuration,
        string name,
        IEffectsManager effectsManager,
        bool renderEnvironmentMap,
        Model? aroundTransform = null,
        double defaultAroundTransformAngle = 0)
    {
        _configuration = configuration;
        _defaultAroundTransformAngle = defaultAroundTransformAngle;
        _renderEnvironmentMap = renderEnvironmentMap;
        Name = name;
        AroundTransform = aroundTransform;

        var path = _configuration.GetValue<string>($"Settings:Models:{Name}:Path") ?? string.Empty;
        using var loader = new Importer();
        Figure = loader.Load(path);
        
        if (Figure is not null)
        {
            Figure.Root.Attach(effectsManager);
            Figure.Root.UpdateAllTransformMatrix();

            SetMaterials();
            SetDefault();
        }
    }

    public void SetDefault()
    {
        var positions = _configuration.GetSection($"Settings:Models:{Name}:Position").Get<double[]>() ?? new double[] { 0, 0, 0 };
        var rotation = _configuration.GetSection($"Settings:Models:{Name}:Rotation").Get<double[]>() ?? new double[] { 0, 0, 0 };
        var axis = _configuration.GetSection($"Settings:Models:{Name}:Axis").Get<double[]>() ?? new double[] { 0, 0, 0 };

        Coordinates.PositionX = positions[0];
        Coordinates.PositionY = positions[1];
        Coordinates.PositionZ = positions[2];
        Coordinates.RotationX = rotation[0];
        Coordinates.RotationY = rotation[1];
        Coordinates.RotationZ = rotation[2];
        Axis = new Vector3D(axis[0], axis[1], axis[2]);

        UpdateTransform(_defaultAroundTransformAngle);

        foreach (var item in Children)
        {
            item.SetDefault();
        }
    }

    public void UpdateTransform(double? aroundTransformAngle = null)
    {
        if (aroundTransformAngle is null)
            aroundTransformAngle = _currentAroundTransformAngle;
        else
            _currentAroundTransformAngle = (double)aroundTransformAngle;

        var transformBuilder = new TransformGroupBuilder()
            .CalculateTransform(Coordinates.PositionX, Coordinates.PositionY, Coordinates.PositionZ)
            .CalculateRotation(Coordinates.RotationX, Coordinates.RotationY, Coordinates.RotationZ, Axis, aroundTransformAngle.Value);

        if (AroundTransform is not null && AroundTransform.Figure is not null)
        {
            AroundTransform.Figure.Root.UpdateAllTransformMatrix();

            transformBuilder.SetObjectTransformAround(
                TransformGroupBuilder.ToWpfTransform(AroundTransform.Figure.Root.Items[0].TotalModelMatrix));
        }

        if (Figure is not null)
        {
            Figure.Root.Items[0].ModelMatrix = transformBuilder.Build().ToMatrix();
            Figure.Root.UpdateAllTransformMatrix();
        }

        foreach (var item in Children)
        {
            item.UpdateTransform();
        }
    }

    private void SetMaterials()
    {
        if (Figure is null || Figure.Root is null)
            return;

        foreach (var node in Figure.Root.Traverse())
        {
            if (node is MaterialGeometryNode mgn)
            {
                mgn.Material = new PhongMaterialCore()
                {
                    AmbientColor = Color4.Black,
                    DiffuseColor = new Color4(0.5f, 0.5f, 0.5f, 1f),
                    EmissiveColor = Color4.Black,
                    SpecularColor = new Color4(0.5f, 0.5f, 0.5f, 1f),
                    SpecularShininess = 0f,
                    RenderEnvironmentMap = _renderEnvironmentMap
                };
            }

            if (node is MeshNode mn)
            {
                mn.Material = new PhongMaterialCore()
                {
                    AmbientColor = Color4.Black,
                    DiffuseColor = new Color4(0.5f, 0.5f, 0.5f, 1f),
                    EmissiveColor = Color4.Black,
                    SpecularColor = new Color4(0.5f, 0.5f, 0.5f, 1f),
                    SpecularShininess = 0f,
                    RenderEnvironmentMap = _renderEnvironmentMap
                };
            }
        }
    }
}