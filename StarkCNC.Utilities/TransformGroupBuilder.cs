using System.Windows.Media.Media3D;

namespace StarkCNC.Utilities;

public class TransformGroupBuilder
{
    public readonly Transform3DGroup _transformGroup = new Transform3DGroup();

    public TransformGroupBuilder CalculateTransform(double angleX, double angleY, double angleZ)
    {
        var translateTransform = new TranslateTransform3D(angleX, angleY, angleZ);
        _transformGroup.Children.Add(translateTransform);
        return this;
    }

    public TransformGroupBuilder CalculateRotation(double angleX, double angleY, double angleZ, Vector3D axis, double angle)
    {
        var axisAngleRotation = new AxisAngleRotation3D(axis, angle);
        var rotateTransform = new RotateTransform3D(axisAngleRotation, angleX, angleY, angleZ);
        _transformGroup.Children.Add(rotateTransform);
        return this;
    }

    public TransformGroupBuilder SetObjectTransformAround(Transform3D transformAround)
    {
        _transformGroup.Children.Add(transformAround);
        return this;
    }

    public Transform3DGroup Build() => _transformGroup;
}
