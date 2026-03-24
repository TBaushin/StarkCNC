using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Services;

public partial class BreadcrumbService : ObservableObject, IBreadcrumbService
{
    private readonly IRouter _router;
    private readonly INavigationService _navigationService;

    private readonly List<string> _breadcrumbsTitles = new();

    [ObservableProperty]
    private object _visibleObject;

    public BreadcrumbService(IRouter router, INavigationService navigationService)
    {
        _router = router;
        _navigationService = navigationService;

        _navigationService.Navigation += _navigationService_Navigation;
    }

    private void _navigationService_Navigation(object? sender, NavigationEventArgs e)
    {
        _breadcrumbsTitles.Clear();

        var segments = _router.CurrentRoute?.Path.Split('/', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        var current = "";
        var routes = _router.GetRoutes();
        foreach (var segment in segments)
        {
            current += $"/{segment}";

            var route = _router.GetRoute(current);
            if (route is null)
                continue;

            if (!string.IsNullOrEmpty(route.Title))
                _breadcrumbsTitles.Add(route.Title);
        }

        GenerateVisibleObject();
    }

    private void GenerateVisibleObject()
    {
        var sp = new StackPanel { Orientation = Orientation.Horizontal };

        for (int i = 0; i < _breadcrumbsTitles.Count; i++)
        {
            var pageTitle = _breadcrumbsTitles[i];
            var label = new TextBlock() { Text = pageTitle, Style = App.Current.TryFindResource("SubtitleTextBlockStyle") as Style, Cursor = Cursors.Hand };
            label.PreviewMouseLeftButtonDown += (s, e) =>
            {
                var segments = _router.CurrentRoute?.Path.Split('/', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                var targetRoute = "";

                foreach (var segment in segments)
                {
                    targetRoute += "/" + segment;
                    var type = _router.ResolveType(targetRoute);

                    if (type is null)
                        continue;
                    
                    var pg = ViewLocator.Build(type);
                    if (pg is null)
                        continue;
                    
                    if (pg.Title == pageTitle)
                        break;
                }

                _router.Navigate(targetRoute);
            };

            sp.Children.Add(label);

            if (i < _breadcrumbsTitles.Count - 1)
                sp.Children.Add(
                    new Label()
                    {
                        Content = "\xE76C",
                        FontFamily =  App.Current.TryFindResource("SymbolThemeFontFamily") as FontFamily,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5, 0, 5, 0)
                    });
        }

        VisibleObject = sp;
    }
}
