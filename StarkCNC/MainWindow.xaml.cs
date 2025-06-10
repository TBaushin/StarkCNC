using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Controls;
using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private INavigationService _navigationService;

        private IServiceProvider _serviceProvider;

        public MainWindowViewModel ViewModel { get; set; }

        public FlyoutMenuControl PageList { get; set; }

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
        }

        public void OnNavigation(object? sender, NavigationEventArgs e)
        {
            RootContentFrame.UpdateLayout();
            PageTitleTextBlock.Text = $"{Localization.Language.Tab}: {e.PageTitle}";

            var page = ViewModel.GetNavigationItem(e.PageTitle);
            PageList.UpdateSelected(page);
        }

        private void RootContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.UpdateCanNavigateBack();
        }
    }
}