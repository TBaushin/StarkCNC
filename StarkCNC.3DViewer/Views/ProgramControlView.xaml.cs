using StarkCNC._3DViewer.ViewModels;
using System.Windows.Controls;

namespace StarkCNC._3DViewer.Views
{
    /// <summary>
    /// Interaction logic for ProgramControlView.xaml
    /// </summary>
    public partial class ProgramControlView : UserControl
    {
        private readonly ProgramControllerViewModel ViewModel;

        public ProgramControlView(ProgramControllerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

            BendingView.Children.Add(ViewModel.GetPipe());
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
}
