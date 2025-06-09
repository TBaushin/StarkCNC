using StarkCNC.Models;
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
        private INavigationService _navigationService;

        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigation += OnNavigation;

            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            _navigationService.SetFrame(RootContentFrame);

            PageList.SelectedItemChanged += SetSelectedItem;

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
            if (page is null)
            {
                foreach (var item in PageList.Items)
                {
                    TreeViewItem treeViewItem = PageList.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                    if (treeViewItem is null)
                        continue;

                    treeViewItem.IsSelected = false;
                }

                return;
            }

            var treeViewItemFromPage = PageList.ItemContainerGenerator.ContainerFromItem(page) as TreeViewItem;
            if (treeViewItemFromPage is null)
                return;

            treeViewItemFromPage.IsSelected = true;
        }

        private void SetSelectedItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var navItem = PageList.SelectedItem as ViewData;
            if (navItem is null) 
                return;

            _navigationService.Navigate(navItem.Page);
        }

        private void RootContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.UpdateCanNavigateBack();
        }
    }
}