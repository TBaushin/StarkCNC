using StarkCNC.DTO;
using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentCoordinateSettingsView.xaml
    /// </summary>
    public partial class AdjustmentCoordinateSettingsView : Page
    {
        private AdjustmentViewModel ViewModel { get; set; }
        public AdjustmentParametersDto Adjustment { get; set; }

        public AdjustmentCoordinateSettingsView(AdjustmentViewModel viewModel, AdjustmentParametersDto adjustment)
        {
            Adjustment = adjustment;
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            ViewModel.GoToEditParametersSettingsCommand
                .Execute(btn.Name.Replace("EditButton", "", StringComparison.CurrentCulture));
        }
    }
}
