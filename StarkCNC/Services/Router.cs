using StarkCNC.Core.Services;

namespace StarkCNC.Services;

public class Router : IRouter, IRouteBuilder
{
    private readonly INavigationService _navigationService;
    private readonly Dictionary<string, Route> _routes = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<KeyValuePair<Route?, object[]?>> _history = new Stack<KeyValuePair<Route?, object[]?>>();

    private KeyValuePair<Route?, object[]?> _currentRoute;

    public Route? CurrentRoute => _currentRoute.Key;

    public bool CanGoBack => _history.Any();

    public Router(INavigationService navigationService)
    {
        _navigationService = navigationService;

        //_navigationService.Navigation += _navigationService_Navigation;
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

        if (CurrentRoute is not null)
            _history.Push(_currentRoute);

        _currentRoute = new KeyValuePair<Route?, object[]?>(route, parameters);
        _navigationService.Navigate(page);
        return page;
    }

    public object? GoBack()
    {
        if (_history.Count == 0)
            return null;

        var route = _history.Pop();
        if (route.Key is null)
            return null;

        var page = ViewLocator.Build(route.Key.Type, route.Value);
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
}
