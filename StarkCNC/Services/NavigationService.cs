using System.Windows.Controls;

namespace StarkCNC.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _frame;

        private readonly Stack<object> _history = new Stack<object>();

        private readonly Stack<object> _future = new Stack<object>();

        private object? _currentContent;

        public object? CurrentContent 
        { 
            get => _currentContent; 
            set
            {
                _currentContent = value;

                if (value is Page page)
                    Navigation?.Invoke(this, new NavigationEventArgs(page.Title));
            } 
        }

        public bool CanGoBack
        {
            get
            {
                if (_history.Count <= 0)
                    return false;

                var content = _history.Peek();
                if (content is null)
                    return false;

                return true;
            }
        }

        public bool CanGoForward
        {
            get
            {
                if (_future.Count <= 0)
                    return false;

                var content = _future.Peek();
                if (content is null)
                    return false;

                return true;
            }
        }

        public event EventHandler<NavigationEventArgs>? Navigation;

        public void GoBack()
        {
            if (!CanGoBack)
                return;

            if (_currentContent is not null)
                _future.Push(_currentContent);
            CurrentContent = _history.Pop();

            NavigateToContent(CurrentContent);
        }

        public void GoForward()
        {
            if (!CanGoForward)
                return;

            if (_currentContent is not null)
                _history.Push(_currentContent);
            CurrentContent = _future.Pop();

            NavigateToContent(CurrentContent);
        }

        public void Navigate(object content)
        {
            _future.Clear();

            if (_currentContent is not null)
                _history.Push(_currentContent);
            CurrentContent = content;

            NavigateToContent(content);
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        private void NavigateToContent(object content)
        {
            _frame?.Navigate(content);
        }
    }
}
