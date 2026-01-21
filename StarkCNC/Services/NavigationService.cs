using System.Windows.Controls;

namespace StarkCNC.Services;

public class NavigationService : INavigationService
{
    private Frame? _frame;

    private readonly Stack<Type> _history = new Stack<Type>();

    private readonly Stack<Type> _future = new Stack<Type>();

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

        if (CurrentContent is not null)
            _future.Push(CurrentContent.GetType());
        CurrentContent = ViewLocator.Build(GetViewModel(_history.Pop()));
    }

    public void GoForward()
    {
        if (!CanGoForward)
            return;

        if (CurrentContent is not null)
            _history.Push(CurrentContent.GetType());
        CurrentContent = ViewLocator.Build(GetViewModel(_future.Pop()));
    }

    public void Navigate(object content)
    {
        _future.Clear();

        if (CurrentContent is not null)
            _history.Push(CurrentContent.GetType());
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

    private static Type GetViewModel(Type type)
    {
        var name = type.FullName!;
        if (name.Contains("ViewModel", StringComparison.CurrentCultureIgnoreCase))
            return type;

        var viewModelType = Type.GetType(name.Replace("View", "ViewModel", StringComparison.CurrentCultureIgnoreCase));
        return viewModelType!;
    }
}