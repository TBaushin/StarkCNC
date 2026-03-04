using StarkCNC.Core.Calculations;
using System.Windows.Media.Media3D;

namespace StarkCNC.Models;

/// <summary>
/// Класс 3D модели
/// </summary>
public class Model
{
    private Coordinates _defaultCoordinates = new Coordinates();

    public Model3DGroup Figure { get; set; } = new Model3DGroup();

    public Coordinates Coordinates { get; set; } = new Coordinates();

    public Vector3D Axis { get; set; }

    public ModelType Type { get; set; }

    public Model()
    {

    }

    public void UpdateTransform(double angle = 0, Model? arountTransform = null)
    {
        IModelsTransformCalculation calculations = new ModelsTransformCalculation()
            .CalculateTransform(Coordinates.PositionX, Coordinates.PositionY, Coordinates.PositionZ)
            .CalculateRotation(Coordinates.RotationX, Coordinates.RotationY, Coordinates.RotationZ, Axis, angle);

        if (arountTransform != null)
        {
            calculations.SetObjectTransformAround(arountTransform.Figure.Transform);
        }

        Figure.Transform = calculations.GetResult();
    }

    // TODO: Добавить чтение из Config, вынести настройки дефолтных позиций туда
}