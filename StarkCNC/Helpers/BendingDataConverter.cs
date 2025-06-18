using Calculation.Models;
using StarkCNC.Models;
using System.Globalization;
using System.Windows.Data;

namespace StarkCNC.Helpers
{
    internal class BendingDataConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BendingData data && targetType == typeof(BendingParameters))
            {
                return new BendingParameters()
                {
                    StraightLength = data.StraightLength,
                    BendingAngle = data.BendingAngle,
                    BendingRadius = data.BendingRadius,
                    RotationAngle = data.RotationAngle,
                };
            }

            if (value is BendingData bendingData && targetType == typeof(StarkCNC.Core.Models.BendingData))
            {
                return new StarkCNC.Core.Models.BendingData()
                {
                    StraightLength = bendingData.StraightLength,
                    BendingAngle = bendingData.BendingAngle,
                    BendingRadius = bendingData.BendingRadius,
                    RotationAngle = bendingData.RotationAngle
                };
            }

            if (value is BendingParameters parameters && targetType == typeof(BendingData))
            {
                return new BendingData()
                {
                    StraightLength = parameters.StraightLength,
                    BendingAngle = parameters.BendingAngle,
                    BendingRadius = parameters.BendingRadius,
                    RotationAngle = parameters.RotationAngle,
                };
            }

            if (value is StarkCNC.Core.Models.BendingData bdata && targetType == typeof(BendingData))
            {
                return new BendingData()
                {
                    StraightLength = bdata.StraightLength,
                    BendingAngle = bdata.BendingAngle,
                    BendingRadius = bdata.BendingRadius,
                    RotationAngle = bdata.RotationAngle
                };
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
