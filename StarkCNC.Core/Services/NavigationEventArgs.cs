namespace StarkCNC.Core.Services;

public class NavigationEventArgs
{
    public string? Title { get; set; }

    public object? Page { get; set; }

    public NavigationEventArgs(string? title, object? page)
    {
        Title = title;
        Page = page;
    }
}
