using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using System.Windows.Controls;

namespace StarkCNC.Services;

public class Router : IRouter, IRouteBuilder
{
    private readonly IUserService _userService;
    private readonly Dictionary<string, Route> _routes = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<KeyValuePair<Route, object[]>> _history = new Stack<KeyValuePair<Route, object[]>>();

    private Frame _frame;
    private Route? _currentRoute;
    private object[] _currentRouteParams;

    public bool CanGoBack => _history.Any();

    public Route? CurrentRoute => _currentRoute;

    public event EventHandler<NavigationEventArgs>? Navigated;

    public Router(IUserService userService)
    {
        _userService = userService;
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

        Page? page = GetPage(route, parameters);

        if (page is null)
            return null;

        if (_currentRoute is not null)
        {
            _history.Push(new KeyValuePair<Route, object[]>(_currentRoute, _currentRouteParams));
        }

        _currentRoute = route;
        _currentRouteParams = parameters;

        _frame.Navigate(page);
        Navigated?.Invoke(this, new NavigationEventArgs(page.Title, page));

        return page;
    }

    public object? GoBack()
    {
        if (!CanGoBack)
            return null;

        var previos = _history.Pop();

        var page = GetPage(previos.Key, previos.Value);
        if (page is null)
            return null;

        _currentRoute = previos.Key;
        _currentRouteParams = previos.Value;

        _frame.Navigate(page);
        Navigated?.Invoke(this, new NavigationEventArgs(page.Title, page));

        return page;
    }

    public Type? ResolveType(string path) =>
        _routes.TryGetValue(path, out var route) ? route.Type : null;

    public IEnumerable<KeyValuePair<string, Route>> GetRoutes() => _routes;

    public Route? GetRoute(string path) =>
        _routes.TryGetValue(path, out var route) ? route : null;

    public void SetFrame(object frame)
    {
        var fr = frame as Frame;
        if (fr is null)
            throw new InvalidOperationException("Не верный тип объекта");

        _frame = fr;
    }

    private Page? GetPage(Route route, params object[] parameters)
    {
        Page? page;
        if (!HasAccessToPage(route))
            page = ViewLocator.NoAccess();
        else
            page = ViewLocator.Build(route.Type, parameters);

        return page;
    }

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

    private static Roles GetRole(string roleName) =>
        roleName.ToUpperInvariant() switch
        {
            "СЕРВИС" => Roles.Service,
            "АДМИНИСТРАТОР" => Roles.Administrator,
            "ОПЕРАТОР" => Roles.Operator,
            _ => Roles.Operator
        };
}
