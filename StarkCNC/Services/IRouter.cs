namespace StarkCNC.Services;

public interface IRouter
{
    string CurrentRoute { get; }

    void ConfigureRoutes(Action<IRouteBuilder> configure);

    void Navigate(string route);

    Type? ResolveType(string route);
}
