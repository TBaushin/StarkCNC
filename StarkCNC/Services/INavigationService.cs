using System.Windows.Controls;

namespace StarkCNC.Services;

public interface INavigationService
{
    public event EventHandler<NavigationEventArgs> Navigation;

    void Navigate(object content);

    void SetFrame(Frame frame);
}