using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Models;
using StarkCNC.Repository;
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

    private readonly IAdjustmentRepository _adjustmentRepository;

    private readonly AdjustmentViewModel _adjustmentViewModel;

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
        IAdjustmentRepository adjustmentRepository,
        AdjustmentViewModel adjustmentViewModel,
        ManualViewModel manualViewModel,
        VisualizationViewModel visualizationViewModel,
        ProgramViewModel programViewModel) 
    {
        _navigationService = navigationService;
        _router = router;
        _adjustmentRepository = adjustmentRepository;
        _adjustmentViewModel = adjustmentViewModel;

        if (statusService is not null)
            statusService.PropertyChanged += (_, _) => Status = statusService.Status;

        RegisterPages();
        _adjustmentPage = Pages.First(e => e.Title == "Оснастка");
        AdjustmentUpdateChildElements();

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

    private void RegisterPages()
    {
        var routes = new string[] { "/manual", "/visualization", "/program", "/adjustment" };
        foreach (var route in routes)
        {
            var realRoute = _router.GetRoute(route);
            if (realRoute is null)
                continue;

            Pages.Add(new ViewData(realRoute.Title, realRoute.IconGlyph, new RelayCommand(() => _router.Navigate(realRoute.Path))));
        }
    }

    private async void AdjustmentUpdateChildElements()
    {
        _adjustmentPage.Items.Clear();
        var withLevel = await _adjustmentRepository.GetAdjustmentsWithLevelAsync().ConfigureAwait(false);
        foreach (var adjustment in withLevel)
        {
            var leveledAdjustmentViewData = new ViewData(
                $"{adjustment.Name} Этаж {adjustment.InstalledLevel}",
                null,
                new RelayCommand(() => _router.Navigate("/adjustment/edit", adjustment.Id)));

            var adjustmentCoordinateSettingsViewData = new ViewData(
                "Настройка координат",
                null,
                new RelayCommand(() => _router.Navigate("/adjustment/edit/coordinates", adjustment.Id)));

            leveledAdjustmentViewData.Items.Add(adjustmentCoordinateSettingsViewData);

            _adjustmentPage.Items.Add(leveledAdjustmentViewData);
        }
    }

    private void SetUpAdjustments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        AdjustmentUpdateChildElements();
    }
}