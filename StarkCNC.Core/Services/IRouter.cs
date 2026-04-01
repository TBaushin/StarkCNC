namespace StarkCNC.Core.Services;

public interface IRouter
{
    bool CanGoBack { get; }

    Route? CurrentRoute { get; }

    event EventHandler<NavigationEventArgs>? Navigated;

    void ConfigureRoutes(Action<IRouteBuilder> configure);

    object? Navigate(string path, params object[] parameters);

    object? GoBack();

    Type? ResolveType(string path);

    IEnumerable<KeyValuePair<string, Route>> GetRoutes();

    Route? GetRoute(string path);

    void SetFrame(object frame);
}
