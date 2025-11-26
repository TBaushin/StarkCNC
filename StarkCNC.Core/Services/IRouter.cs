namespace StarkCNC.Core.Services;

public interface IRouter
{
    Route? CurrentRoute { get; }

    void ConfigureRoutes(Action<IRouteBuilder> configure);

    object? Navigate(string path, params object[] parameters);

    Type? ResolveType(string path);

    IEnumerable<KeyValuePair<string, Route>> GetRoutes();

    Route? GetRoute(string path);
}
