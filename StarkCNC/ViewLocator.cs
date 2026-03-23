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

    public static Page? Build(Type type, params object[] parameters)
    {
        if (_serviceProvider is null)
            throw new InvalidOperationException("ViewLocator is not initialized. Call ViewLocator.Initialize with a valid IServiceProvider before using.");

        if (type is null)
            return null;

        var name = type.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var pageType = Type.GetType(name);

        if (pageType is null)
            return GenerateNotFoundPage(name);

        var viewModel = parameters is not null && parameters.Length > 0
            ? ActivatorUtilities.CreateInstance(_serviceProvider, type, parameters)
            : ActivatorUtilities.CreateInstance(_serviceProvider, type);

        var page = (Page)ActivatorUtilities.CreateInstance(_serviceProvider, pageType, viewModel)!;

        return page;
    }

    public static Page GenerateNotFoundPage(string name) =>
        new Page
        {
            Content = new Label() { Content = $"Not Found: {name}" }
        };

    public static Page NoAccess() =>
        new Page
        {
            Content = new Label { Content = "Нет доступа к запрашиваемой странице" }
        };
}
