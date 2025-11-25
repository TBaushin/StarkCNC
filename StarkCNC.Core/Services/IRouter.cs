namespace StarkCNC.Core.Services;

public interface IRouter
{
    string CurrentRoute { get; }

    void ConfigureRoutes(Action<IRouteBuilder> configure);

    object? Navigate(string path, params object[] parameters);

    Type? ResolveType(string path);

    IEnumerable<KeyValuePair<string, Route>> GetRoutes();
}
