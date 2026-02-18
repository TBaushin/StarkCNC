namespace StarkCNC.Core.Services;

public interface IRouter
{
    Route? CurrentRoute { get; }

    bool CanGoBack { get; }

    void ConfigureRoutes(Action<IRouteBuilder> configure);

    object? Navigate(string path, params object[] parameters);

    object? GoBack();

    Type? ResolveType(string path);

    IEnumerable<KeyValuePair<string, Route>> GetRoutes();

    Route? GetRoute(string path);
}
