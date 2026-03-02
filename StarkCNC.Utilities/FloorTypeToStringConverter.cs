using StarkCNC.Core.Models;
using System.Globalization;
using System.Windows.Data;

namespace StarkCNC.Utilities;

public class FloorTypeToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not FloorType type)
            return string.Empty;

        if (type == FloorType.SingleLevel)
            return "Одноуровневый"; // TODO: Локализовать

        if (type == FloorType.TwoLevel)
            return "Двухуровневый"; // TODO: Локализовать

        if (type == FloorType.ThreeLevel)
            return "Трёхуровневый"; // TODO: Локализовать

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text)
            return FloorType.SingleLevel;

        if (text == "Одноуровневый")
            return FloorType.SingleLevel;

        if (text == "Двухуровневый")
            return FloorType.TwoLevel;

        if (text == "Трёхуровневый")
            return FloorType.ThreeLevel;

        return FloorType.SingleLevel;
    }
}