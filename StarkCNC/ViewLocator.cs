using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace StarkCNC;

public static class ViewLocator
{
    private static IServiceProvider? _serviceProvider;
    private static readonly Dictionary<Type, Type> _viewCache = new();

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static Type? GetPageType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!_viewCache.TryGetValue(type, out var pageType))
        {
            var name = type.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            pageType = Type.GetType(name);

            if (pageType is not null)
                _viewCache[type] = pageType;
        }

        return pageType;
    }

    public static Page? Build(Type type, params object[] parameters)
    {
        if (_serviceProvider is null)
            throw new InvalidOperationException("ViewLocator is not initialized. Call ViewLocator.Initialize with a valid IServiceProvider before using.");

        if (type is null)
            return null;

        var pageType = GetPageType(type);
        if (pageType is null)
            return GenerateNotFoundPage(type.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal));

        var viewModel = _serviceProvider.GetService(type);
        if (viewModel is null)
            viewModel = parameters is not null && parameters.Length > 0
                ? ActivatorUtilities.CreateInstance(_serviceProvider, type, parameters)
                : ActivatorUtilities.CreateInstance(_serviceProvider, type);

        var page = (Page)ActivatorUtilities.CreateInstance(_serviceProvider, pageType, viewModel)!;

        return page;
    }

    public static Page? Build(Type viewModelType, Type viewType, params object[] parameters)
    {
        if (_serviceProvider is null)
            throw new InvalidOperationException("ViewLocator is not initialized. Call ViewLocator.Initialize with a valid IServiceProvider before using.");

        if (viewModelType is null)
            return null;

        if (viewType is null)
            return null;

        if (!_viewCache.TryGetValue(viewModelType, out var viewTypeExpected) && viewTypeExpected != viewType)
            return GenerateNotFoundPage(viewModelType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal));

        var viewModel = _serviceProvider.GetService(viewModelType);
        if (viewModel is null)
            viewModel = parameters is not null && parameters.Length > 0
                ? ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, parameters)
                : ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType);

        var page = (Page)ActivatorUtilities.CreateInstance(_serviceProvider, viewType, viewModel)!;

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
