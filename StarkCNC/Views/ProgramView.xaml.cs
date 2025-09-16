using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Windows.Controls;

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

    private void PipeBendParametersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        /*var item = PipeBendParametersDataGrid.CurrentItem;
        if (item is not BendingData data)
            return;

        var value = NumberInputViewModel.ShowDialog();

        switch (PipeBendParametersDataGrid.CurrentColumn.DisplayIndex)
        {
            case 1:
                data.StraightLength = value;
                break;
            case 2:
                data.BendingAngle = value;
                break;
            case 3:
                data.BendingRadius = value;
                break;
            case 4:
                data.RotationAngle = value;
                break;
        }*/
    }

    private void PipeBendParametersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        ViewModel.UpdateBend();
    }

    private void ZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(-0.1); // Не знаю, но отрицательное число приближает, а положительное отодвигает
    }

    private void ZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(0.1);
    }
}