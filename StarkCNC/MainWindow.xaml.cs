using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shell;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IServiceProvider _serviceProvider;
        private INavigationService _navigationService;

        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider, INavigationService navigationService)
        {
            _serviceProvider = serviceProvider;
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            _navigationService = navigationService;
            _navigationService.Navigation += OnNavigation;
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
            IEnumerable<Page> list = ViewModel.GetNavigationItem();

            if (list.Count() <= 0) return;

            TreeViewItem selectedTreeViewItem = null;
            ItemsControl itemsControl = PageList;
            foreach (var item in list)
            {
                var tvi = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (tvi is null) continue;

                tvi.IsEnabled = true;
                tvi.UpdateLayout();
                itemsControl = tvi;
                selectedTreeViewItem = tvi;
            }

            if (selectedTreeViewItem is not null)
            {
                selectedTreeViewItem.IsEnabled = true;
                SetSelectedItem();
            }
        }

        private void PageList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetSelectedItem();
            }
        }

        private void PageList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if(e.OriginalSource is ToggleButton)
            //{
            //    return;
            //}
            SetSelectedItem();
        }

        private void SetSelectedItem()
        {
            if (PageList.SelectedItem is Page navItem)
            {
                _navigationService.Navigate(navItem);
                var tvi = PageList.ItemContainerGenerator.ContainerFromItem(navItem) as TreeViewItem;
                if (tvi is null) return;

                tvi.BringIntoView();
            }
        }

        private void RootContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.UpdateCanNavigateBack();
        }
    }
}