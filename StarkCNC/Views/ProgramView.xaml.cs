using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ProgramView.xaml
    /// </summary>
    public partial class ProgramView : Page
    {
        ProgramViewModel ViewModel;

        public ProgramView(ProgramViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            ProgramControllerBorder.Child = ViewModel.GetProgramControllerView();
        }

        private void PipeBendParametersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Apply.IsEnabled = true;
        }

        private void Apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.UpdateBend();
            Apply.IsEnabled = false;
        }
    }
}
