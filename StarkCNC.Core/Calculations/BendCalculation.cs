using StarkCNC.Core.Models;
using System.Windows.Media.Media3D;

namespace StarkCNC.Core.Calculations
{
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

                    var xb = Convert.ToSingle(Math.Sin(firstAngle) * x + carriagePos + 2000);
                    var yb = Convert.ToSingle(Math.Cos(firstAngle) * x - BendParameters.BendingRadius);
                    var zb = z;
                    bend.StartPosition = new Point3D(xb, yb, zb);

                    xb = Convert.ToSingle(Math.Sin(secondAngle) * x + carriagePos + 2000);
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