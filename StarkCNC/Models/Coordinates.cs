namespace StarkCNC.Models
{
    public class Coordinates
    {
        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public double PositionZ { get; set; }

        public double RotationX { get; set; }

        public double RotationY { get; set; }

        public double RotationZ { get; set; }

        public void SetPosition(double x, double y, double z)
        {
            PositionX = x;
            PositionY = y;
            PositionZ = z;
        }

        public void SetRotation(double x, double y, double z)
        {
            RotationX = x;
            RotationY = y;
            RotationZ = z;
        }
    }
}
