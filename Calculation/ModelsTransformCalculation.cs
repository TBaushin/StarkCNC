using System.Windows.Media.Media3D;

namespace Calculation
{
    public class ModelsTransformCalculation
    {
        private Transform3DGroup _transformGroup = new Transform3DGroup();

        public ModelsTransformCalculation CalculateTransform(double angleX, double angleY, double angleZ)
        {
            TranslateTransform3D translateTransform = new TranslateTransform3D(angleX, angleY, angleZ);
            _transformGroup.Children.Add(translateTransform);
            return this;
        }

        public ModelsTransformCalculation CalculateRotation(double rotationX, double rotationY, double rotationZ, Vector3D axis, double angle)
        {
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(axis, angle);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation, rotationX, rotationY, rotationZ);
            _transformGroup.Children.Add(rotateTransform);
            return this;
        }

        public ModelsTransformCalculation SetObjectTransformAround(Transform3D transformGroup)
        {
            _transformGroup.Children.Add(transformGroup);
            return this;
        }

        public Transform3DGroup GetResult()
        {
            return _transformGroup;
        }
    }
}
