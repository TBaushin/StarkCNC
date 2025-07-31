using HelixToolkit.Wpf;
using StarkCNC._3DViewer.ViewModels;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Views
{
    /// <summary>
    /// Interaction logic for VisualizationControllerView.xaml
    /// </summary>
    public partial class VisualizationControllerView : UserControl
    {
        private readonly VisualizationControllerViewModel ViewModel;

        public VisualizationControllerView(VisualizationControllerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

            BendingView.Children.Add(ViewModel.GetModels());
            //BendingView.Children.Add(ViewModel.GetPipe());

            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            Dictionary<string, double> positions = ViewModel.GetDefaults();
            ConsoleSlider.Value = positions["console"];
            BendSlider.Value = positions["bend"];
            SupplySlider.Value = positions["carriage"];
            HeightSlider.Value = positions["height"];
            ClampSlider.Value = positions["clamp"];
            PressSlider.Value = positions["press"];
        }

        private void Sliders_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.UpdatePositions(
                ConsoleSlider.Value,
                BendSlider.Value,
                SupplySlider.Value,
                HeightSlider.Value,
                ClampSlider.Value,
                PressSlider.Value
            );
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
