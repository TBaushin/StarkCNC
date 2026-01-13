using System.Windows.Controls;

namespace StarkCNC.Services;

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

            if (value is not null)
            {
                NavigateToContent(value);

                if (value is Page page)
                    Navigation?.Invoke(this, new NavigationEventArgs(page));
            }
        }
    }

    public bool CanGoBack => _history.Any();

    public bool CanGoForward => _future.Any();

    public event EventHandler<NavigationEventArgs>? Navigation;

    public void GoBack()
    {
        if (!CanGoBack)
            return;

        if (_currentContent is not null)
            _future.Push(_currentContent);
        CurrentContent = _history.Pop();
    }

    public void GoForward()
    {
        if (!CanGoForward)
            return;

        if (CurrentContent is not null)
            _history.Push(CurrentContent);
        CurrentContent = _future.Pop();
    }

    public void Navigate(object content)
    {
        _future.Clear();

        if (CurrentContent is not null)
            _history.Push(CurrentContent);
        CurrentContent = content;
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