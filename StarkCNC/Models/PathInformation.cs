using System.Windows.Input;

namespace StarkCNC.Models;

public class PathInformation
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Icon { get; set; }

    public ICommand Command { get; set; }

    public PathInformation(string name, string path, string icon, ICommand command)
    {
        Name = name;
        Path = path;
        Icon = icon;
        Command = command;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as PathInformation;
        if (other is null)
            return false;

        return other.Name == Name && other.Path == Path && other.Icon == Icon;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Path, Icon);
    }
}
