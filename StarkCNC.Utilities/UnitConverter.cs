using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace StarkCNC.Utilities;

public class UnitConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"{value} {parameter}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            var reg = new Regex(@"\s{0,}\D+(\.){0,1}\D+(\.){0,}");
            string resultString = reg.Replace(s, "");
            return targetType switch
            {
                var t when t == typeof(int) => int.TryParse(resultString, culture, out var result) ? result : value,
                var t when t == typeof(float) => float.TryParse(resultString, culture, out var result) ? result : value,
                var t when t == typeof(double) => double.TryParse(resultString, culture, out var result) ? result : value,
                _ => value
            };
        }

        return value;
    }
}
