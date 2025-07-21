using StarkCNC._3DViewer.ViewModels;
using System.Windows.Controls;

namespace StarkCNC._3DViewer.Views
{
    /// <summary>
    /// Interaction logic for ProgramControlView.xaml
    /// </summary>
    public partial class ProgramControlView : UserControl
    {
        ProgramControllerViewModel ViewModel;

        public ProgramControlView(ProgramControllerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

            BendingView.Children.Add(ViewModel.GetPipe());
        }
    }
}
