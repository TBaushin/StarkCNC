using System.Globalization;
using System.Windows.Data;

namespace StarkCNC.Helpers
{
    public class PositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StarkCNC.Core.Models.BendPositions calcPosition && targetType == typeof(StarkCNC._3DViewer.Models.BendPositions))
            {
                return new StarkCNC._3DViewer.Models.BendPositions()
                {
                    StartPosition = calcPosition.StartPosition,
                    EndPosition = calcPosition.EndPosition,
                };
            }

            if (value is StarkCNC._3DViewer.Models.BendPositions viewerPositios && targetType == typeof(StarkCNC.Core.Models.BendPositions))
            {
                return new StarkCNC.Core.Models.BendPositions()
                {
                    StartPosition = viewerPositios.StartPosition,
                    EndPosition = viewerPositios.EndPosition,
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
