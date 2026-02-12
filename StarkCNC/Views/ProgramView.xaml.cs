using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for ProgramView.xaml
/// </summary>
public partial class ProgramView : Page
{
    private readonly ProgramViewModel ViewModel;

    public ProgramView(ProgramViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
        BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

        BendingView.Children.Add(ViewModel.Pipe);
    }

    private void ZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(-0.1); // Не знаю, но отрицательное число приближает, а положительное отодвигает
    }

    private void ZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(0.1);
    }

    private void Input_TextChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.UpdateBend();
    }

    private void SetSupplySpeedButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetSupplySpeedToAllCommand.Execute(null);
    }

    private void SetRotationSpeedButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetRotationSpeedToAllCommand.Execute(null);
    }

    private void SetBendSpeedButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetBendSpeedToAllCommand.Execute(null);
    }

    private void SetBendCoefficientButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetBendCoefficientToAllCommand.Execute(null);
    }

    private void SetRadiusModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetRadiusModeToAllCommand.Execute(null);
    }

    private void SetOffsetSpeedButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetOffsetSpeedToAllCommand.Execute(null);
    }

    private void SetOffsetCoefficientButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        ViewModel.SetOffsetCoefficientToAllCommand.Execute(null);
    }

    private void DeleteBendingDataButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DeleteSelectedBendingData();
    }

    private void BendingDatasDataGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Delete)
            DeleteSelectedBendingData();
    }

    private void DeleteSelectedBendingData()
    {
        var item = BendingDatasDataGrid.SelectedItem as BendingDataViewModel;
        if (item is null)
            return;

        ViewModel.RemoveBendingDataCommand.Execute(item);
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.UpdateBend();
    }

    private void dgUp_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var scrollViewer = GetInternalScrollViewer(BendingDatasDataGrid);
        if (scrollViewer is null)
            return;

        scrollViewer.LineUp();
    }

    private void dgDown_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var scrollViewer = GetInternalScrollViewer(BendingDatasDataGrid);
        if (scrollViewer is null)
            return;

        scrollViewer.LineDown();
    }

    private ScrollViewer? GetInternalScrollViewer(DependencyObject element)
    {
        if (element is ScrollViewer scrollViewer)
            return scrollViewer;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            var result = GetInternalScrollViewer(child);
            if (result is not null)
                return result;
        }

        return null;
    }
}