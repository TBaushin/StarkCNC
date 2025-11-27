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

    private void ZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(-0.1); // Не знаю, но отрицательное число приближает, а положительное отодвигает
    }

    private void ZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        BendingView.CameraController.Zoom(0.1);
    }
}