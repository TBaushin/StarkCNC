using StarkCNC.Core.Models;
using System.Windows.Media.Media3D;

namespace StarkCNC.Core.Calculations
{
    public class WireSegment
    {
        public double Diameter; // Диаметр провода
        public double BendingRadius; // Радиус гиба
        public double StraightLength; // Длина прямого участка между гибами
        public double BendingAngle; // Угол гиба в градусах
        public double RotationAngle; // Проворот гиба относительно оси, градусов
    }

    public static class QuaternionExtensions
    {
        public static Vector3D Rotate(this Quaternion q, Vector3D v)
        {
            var m = new Matrix3D();
            m.Rotate(q);
            return m.Transform(v);
        }
    }

    public static class WireBuilder
    {
        // Возвращает список 3D-точек маршрута провода по списку сегментов
        public static ICollection<Point3D> BuildWirePath(ICollection<BendingData> bendingDatas, double diameter, int bendSteps = 16)
        {
            List<BendingData> segments = new List<BendingData>();
            foreach (var data in bendingDatas)
            {
                segments.Add(data.Copy());
            }

            var points = new List<Point3D>();
            var currentPoint = new Point3D(0, 0, 0);
            var currentDirection = new Vector3D(1, 0, 0); // Стартовое направление по X
            var currentUp = new Vector3D(0, 0, 1);        // Стартовое «вверх»
            points.Add(currentPoint);

            foreach (var seg in segments)
            {
                // Прямой участок
                if (seg.StraightLength > 0)
                {
                    currentPoint += currentDirection * seg.StraightLength;
                    points.Add(currentPoint);
                }

                if (Math.Abs(seg.BendingAngle) < 1e-6) continue; // Без дуги — следующий сегмент

                if (seg.BendingRadius == 0 && seg.BendingAngle > 0)
                    seg.BendingRadius = diameter;

                // Проворот up-вектора
                if (Math.Abs(seg.RotationAngle) > 1e-6)
                {
                    var rotationQuat = new Quaternion(currentDirection, seg.RotationAngle);
                    currentUp = rotationQuat.Rotate(currentUp);
                }

                // Ось напряжения дуги
                var bendAxis = currentUp;

                // Нормаль радиуса — из продукта векторного произведения
                var radiusDir = Vector3D.CrossProduct(bendAxis, currentDirection);
                radiusDir.Normalize();

                // Центр дуги
                var bendCenter = currentPoint + radiusDir * seg.BendingRadius;

                double bendRad = seg.BendingAngle * Math.PI / 180.0;
                for (int i = 1; i <= bendSteps; i++)
                {
                    double angle = i * bendRad / bendSteps;
                    var quat = new Quaternion(bendAxis, angle * 180.0 / Math.PI);
                    Vector3D offset = quat.Rotate(-radiusDir * seg.BendingRadius);
                    Point3D pt = bendCenter + offset;
                    points.Add(pt);
                }

                // Обновить направление для следующего отрезка
                var finalQuat = new Quaternion(bendAxis, seg.BendingAngle);
                currentDirection = finalQuat.Rotate(currentDirection);
                currentDirection.Normalize();

                // Новый старт для следующего участка
                currentPoint = points[points.Count - 1];
            }

            return points;
        }
    }
    
    public class BendCalculation : IBendCalculation
    {
        private readonly int _slices = 100;

        public BendingData BendParameters { get; set; }

        public BendCalculation(BendingData bendParameters)
        {
            BendParameters = bendParameters;
        }

        public ICollection<BendPositions> CalculateBend(double pipeDiameter, double carriagePos)
        {
            var data = new List<BendPositions>();

            double bendAngleStart = 0;
            double bendAngleEnd = BendParameters.BendingAngle / 57.3;

            for (int i = 0; i < _slices; i++)
            {
                var bend = new BendPositions();

                double firstVector = Convert.ToDouble(i) / Convert.ToDouble(_slices);
                double secondVector = Convert.ToDouble(i + 1) / Convert.ToDouble(_slices);

                double firstAngle = bendAngleEnd + (bendAngleStart - bendAngleEnd) * firstVector;
                double secondAngle = bendAngleEnd + (bendAngleStart - bendAngleEnd) * secondVector;

                for (int j = 0; j <= 1; j++)
                {
                    double circumferenceLength = 2 * Math.PI * j / 180;
                    var x = Convert.ToSingle(pipeDiameter * Math.Cos(circumferenceLength) + BendParameters.BendingRadius);
                    var z = Convert.ToSingle(pipeDiameter * Math.Sin(circumferenceLength));

                    var xb = Convert.ToSingle(Math.Sin(firstAngle) * x + carriagePos + BendParameters.StraightLength);
                    var yb = Convert.ToSingle(Math.Cos(firstAngle) * x - BendParameters.BendingRadius);
                    var zb = z;
                    bend.StartPosition = new Point3D(xb, yb, zb);

                    xb = Convert.ToSingle(Math.Sin(secondAngle) * x + carriagePos + BendParameters.StraightLength);
                    yb = Convert.ToSingle(Math.Cos(secondAngle) * x - BendParameters.BendingRadius);
                    zb = z;
                    bend.EndPosition = new Point3D(xb, yb, zb);

                    data.Add(bend);
                }
            }

            return data;
        }
    }
}