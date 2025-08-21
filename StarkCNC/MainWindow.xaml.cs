using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC.Controls;
using StarkCNC.Helpers;
using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navigationService;

        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel ViewModel { get; set; }

        public FlyoutMenuControl PageList { get; set; }

        private double _windowHeight;
        private double _windowWidth;
        private double _windowLeft;
        private double _windowTop;
        private bool _maximized = false;

        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _navigationService.Navigation += OnNavigation;

            _serviceProvider = serviceProvider;

            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            UpdateWindowBackground();
            UpdateMainWindowVisuals();
            
            PageList = _serviceProvider.GetRequiredService<FlyoutMenuControl>();
            PageList.Pages = ViewModel.Pages;
            Grid.SetRowSpan(PageList, 2);
            PageGrid.Children.Add(PageList);

            _navigationService.SetFrame(RootContentFrame);

            WindowChrome.SetWindowChrome(this,
                new WindowChrome
                {
                    CaptionHeight = 50,
                    CornerRadius = new CornerRadius(12),
                    GlassFrameThickness = new Thickness(-1),
                    ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                    UseAeroCaptionButtons = true,
                    NonClientFrameEdges = SystemParameters.HighContrast ? NonClientFrameEdges.None :
                        NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
                }
            );

            _windowHeight = Height;
            _windowWidth = Width;

            _navigationService.Navigate(ViewModel.Pages[0].Page);

            MaximizeWindow();

            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            StateChanged += (_, _) => UpdateMainWindowVisuals();
            Activated += (_, _) => UpdateMainWindowVisuals();
            Deactivated += (_, _) => UpdateMainWindowVisuals();
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Dispatcher.Invoke(() => UpdateMainWindowVisuals());
        }

        private void UpdateWindowBackground()
        {
            if ((!Utility.IsBackdropDisabled() && !Utility.IsBackdropSupported()))
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

            if (SystemParameters.HighContrast == true)
            {
                HighContrastBorder.SetResourceReference(BorderBrushProperty, IsActive ? SystemColors.ActiveCaptionBrushKey :
                                                                                        SystemColors.InactiveCaptionBrushKey);
                HighContrastBorder.BorderThickness = new Thickness(8, 1, 8, 8);

                WindowChrome wc = WindowChrome.GetWindowChrome(this);
                if (wc is not null)
                {
                    wc.NonClientFrameEdges = NonClientFrameEdges.None;
                }
            }
            else
            {
                HighContrastBorder.BorderBrush = Brushes.Transparent;
                HighContrastBorder.BorderThickness = new Thickness(0);

                var wc = WindowChrome.GetWindowChrome(this);
                if (wc is not null)
                {
                    wc.NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left;
                }
            }
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

        private void OnNavigation(object? sender, NavigationEventArgs e)
        {
            RootContentFrame.UpdateLayout();
            PageTitleTextBlock.Text = $"{Localization.Language.Tab}: {e.PageTitle}";

            string pageTitle = string.Empty;
            if (e.PageTitle is not null)
                pageTitle = e.PageTitle;

            var page = ViewModel.GetNavigationItem(pageTitle);
            PageList.UpdateSelected(page);
        }

        private void RootContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.UpdateCanNavigateBack();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.F12 && e.Key != System.Windows.Input.Key.Escape)
                return;

            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;

            if (e.Key == System.Windows.Input.Key.Escape && _maximized)
                MinimizeWindow();

            if (e.Key == System.Windows.Input.Key.F12 && !_maximized)
                MaximizeWindow();
            else
                MinimizeWindow();
        }

        private void MaximizeWindow()
        {
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
            _maximized = true;
            MaximizeIcon.Text = "\uE923";
        }

        private void MinimizeWindow()
        {
            WindowState = WindowState.Normal;
            ResizeMode = ResizeMode.CanResize;
            Topmost = false;
            _maximized = false;
            MaximizeIcon.Text = "\uE922";
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                MinimizeWindow();
            }
            else
            {
                MaximizeWindow();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}