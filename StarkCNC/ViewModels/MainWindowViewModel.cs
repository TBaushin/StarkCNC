using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace StarkCNC.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = "StarkCNC";

    [ObservableProperty]
    private ViewData? _selectedPage;

    private readonly IRouter _router;

    private readonly IBendingDataUnitOfWork _bendingUnitOfWork;

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
    private bool _showStatus;

    [ObservableProperty]
    private object _breadcrumb;

    [ObservableProperty]
    private string _currentUserName;

    public MainWindowViewModel(
        IRouter router,
        IBreadcrumbService breadcrumbService,
        IStatusService statusService,
        IBendingDataUnitOfWork bendingUnitOfWork,
        IAdjustmentRepository adjustmentRepository,
        IUserService userService,
        AdjustmentViewModel adjustmentViewModel) 
    {
        _router = router;
        _bendingUnitOfWork = bendingUnitOfWork;
        _adjustmentRepository = adjustmentRepository;
        _adjustmentViewModel = adjustmentViewModel;

        if (statusService is not null)
        {
            ShowStatus = statusService.ShowStatus;
            statusService.PropertyChanged += (sender, args) =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    if (statusService.CurrentStatus is null || string.IsNullOrEmpty(statusService.CurrentStatus.Text))
                        Status = string.Empty;
                    else
                        Status = statusService.CurrentStatus.Text;
                });

                if (args.PropertyName == nameof(statusService.ShowStatus))
                    ShowStatus = statusService.ShowStatus;
            };
        }

        RegisterPages();
        _adjustmentPage = Pages.First(e => e.Title == "Оснастка");
        AdjustmentUpdateChildElements();

        _adjustmentViewModel.SetUpAdjustments.CollectionChanged += SetUpAdjustments_CollectionChanged;
        _router.Navigated += (_, _) =>
        {
            if (_router.CurrentRoute == _router.GetRoute("/program"))
                statusService?.ShowStatus = false;
            else
                statusService?.ShowStatus = true;
            Breadcrumb = breadcrumbService.VisibleObject;
        };

        Task.Run(() =>
        {
            while (true)
            {
                CurrentUserName = userService?.CurrentUser?.UserName ?? string.Empty;
                Task.Delay(150);
            }
        });
    }

    [RelayCommand]
    private void Back()
    {
        _router.GoBack();
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

    [RelayCommand]
    private void CloseApp()
    {
        if (_bendingUnitOfWork.HasUnsavedData)
        {
            var answer = MessageBox.Show(
                "На странице \"Программа\" есть несохранённые данные. Вы уверены, что хотите закрыть приложение?",
                "Есть несохранённые данные",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;
        }
        App.Current.Shutdown();
    }

    public void UpdateCanNavigateBack()
    {
        CanNavigateBack = _router.CanGoBack;
    }

    public ViewData? GetNavigationItem(string title)
    {
        return Pages.FirstOrDefault(e => e.Title == title);
    }

    private void RegisterPages()
    {
        foreach (var route in _router.GetRoutes())
        {
            if (route.Key == "/users" || route.Key == "/settings")
                continue;

            if (route.Key.Split('/', StringSplitOptions.RemoveEmptyEntries).Length > 1)
                continue;

            Pages.Add(new ViewData(route.Value.Title, route.Value.IconGlyph, new RelayCommand(() => _router.Navigate(route.Value.Path))));
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