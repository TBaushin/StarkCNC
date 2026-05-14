using HelixToolkit.Wpf;
using Microsoft.Extensions.Configuration;
using StarkCNC.Utilities;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC.Models;

/// <summary>
/// Класс 3D модели
/// </summary>
public class Model
{
    private readonly IConfiguration _configuration;
    private readonly double _defaultAroundTransformAngle;
    private double _currentAroundTransformAngle;

    public string Name { get; private set; }

    public Model? AroundTransform { get; private set; }

    public ICollection<Model> Children { get; set; } = new List<Model>();

    public Coordinate Coordinates { get; } = new Coordinate();

    public Vector3D Axis { get; private set; }

    public Model3DGroup? Figure { get; private set; }

    public Model(IConfiguration configuration, string name, Model? aroundTransform = null, double defaultAroundTransformAngle = 0)
    {
        _configuration = configuration;
        _defaultAroundTransformAngle = defaultAroundTransformAngle;
        Name = name;
        AroundTransform = aroundTransform;

        var path = _configuration.GetValue<string>($"Settings:Models:{Name}:Path") ?? string.Empty;
        Figure = new ModelImporter().Load(path);
        SetMaterials();
        SetDefault();
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
            transformBuilder.SetObjectTransformAround(AroundTransform.Figure.Transform);

        if (Figure is not null)
            Figure.Transform = transformBuilder.Build();

        foreach (var item in Children)
        {
            item.UpdateTransform();
        }
    }

    private void SetMaterials()
    {
        if (Figure is null)
            return;

        var geometry = Figure.Children[0] as GeometryModel3D;
        if (geometry is null)
            return;

        var materialGroup = new MaterialGroup();
        materialGroup.Children.Add(new EmissiveMaterial(new SolidColorBrush(Colors.White)));
        materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.Gray)));
        materialGroup.Children.Add(new SpecularMaterial(new SolidColorBrush(Colors.Blue), 200));

        geometry.Material = materialGroup;
        geometry.BackMaterial = materialGroup;
    }
}