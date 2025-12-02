using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StarkCNC.Helpers;

/// <summary>
/// Converts an bool to Visibility.Collapsed
/// </summary>
internal sealed class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b_value)
            return Visibility.Collapsed;

        if (parameter is string param && param.Equals("invert", StringComparison.OrdinalIgnoreCase))
            b_value = !b_value;

        return b_value == true ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility)
            return false;

        if (parameter is string param && param.Equals("invert", StringComparison.OrdinalIgnoreCase))
            visibility = visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

        return visibility == Visibility.Visible;
    }
}