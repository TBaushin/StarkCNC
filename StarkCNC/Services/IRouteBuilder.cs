namespace StarkCNC.Services;

public interface IRouteBuilder
{
    void AddRoute(string route, Type type);
}
