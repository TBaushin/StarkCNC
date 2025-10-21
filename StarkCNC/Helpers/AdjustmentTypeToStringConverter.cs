using StarkCNC.Core.Models;
using System.Globalization;
using System.Windows.Data;

namespace StarkCNC.Helpers;

internal class AdjustmentTypeToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not AdjustmentType type)
            return string.Empty;

        if (type == AdjustmentType.Winding)
            return "Намоткой"; // TODO: Локализовать

        if (type == AdjustmentType.Rolling)
            return "Прокатная"; // TODO: Локализовать

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text)
            return AdjustmentType.Winding;

        if (text == "Намоткой")
            return AdjustmentType.Winding;

        if (text == "Прокатная")
            return AdjustmentType.Rolling;

        return AdjustmentType.Winding;
    }
}