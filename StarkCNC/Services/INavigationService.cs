using System.Windows.Controls;

namespace StarkCNC.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        bool CanGoForward { get; }

        public event EventHandler<NavigationEventArgs> Navigation;

        void Navigate(object content);

        void GoBack();

        void GoForward();

        void SetFrame(Frame frame);
    }
}
