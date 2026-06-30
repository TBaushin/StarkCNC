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
    private double? _dynamicAngle;

    public string Name { get; private set; }

    public Model? AroundTransform { get; private set; }

    public ICollection<Model> Children { get; } = new List<Model>();

    public Coordinate Coordinates { get; } = new Coordinate();

    public ICollection<Rotation> Rotations { get; } = new List<Rotation>();

    public HelixToolkitScene? Figure { get; private set; }

    public Model(
        IConfiguration configuration,
        string name,
        IEffectsManager effectsManager,
        bool renderEnvironmentMap,
        Model? aroundTransform = null)
    {
        _configuration = configuration;
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

        Coordinates.PositionX = positions[0];
        Coordinates.PositionY = positions[1];
        Coordinates.PositionZ = positions[2];
        Coordinates.RotationX = rotation[0];
        Coordinates.RotationY = rotation[1];
        Coordinates.RotationZ = rotation[2];

        LoadRotations();
        UpdateTransform();

        foreach (var item in Children)
        {
            item.SetDefault();
        }
    }

    public void UpdateTransform(double? dynamicAngle = null)
    {
        if (dynamicAngle is not null)
            _dynamicAngle = dynamicAngle;

        var transformBuilder = new TransformGroupBuilder()
            .CalculateTransform(Coordinates.PositionX, Coordinates.PositionY, Coordinates.PositionZ);

        var rotations = Rotations.ToList();
        for (var i = 0; i < rotations.Count; i++)
        {
            var axis = rotations[i].Axis;
            var angle = rotations[i].Angle;
            if (axis.LengthSquared == 0)
                continue;

            var isLast = i == rotations.Count - 1;
            var appliedAngle = _dynamicAngle is not null && isLast ? _dynamicAngle.Value : angle;

            transformBuilder
                .CalculateRotation(Coordinates.RotationX, Coordinates.RotationY, Coordinates.RotationZ, axis, appliedAngle);
        }

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

    private void LoadRotations()
    {
        Rotations.Clear();

        foreach (var item in _configuration.GetSection($"Settings:Models:{Name}:Rotations").GetChildren())
        {
            var axis = item.GetSection("Axis").Get<double[]>() ?? [0, 0, 0];
            var angle = item.GetValue("Angle", 0.0);
            Rotations.Add(new Rotation() { Axis = new Vector3D(axis[0], axis[1], axis[2]), Angle = angle });
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