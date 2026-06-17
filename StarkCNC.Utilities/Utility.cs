using System.Windows;
using System.Windows.Media;

namespace StarkCNC.Utilities;

public static class Utility
{
    public static bool IsWindows110rGreater()
    {
        var os = Environment.OSVersion;
        var version = os.Version;

        return version.Major >= 10 && version.Build >= 22000;
    }

    public static bool IsBackdropSupported()
    {
        var os = Environment.OSVersion;
        var version = os.Version;

        return version.Major >= 10 && version.Build >= 22621;
    }

    public static bool IsBackdropDisabled()
    {
        var appContextBackdropData = AppContext
            .GetData("Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop");
        bool disableFluentThemeWindowBackdrop = false;

        if (appContextBackdropData is not null && Convert.ToString(appContextBackdropData) is string stringAppContext)
            disableFluentThemeWindowBackdrop = bool.Parse(stringAppContext);

        return disableFluentThemeWindowBackdrop;
    }

    public static bool IsLightTheme(Application app)
    {
#pragma warning disable WPF0001
        try
        {
            var themeMode = Application.Current.ThemeMode;

            if (themeMode == ThemeMode.Light)
                return true;
            if (themeMode == ThemeMode.Dark)
                return false;

            // For System theme, detect the actual effective theme
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                var backgroundResource = mainWindow.TryFindResource("SolidBackgroundFillColorBaseBrush");
                if (backgroundResource is SolidColorBrush brush)
                {
                    var color = brush.Color;
                    var luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
                    return luminance > 0.5;
                }
            }

            return themeMode != ThemeMode.Dark;
        }
        catch
        {
            return true;
        }
#pragma warning restore WPF0001
    }
}