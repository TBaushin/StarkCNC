using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public class Route
{
    public string Title { get; }

    public string Path { get; }

    public Type Type { get; }

    public string IconGlyph { get; }

    public Roles[] RolesHasAccess { get; }

    public Route(string path, Type type, string title, string iconGlyph, Roles[] rolesHasAccess)
    {
        Path = path;
        Type = type;
        Title = title;
        IconGlyph = iconGlyph;
        RolesHasAccess = rolesHasAccess;
    }
}
