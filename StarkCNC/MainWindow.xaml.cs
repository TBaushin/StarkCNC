using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Controls;
using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
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

            WindowChrome ws = WindowChrome.GetWindowChrome(this);
            ws.NonClientFrameEdges = SystemParameters.HighContrast ? NonClientFrameEdges.None :
                        NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left;

            _windowHeight = Height;
            _windowWidth = Width;

            _navigationService.Navigate(ViewModel.Pages[0].Page);
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
            _windowHeight = Height;
            _windowWidth = Width;
            _windowLeft = Left;
            _windowTop = Top;
            Height = SystemParameters.PrimaryScreenHeight;
            Width = SystemParameters.PrimaryScreenWidth;
            Left = 0;
            Top = 0;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
            _maximized = true;

        }

        private void MinimizeWindow()
        {
            WindowState = WindowState.Normal;
            Height = _windowHeight;
            Width = _windowWidth;
            Left = _windowLeft;
            Top = _windowTop;
            ResizeMode = ResizeMode.CanResize;
            Topmost = false;
            _maximized = false;
        }
    }
}