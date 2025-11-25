namespace StarkCNC.Core.Services;

public interface IRouteBuilder
{
    void AddRoute(string route, Type type, string? title = null, string? iconGlyph = null);
}
