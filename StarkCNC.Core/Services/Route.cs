namespace StarkCNC.Core.Services;

public class Route
{
    public string Title { get; }

    public string Path { get; }

    public Type Type { get; }

    public string IconGlyph { get;}

    public Route(string path, Type type, string title, string iconGlyph)
    {
        Path = path;
        Type = type;
        Title = title;
        IconGlyph = iconGlyph;
    }
}
