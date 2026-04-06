using Microsoft.Win32;
using StarkCNC.Controls;
using StarkCNC.Core.Services;
using StarkCNC.Utilities;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Windows.Data;

namespace StarkCNC;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; set; }

    public FlyoutMenuControl FlyoutMenu { get; set; }

    public MainWindow(IRouter router, MainWindowViewModel viewModel)
    {
        if (router is null)
            throw new ArgumentNullException(nameof(router));

        router.Navigated += OnNavigation;

        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        UpdateWindowBackground();
        UpdateMainWindowVisuals();

        FlyoutMenu = new FlyoutMenuControl();
        BindingOperations.SetBinding(
            FlyoutMenu,
            FlyoutMenuControl.CurrentUserNameProperty,
            new Binding(nameof(viewModel.CurrentUserName)) { Source = viewModel });
        BindingOperations.SetBinding(
            FlyoutMenu,
            FlyoutMenuControl.PagesProperty,
            new Binding(nameof(viewModel.Pages)) { Source = viewModel });
        FlyoutMenu.Pages = ViewModel.Pages;
        FlyoutMenu.MenuIsOpen = true;
        Grid.SetRowSpan(FlyoutMenu, 3);
        PageGrid.Children.Add(FlyoutMenu);

        router.SetFrame(RootContentFrame);

        WindowChrome.SetWindowChrome(this,
            new WindowChrome
            {
                CaptionHeight = 50,
                CornerRadius = new CornerRadius(12),
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = true,
                NonClientFrameEdges = GetPrefferedNonClientFrameEdges()
            }
        );

        router.Navigate("/manual");

        MaximizeWindow();

        SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        StateChanged += (_, _) => UpdateMainWindowVisuals();
        Activated += (_, _) => UpdateMainWindowVisuals();
        Deactivated += (_, _) => UpdateMainWindowVisuals();

        DispatcherTimer timer = new DispatcherTimer(
            TimeSpan.FromSeconds(1),
            DispatcherPriority.Normal,
            (_, _) => CurrentTimeLabel.Content = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture),
            Dispatcher);
        timer.Start();
    }

    private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        Dispatcher.Invoke(() => UpdateMainWindowVisuals());
    }

    private void UpdateWindowBackground()
    {
        if (!Utility.IsBackdropDisabled() && !Utility.IsBackdropSupported())
        {
            this.SetResourceReference(BackgroundProperty, "WindowBackground");
        }
    }

    private void UpdateMainWindowVisuals()
    {
        MainGrid.Margin = default;
        if (WindowState == WindowState.Maximized)
        {
            MainGrid.Margin = SystemParameters.HighContrast ? new Thickness(0, 8, 0, 0) : new Thickness(8);
        }

        UpdateTitleBarButtonsVisibility();

        SetWindowVisual();
    }

    private void UpdateTitleBarButtonsVisibility()
    {
        if (Utility.IsBackdropDisabled() || !Utility.IsBackdropSupported() ||
                SystemParameters.HighContrast == true)
        {
            MinimizeButton.Visibility = Visibility.Visible;
            MaximizeButton.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
        }
        else
        {
            MinimizeButton.Visibility = Visibility.Collapsed;
            MaximizeButton.Visibility = Visibility.Collapsed;
            CloseButton.Visibility = Visibility.Collapsed;
        }
    }

    private void SetWindowVisual()
    {
        if (SystemParameters.HighContrast == true)
        {
            HighContrastBorder.SetResourceReference(BorderBrushProperty, IsActive ? SystemColors.ActiveCaptionBrushKey :
                                                                                    SystemColors.InactiveCaptionBrushKey);
            HighContrastBorder.BorderThickness = new Thickness(8, 1, 8, 8);
        }
        else
        {
            HighContrastBorder.BorderBrush = Brushes.Transparent;
            HighContrastBorder.BorderThickness = new Thickness(0);
        }

        if (Utility.IsWindows110rGreater())
        {
            WindowChrome wc = WindowChrome.GetWindowChrome(this);
            if (wc is not null)
                wc.NonClientFrameEdges = GetPrefferedNonClientFrameEdges();
        }
    }

    private static NonClientFrameEdges GetPrefferedNonClientFrameEdges()
    {
        if (SystemParameters.HighContrast == true || Utility.IsWindows110rGreater() == false)
            return NonClientFrameEdges.None;

        return NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left;
    }

    private void OnNavigation(object? sender, NavigationEventArgs e)
    {
        Breadcrumbs.Children.Clear();

        RootContentFrame.UpdateLayout();
        //PageTitleTextBlock.Text = $"{Localization.Language.Tab}: {e.Title}";

        var breadcrumb = ViewModel.Breadcrumb as StackPanel;
        if (breadcrumb is not null)
            Breadcrumbs.Children.Add(breadcrumb);

        string pageTitle = string.Empty;
        if (e.Title is not null)
            pageTitle = e.Title;

        var page = ViewModel.GetNavigationItem(pageTitle);
        FlyoutMenu.UpdateSelected(page);
    }

    private void RootContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {
        ViewModel.UpdateCanNavigateBack();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key != System.Windows.Input.Key.F12 && e.Key != System.Windows.Input.Key.Escape)
            return;

        if (e.Key == System.Windows.Input.Key.Escape)
            MinimizeWindow();

        if (e.Key == System.Windows.Input.Key.F12 && WindowState != WindowState.Maximized)
            MaximizeWindow();
        else
            MinimizeWindow();
    }

    private void MaximizeWindow()
    {
        WindowState = WindowState.Maximized;
        ResizeMode = ResizeMode.NoResize;
        Topmost = true;
        MaximizeIcon.Text = "\uE923";
    }

    private void MinimizeWindow()
    {
        if (WindowState == WindowState.Normal && ResizeMode == ResizeMode.CanResize && Topmost == false && MaximizeIcon.Text == "\uE922")
            return;

        WindowState = WindowState.Normal;
        ResizeMode = ResizeMode.CanResize;
        Topmost = false;
        MaximizeIcon.Text = "\uE922";
    }

    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            MinimizeWindow();
        }
        else
        {
            MaximizeWindow();
        }
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (WindowState != WindowState.Maximized)
            MinimizeWindow();
        else
            MaximizeWindow();

        if (Width < 1200)
        {
            FlyoutMenu.MenuIsOpen = false;
        }
    }

    private void HistoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (StatusHistoryDataGrid.Visibility == Visibility.Collapsed)
            StatusHistoryDataGrid.Visibility = Visibility.Visible;
        else
            StatusHistoryDataGrid.Visibility = Visibility.Collapsed;
    }
}