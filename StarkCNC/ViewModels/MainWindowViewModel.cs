using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace StarkCNC.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private IManualConfigurationService _configurationService;

    private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;
    private DispatcherTimer _timer;
    private bool _disposed;

    [ObservableProperty]
    private string _title = "StarkCNC";

    [ObservableProperty]
    private ViewData? _selectedPage;

    private readonly IRouter _router;

    private readonly IBendingDataUnitOfWork _bendingUnitOfWork;

    private readonly IAdjustmentRepository _adjustmentRepository;

    private readonly IUserService _userService;

    private readonly IErrorsService _errorsService;

    private readonly IStartupSendService _startupSendService;

    private readonly AdjustmentViewModel _adjustmentViewModel;

    [ObservableProperty]
    private bool _canNavigateBack;

    [ObservableProperty]
    private ObservableCollection<ViewData> _pages = new ObservableCollection<ViewData>();

    private readonly ViewData _adjustmentPage;

    [ObservableProperty]
    private ObservableCollection<Status> _statuses = new ObservableCollection<Status>();

    [ObservableProperty]
    private bool _showStatus;

    [ObservableProperty]
    private bool _showHistory;

    [ObservableProperty]
    private ObservableCollection<Status> _history = new ObservableCollection<Status>();

    [ObservableProperty]
    private object _breadcrumb;

    [ObservableProperty]
    private string _currentUserName;

    public MainWindowViewModel(
        IRouter router,
        IBreadcrumbService breadcrumbService,
        IStatusService statusService,
        IErrorsService errorsService,
        IBendingDataUnitOfWork bendingUnitOfWork,
        IAdjustmentRepository adjustmentRepository,
        IUserService userService,
        IManualConfigurationService configurationService,
        IStartupSendService startupSendService,
        AdjustmentViewModel adjustmentViewModel) 
    {
        _router = router;
        _bendingUnitOfWork = bendingUnitOfWork;
        _adjustmentRepository = adjustmentRepository;
        _adjustmentViewModel = adjustmentViewModel;
        _userService = userService;
        _configurationService = configurationService;
        _errorsService = errorsService;
        _startupSendService = startupSendService;

        ShowStatus = true;
        if (statusService is not null)
        {
            statusService.CurrentStatuses.CollectionChanged += (sender, args) =>
            {
                Statuses = statusService.CurrentStatuses;
            };

            statusService.History.CollectionChanged += (sender, args) =>
            {
                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add &&
                    args.NewItems?.Count > 0)
                {
                    foreach (var item in args.NewItems)
                    {
                        if (item is Status status)
                            _dispatcher.Invoke(() => History.Add(status));
                    }
                }                    
            };
        }

        RegisterPages();
        _adjustmentPage = Pages.First(e => e.Title == "Оснастка");

        _adjustmentViewModel.SetUpAdjustments.CollectionChanged += SetUpAdjustments_CollectionChanged;
        _router.Navigated += (_, _) =>
        {
            if (_router.CurrentRoute == _router.GetRoute("/program"))
                ShowStatus = false;
            else
                ShowStatus = true;
            Breadcrumb = breadcrumbService.VisibleObject;
        };

        _timer = new DispatcherTimer(
            TimeSpan.FromMilliseconds(250),
            DispatcherPriority.Normal,
            (_, _) =>
            {
                var newValue = userService?.CurrentUser?.UserName ?? string.Empty;
                if (CurrentUserName != newValue)
                    CurrentUserName = newValue;
            },
            Application.Current.Dispatcher);
        _timer.Start();
    }

    public async Task InitializeAsync()
    {
        await Connect(_configurationService).ConfigureAwait(true);
        await _startupSendService.SendAllAsync().ConfigureAwait(true);
        await AdjustmentUpdateChildElements().ConfigureAwait(true);
        _errorsService.Subscribe();
        if (_configurationService is ManualConfigurationService mcs)
        {
            mcs.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(mcs.Connected) && mcs.Connected == true)
                {
                    await _startupSendService.SendAllAsync().ConfigureAwait(true);
                    _errorsService.Subscribe();
                }
            };
        }
    }

    private static async Task Connect(IManualConfigurationService configurationService)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        if (!configurationService.Connected)
            await configurationService.TryConnectAsync(cts.Token).ConfigureAwait(false);
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
    private async Task GoUsers()
    {
        var viewModel = await AuthorizationWindowViewModel.InitializeAsync(_userService).ConfigureAwait(true);
        var window = new AuthorizationWindow(viewModel);
        window.ShowDialog();
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
            if (route.Key == "/users" || route.Key == "/settings" || route.Key == "/file-selector")
                continue;

            if (route.Key.Split('/', StringSplitOptions.RemoveEmptyEntries).Length > 1)
                continue;

            Pages.Add(new ViewData(route.Value.Title, route.Value.IconGlyph, new RelayCommand(() => _router.Navigate(route.Value.Path))));
        }
    }

    private async Task AdjustmentUpdateChildElements()
    {
        _adjustmentPage.Items.Clear();
        var withLevel = await _adjustmentRepository.GetAdjustmentsWithLevelAsync().ConfigureAwait(false);
        foreach (var adjustment in withLevel)
        {
            var leveledAdjustmentViewData = new ViewData(
                $"{adjustment.Name} Этаж {adjustment.InstalledLevel}",
                null,
                new RelayCommand(() => _router.Navigate("/adjustment/list/edit", adjustment.Id)));

            var adjustmentCoordinateSettingsViewData = new ViewData(
                "Настройка координат",
                null,
                new RelayCommand(() => _router.Navigate("/adjustment/list/edit/coordinates", adjustment.Id)));

            leveledAdjustmentViewData.Items.Add(adjustmentCoordinateSettingsViewData);

            _adjustmentPage.Items.Add(leveledAdjustmentViewData);
        }
    }

    private async void SetUpAdjustments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        await AdjustmentUpdateChildElements().ConfigureAwait(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _timer.Stop();
        }

        _disposed = true;
    }
}