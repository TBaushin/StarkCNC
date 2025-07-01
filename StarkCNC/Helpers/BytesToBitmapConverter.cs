using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StarkCNC.Helpers
{
    internal class BytesToBitmapConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArr)
            {
                if (byteArr.Length == 0)
                    return null;

                var bitmap = new BitmapImage();
                using var ms = new MemoryStream(byteArr);
                ms.Position = 0;

                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = null;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                
                bitmap.Freeze();
                return bitmap;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
