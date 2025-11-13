namespace StarkCNC.Services;

public class Router : IRouter, IRouteBuilder
{
    private readonly INavigationService _navigationService;
    private readonly Dictionary<string, Type> _routes = new(StringComparer.OrdinalIgnoreCase);

    private string _currentRoute = string.Empty;

    public string CurrentRoute => _currentRoute;

    public Router(INavigationService navigationService)
    {
        _navigationService = navigationService;

        _navigationService.Navigation += _navigationService_Navigation;
    }

    public void AddRoute(string route, Type type)
    {
        if (!_routes.ContainsKey(route))
            _routes.Add(route, type);
    }

    public void ConfigureRoutes(Action<IRouteBuilder> configure)
    {
        if (configure is null)
            return;

        configure(this);
    }

    public void Navigate(string route)
    {
        if(!_routes.TryGetValue(route, out var type))
            throw new InvalidOperationException($"Route '{route}' is not configured.");

        var page = ViewLocator.Build(type);
        if (page is null)
            return;

        _currentRoute = route;
        _navigationService.Navigate(page);
    }

    public Type? ResolveType(string route)
    {
        if (_routes.TryGetValue(route, out var type))
            return type;
        return null;
    }

    private void _navigationService_Navigation(object? sender, NavigationEventArgs e)
    {
        var pageType = e.Page?.GetType();
        if (pageType is null)
            return;

        foreach (var kv in _routes)
        {
            if (kv.Value.Name == pageType.Name + "Model")
            {
                _currentRoute = kv.Key;
                break;
            }
        }
    }
}
