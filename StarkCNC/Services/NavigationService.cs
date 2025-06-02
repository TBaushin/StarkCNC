using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace StarkCNC.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        private readonly Stack<object> _history = new Stack<object>();

        private readonly Stack<object> _future = new Stack<object>();

        private object _currentContent = null;

        public bool CanGoBack => _history.Count > 0;

        public bool CanGoForward => _future.Count > 0;

        public event EventHandler<NavigationEventArgs> Navigation;

        public void GoBack()
        {
            if (!CanGoBack)
                return;
            if (_currentContent is not null)
                _future.Push(_currentContent);
            _currentContent = _history.Pop();
            _frame.Navigate(_currentContent);
            RaiseNavigationEvent(_currentContent);
        }

        public void GoForward()
        {
            if (!CanGoForward)
                return;
            if (_currentContent is not null)
                _history.Push(_currentContent);
            _currentContent = _future.Pop();
            _frame.Navigate(_currentContent);
            RaiseNavigationEvent(_currentContent);
        }

        public void Navigate(object content)
        {
            _currentContent = content;
            _frame.Navigate(_currentContent);
            RaiseNavigationEvent(_currentContent);
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void RaiseNavigationEvent(object content)
        {
            if (content is Page page)
            {
                Navigation?.Invoke(this, new NavigationEventArgs(page.Title));
            }
        }
    }
}
