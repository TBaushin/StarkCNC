using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentCoordinateSettingsView.xaml
    /// </summary>
    public partial class AdjustmentCoordinateSettingsView : Page
    {
        private AdjustmentViewModel ViewModel;

        public AdjustmentCoordinateSettingsView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
