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

        private void PipeBendParametersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var item = PipeBendParametersDataGrid.CurrentItem;
            if (item is not StarkCNC.Models.BendingData data)
                return;

            var value = NumberInputViewModel.ShowDialog();

            switch (PipeBendParametersDataGrid.CurrentColumn.DisplayIndex)
            {
                case 0:
                    data.StraightLength = value;
                    break;
                case 1:
                    data.BendingAngle = value;
                    break;
                case 2:
                    data.BendingRadius = value;
                    break;
                case 3:
                    data.RotationAngle = value;
                    break;
            }
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
