namespace StarkCNC.Services;

public class NavigationEventArgs
{
    public string? PageTitle { get; set; }

    public NavigationEventArgs() { }

    public NavigationEventArgs(string? pageTitle)
    {
        PageTitle = pageTitle;
    }
}