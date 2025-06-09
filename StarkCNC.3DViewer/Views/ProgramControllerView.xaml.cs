using StarkCNC._3DViewer.ViewModels;
using System.Windows.Controls;

namespace StarkCNC._3DViewer.Views
{
    /// <summary>
    /// Interaction logic for ProgramControllerView.xaml
    /// </summary>
    public partial class ProgramControllerView : UserControl
    {
        ProgramControllerViewModel ViewModel;

        public ProgramControllerView(ProgramControllerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

            BendingView.Children.Add(ViewModel.GetModels());
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
    }
}
