using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Services;

public partial class BreadcrumbService : ObservableObject, IBreadcrumbService
{
    private readonly IRouter _router;
    private readonly INavigationService _navigationService;

    private readonly List<Page> _breadcrumbs = new();

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
        _breadcrumbs.Clear();

        var segments = _router.CurrentRoute.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var current = "";
        foreach (var segment in segments)
        {
            current += "/" + segment;

            var type = _router.ResolveType(current);
            if (type is null)
                continue;

            var page = ViewLocator.Build(type);
            if (page is null)
                continue;

            _breadcrumbs.Add(page);
        }

        GenerateVisibleObject();
    }

    private void GenerateVisibleObject()
    {
        var sp = new StackPanel { Orientation = Orientation.Horizontal };

        for (int i = 0; i < _breadcrumbs.Count; i++)
        {
            var page = _breadcrumbs[i];
            var label = new Label() { Content = page.Title, Cursor = Cursors.Hand };
            label.PreviewMouseLeftButtonDown += (s, e) =>
            {
                var segments = _router.CurrentRoute.Split('/', StringSplitOptions.RemoveEmptyEntries);
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
                    if (pg.Title == page.Title)
                        break;
                }

                _router.Navigate(targetRoute);
            };

            sp.Children.Add(label);

            if (i < _breadcrumbs.Count - 1)
                sp.Children.Add(new Label() { Content = "&#xE76C;", FontFamily =  App.Current.TryFindResource("SymbolThemeFontFamily") as FontFamily });
        }

        VisibleObject = sp;
    }
}
