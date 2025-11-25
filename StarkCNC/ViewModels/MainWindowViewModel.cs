using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Services;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = "StarkCNC";

    [ObservableProperty]
    private ViewData? _selectedPage;

    private readonly INavigationService _navigationService;

    private readonly IRouter _router;

    private readonly AdjustmentViewModel _adjustmentViewModel;

    private readonly SettingsViewModel _settingsViewModel;

    [ObservableProperty]
    private bool _canNavigateBack;

    [ObservableProperty]
    private ObservableCollection<ViewData> _pages = new ObservableCollection<ViewData>();

    private readonly ViewData _adjustmentPage;

    [ObservableProperty]
    private string _status = string.Empty;

    [ObservableProperty]
    private object _breadcrumb;

    public MainWindowViewModel(
        INavigationService navigationService,
        IRouter router,
        IBreadcrumbService breadcrumbService,
        IStatusService statusService,
        AdjustmentViewModel adjustmentViewModel,
        SettingsViewModel settingsViewModel,
        ManualViewModel manualViewModel,
        VisualizationViewModel visualizationViewModel,
        ProgramViewModel programViewModel) 
    {
        _navigationService = navigationService;
        _router = router;
        _adjustmentViewModel = adjustmentViewModel;
        _settingsViewModel = settingsViewModel;

        if (statusService is not null)
            statusService.PropertyChanged += (_, _) => Status = statusService.Status;

        _adjustmentPage = new ViewData(ViewLocator.Build(typeof(AdjustmentViewModel))) { IconGlyph = "\uE726" };
        AdjustmentUpdateChildElements();
        foreach (var item in _router.GetRoutes())
        {
            if (item.Key == "/settings" || item.Key == "/users")
                continue;
            _pages.Add(new ViewData(ViewLocator.Build(item.Value)));
        }
        _pages.Add(_adjustmentPage);
        //_pages = [
        //    new ViewData(new ManualView(App.ServiceProvider.GetRequiredService<ManualViewModel>())) { IconGlyph = "\uE732" },
        //    new ViewData(new VisualizationView(App.ServiceProvider.GetRequiredService<VisualizationViewModel>())) { IconGlyph = "\uE726" },
        //    new ViewData(new ProgramView()) { IconGlyph = "\uE726" },
        //    _adjustmentPage
        //];

        _adjustmentViewModel.SetUpAdjustments.CollectionChanged += SetUpAdjustments_CollectionChanged;
        _navigationService.Navigation += (_, _) => Breadcrumb = breadcrumbService.VisibleObject;
    }

    [RelayCommand]
    private void Back()
    {
        _navigationService.GoBack();
    }

    [RelayCommand]
    private void Forward()
    {
        _navigationService.GoForward();
    }

    [RelayCommand]
    private void GoSettings()
    {
        _router.Navigate("/settings");
    }

    [RelayCommand]
    private void GoUsers()
    {
        _router.Navigate("/users");
    }

    public void UpdateCanNavigateBack()
    {
        CanNavigateBack = _navigationService.CanGoBack;
    }

    public ViewData? GetNavigationItem(string title)
    {
        return Pages.FirstOrDefault(e => e.Title == title);
    }

    private void AdjustmentUpdateChildElements()
    {
        _adjustmentPage.Items.Clear();
        foreach (var adjustment in _adjustmentViewModel.SetUpAdjustments)
        {
            //var adjustmentSettingPage = new AdjustmentParametersView(_adjustmentViewModel, adjustment);
            //var viewData = new ViewData(adjustmentSettingPage) { Title = $"{adjustment.Name} Этаж {adjustment.InstalledLevel}" };

            //var adjustmentCoordinateSettingsPage = new AdjustmentCoordinateSettingsView(
            //    _settingsViewModel.Settings,
            //    _adjustmentViewModel,
            //    adjustment);
            //viewData.Items.Add(new ViewData(adjustmentCoordinateSettingsPage) { Title = "Настройка координат" });

            //_adjustmentPage.Items.Add(viewData);
        }
    }

    private void SetUpAdjustments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        AdjustmentUpdateChildElements();
    }
}