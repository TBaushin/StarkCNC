using System.Windows.Media.Media3D;

namespace StarkCNC.Core.Calculations
{
    public interface IModelsTransformCalculation
    {
        public IModelsTransformCalculation CalculateTransform(double angleX, double angleY, double angleZ);

        public IModelsTransformCalculation CalculateRotation(double rotationX, double rotationY, double rotationZ, Vector3D axis, double angle);

        public IModelsTransformCalculation SetObjectTransformAround(Transform3D transformGroup);

        public Transform3DGroup GetResult();
    }
}
