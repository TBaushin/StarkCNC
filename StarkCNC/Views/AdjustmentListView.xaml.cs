using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

                var btns = FindVisualChildren<Button>(listViewItem);
                foreach (var b in btns) b.Visibility = Visibility.Collapsed;
            }

            foreach (var item in listView.SelectedItems)
            {
                var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (listViewItem is null)
                    continue;

                List<Button> buttons = FindVisualChildren<Button>(listViewItem).ToList();
                foreach(var button in buttons)
                {
                    button.Visibility = Visibility.Visible;
                }
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for(int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                    yield return t;

                foreach(var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
