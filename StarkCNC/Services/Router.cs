using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Windows.Controls;

namespace StarkCNC.Services;

public class Router : IRouter, IRouteBuilder
{
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;
    private readonly Dictionary<string, Route> _routes = new(StringComparer.OrdinalIgnoreCase);

    private Route? _currentRoute;

    public Route? CurrentRoute => _currentRoute;

    public Router(INavigationService navigationService, IUserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;

        _navigationService.Navigation += _navigationService_Navigation;
    }

    public void AddRoute(
        string route,
        Type type,
        string? title = null,
        string? iconGlyph = null,
        Roles[]? rolesHasAccess = null)
    {
        if (rolesHasAccess is null)
            rolesHasAccess = new Roles[] { Roles.Service, Roles.Administrator, Roles.Operator, Roles.None };

        if (!_routes.ContainsKey(route))
            _routes.Add(
                route,
                new Route(
                    route,
                    type,
                    string.IsNullOrEmpty(title) ? string.Empty : title,
                    string.IsNullOrEmpty(iconGlyph) ? string.Empty : iconGlyph,
                    rolesHasAccess));
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

        Page? page = null;

        if (!HasAccessToPage(route))
            page = ViewLocator.NoAccess();
        else
            page = ViewLocator.Build(route.Type, parameters);

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

    //private bool HasAccessToPage(Route route) =>
    //    (_userService.CurrentUser is not null && !route.RolesHasAccess.Contains(GetRole(_userService.CurrentUserRole.Name))) ||
    //    (_userService.CurrentUser is null && !route.RolesHasAccess.Contains(Roles.None));

    private bool HasAccessToPage(Route route)
    {
        if (_userService.CurrentUser is not null && !route.RolesHasAccess.Contains(GetRole(_userService.CurrentUserRole.Name)))
            return false;

        if (_userService.CurrentUser is null && !route.RolesHasAccess.Contains(Roles.None))
            return false;

        return true;
    }

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

    private static Roles GetRole(string roleName) =>
        roleName.ToUpperInvariant() switch
        {
            "СЕРВИС" => Roles.Service,
            "АДМИНИСТРАТОР" => Roles.Administrator,
            "ОПЕРАТОР" => Roles.Operator,
            _ => Roles.Operator
        };
}
