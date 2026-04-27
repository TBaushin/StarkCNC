using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services;

public class Route
{
    public string Title { get; }

    public string Path { get; }

    public Type ViewModelType { get; }

    public Type? ViewType { get; set; }

    public string IconGlyph { get; }

    public Roles[] RolesHasAccess { get; }

    public Route(string path, Type type, string title, string iconGlyph, Roles[] rolesHasAccess, Type? viewType = null)
    {
        Path = path;
        ViewModelType = type;
        Title = title;
        IconGlyph = iconGlyph;
        RolesHasAccess = rolesHasAccess;
        ViewType = viewType;
    }
}
