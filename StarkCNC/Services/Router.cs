using StarkCNC.Core.Services;

namespace StarkCNC.Services;

public class Router : IRouter, IRouteBuilder
{
    private readonly INavigationService _navigationService;
    private readonly Dictionary<string, Route> _routes = new(StringComparer.OrdinalIgnoreCase);

    private Route? _currentRoute;

    public Route? CurrentRoute => _currentRoute;

    public Router(INavigationService navigationService)
    {
        _navigationService = navigationService;

        _navigationService.Navigation += _navigationService_Navigation;
    }

    public void AddRoute(string route, Type type, string? title = null, string? iconGlyph = null)
    {
        if (!_routes.ContainsKey(route))
            _routes.Add(
                route,
                new Route(
                    route,
                    type,
                    string.IsNullOrEmpty(title) ? string.Empty : title,
                    string.IsNullOrEmpty(iconGlyph) ? string.Empty : iconGlyph));
    }

    public void ConfigureRoutes(Action<IRouteBuilder> configure)
    {
        if (configure is null)
            return;

        configure(this);
    }

    public object? Navigate(string path, params object[] parameters)
    {
        if(!_routes.TryGetValue(path, out var route))
            throw new InvalidOperationException($"Route '{path}' is not configured.");

        var page = ViewLocator.Build(route.Type, parameters);
        if (page is null)
            return null;

        _currentRoute = route;
        _navigationService.Navigate(page);
        return page;
    }

    public Type? ResolveType(string path) =>
        _routes.TryGetValue(path, out var route) ? route.Type : null;

    public IEnumerable<KeyValuePair<string, Route>> GetRoutes() => _routes;

    public Route? GetRoute(string path) =>
        _routes.TryGetValue(path, out var route) ? route : null;

    private void _navigationService_Navigation(object? sender, NavigationEventArgs e)
    {
        var pageType = e.Page?.GetType();
        if (pageType is null)
            return;

        foreach (var kv in _routes)
        {
            if (kv.Value.Type.Name == pageType.Name + "Model")
            {
                _currentRoute = kv.Value;
                break;
            }
        }
    }
}
