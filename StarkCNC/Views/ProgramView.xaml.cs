using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ProgramView.xaml
    /// </summary>
    public partial class ProgramView : Page
    {
        private readonly ProgramViewModel ViewModel;
        private Brush? _applyColor; 

        public ProgramView(ProgramViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            var programControllerView = ViewModel.GetProgramControllerView();
            Grid.SetColumn(programControllerView, 1);

            MainGrid.Children.Add(programControllerView);
        }

        private void PipeBendParametersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Apply.IsEnabled = true;

            if (_applyColor is null)
                _applyColor = Apply.Background;

            Apply.Background = Brushes.Green;
        }

        private void Apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.UpdateBend();
            Apply.IsEnabled = false;
            Apply.Background = _applyColor;
        }
    }
}
