using System.Windows.Controls;

namespace StarkCNC.Services;

public class NavigationService : INavigationService
{
    private Frame? _frame;

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

    public event EventHandler<NavigationEventArgs>? Navigation;

    public void Navigate(object content)
    {
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