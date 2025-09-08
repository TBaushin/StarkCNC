using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentSettingsView.xaml
    /// </summary>
    public partial class AdjustmentSettingsView : Page
    {
        private AdjustmentViewModel ViewModel;

        public AdjustmentSettingsView(AdjustmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
