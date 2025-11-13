using System.Windows.Controls;

namespace StarkCNC.Services;

public class NavigationEventArgs
{
    public string? Title { get; set; }
    public Page? Page { get; set; }

    public NavigationEventArgs() { }

    public NavigationEventArgs(Page? page)
    {
        Page = page;
        Title = page?.Title;
    }
}