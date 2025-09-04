using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentListView.xaml
    /// </summary>
    public partial class AdjustmentListView : Page
    {
        AdjustmentViewModel ViewModel;

        public AdjustmentListView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView is null)
                return;

            foreach (var item in e.RemovedItems)
            {
                var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (listViewItem is null)
                    continue;

                var buttons = VisualFinder.FindVisualChildren<Button>(listViewItem).ToList();
                buttons.ForEach(b => b.Visibility = Visibility.Collapsed);
            }

            foreach (var item in listView.SelectedItems)
            {
                var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (listViewItem is null)
                    continue;

                var buttons = VisualFinder.FindVisualChildren<Button>(listViewItem).ToList();
                buttons.ForEach(b => b.Visibility = Visibility.Visible);
            }
        }
    }
}
