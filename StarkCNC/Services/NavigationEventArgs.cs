using System.Windows.Controls;

namespace StarkCNC.Services
{
    public class NavigationEventArgs
    {
        public string? PageTitle { get; set; } = null;

        public NavigationEventArgs() { }

        public NavigationEventArgs(string? pageTitle)
        {
            PageTitle = pageTitle;
        }
    }
}
