namespace Calculation.Models
{
    public class BendingParameters
    {
        /// <summary>
        /// Длина прямого участка
        /// </summary>
        public double StraightLength { get; set; }

        /// <summary>
        /// Угол гиба
        /// </summary>
        public double BendingAngle { get; set; }

        /// <summary>
        /// Радиус гиба
        /// </summary>
        public double BendingRadius { get; set; }

        /// <summary>
        /// Угол поворота
        /// </summary>
        public double RotationAngle { get; set; }
    }
}
