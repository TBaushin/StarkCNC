using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentView.xaml
    /// </summary>
    public partial class AdjustmentView : Page
    {
        private AdjustmentViewModel ViewModel;

        public AdjustmentView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void PipeDiameterInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
        }

        private void AdjustmentManagement_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void AdjustmentManagement_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void AdjustmentManagement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.SelectedAdjustment = null;
            ViewModel.GoToAdjustmentListCommand.Execute(null);
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

                var panels = VisualFinder.FindVisualChildren<StackPanel>(listViewItem).ToList();
                panels.ForEach(p => p.Visibility = Visibility.Collapsed);
            }

            foreach (var item in listView.SelectedItems)
            {
                var listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (listViewItem is null)
                    continue;

                var panels = VisualFinder.FindVisualChildren<StackPanel>(listViewItem).ToList();
                panels
                    .Where(p => p.Name == "InstallSelection").ToList()
                    .ForEach(p => p.Visibility = Visibility.Visible);
            }
        }

        private void SetUpButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button is null)
                return;

            var parent = VisualFinder.FindVisualParent<Grid>(button);
            if (parent is null)
                return;

            foreach(var child in parent.Children)
            {
                if (child is not StackPanel panel)
                    continue;

                if (panel.Name.Contains("LevelSelection", StringComparison.OrdinalIgnoreCase))
                    panel.Visibility = Visibility.Visible;
                else
                    panel.Visibility = Visibility.Collapsed;
            }
        }

        private void ThirdLevel_Click(object sender, RoutedEventArgs e)
        {
            SetLevelToAdjustment(3);
        }

        private void SecondLevel_Click(object sender, RoutedEventArgs e)
        {
            SetLevelToAdjustment(2);
        }

        private void FirstLevel_Click(object sender, RoutedEventArgs e)
        {
            SetLevelToAdjustment(1);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button is null)
                return;

            var parent = VisualFinder.FindVisualParent<Grid>(button);
            if (parent is null)
                return;

            foreach (var child in parent.Children)
            {
                if (child is not StackPanel panel)
                    continue;

                // При нажатии на Cancel, мы в отличие от эвента SetUpButton_Click как раз делаем видимым InstallSelection, и невидимым LevelSelection
                if (!panel.Name.Contains("LevelSelection", StringComparison.OrdinalIgnoreCase))
                    panel.Visibility = Visibility.Visible;
                else
                    panel.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetLevelToAdjustment(int level)
        {
            if (ViewModel.SelectedAdjustment is null)
                return;

            ViewModel.SelectedAdjustment.InstalledLevel = level;
        }
    }
}
