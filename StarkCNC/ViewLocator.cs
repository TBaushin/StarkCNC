using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace StarkCNC;

public static class ViewLocator
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static Page? Build(Type param)
    {
        if (_serviceProvider is null)
            throw new InvalidOperationException("ViewLocator is not initialized. Call ViewLocator.Initialize with a valid IServiceProvider before using.");

        if (param is null)
            return null;

        var name = param.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type is null)
            return GenerateNotFoundPage(name);

        var page = (Page)ActivatorUtilities.CreateInstance(_serviceProvider, type)!;
        page.DataContext = ActivatorUtilities.CreateInstance(_serviceProvider, param)!;

        return page;
    }

    public static Page GenerateNotFoundPage(string name) =>
        new Page
        {
            Content = new Label() { Content = $"Not Found: {name}" }
        };
}
