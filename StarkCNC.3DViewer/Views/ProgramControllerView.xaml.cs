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

            BendingView.Children.Add(ViewModel.GetModels());
        }
    }
}
