using StarkCNC.Core.Services;
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
        private ISettingsService _settingsService;

        private AdjustmentViewModel ViewModel { get; set; }
        public AdjustmentParametersDto Adjustment { get; set; }

        public AdjustmentCoordinateSettingsView(
            ISettingsService settings,
            AdjustmentViewModel viewModel,
            AdjustmentParametersDto adjustment)
        {
            _settingsService = settings;
            Adjustment = adjustment;
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            _settingsService.PropertyChanged += Settings_PropertyChanged;
            ShowOrHideElements();
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ShowOrHideElements();
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            ViewModel.GoToEditParametersSettingsCommand
                .Execute(btn.Name.Replace("EditButton", "", StringComparison.CurrentCulture));
        }

        private void ShowOrHideElements()
        {

            if (_settingsService.IsElectricMachine)
            {
                Squeeze.Visibility = System.Windows.Visibility.Visible;
                Clamp.Visibility = System.Windows.Visibility.Visible;
                Press.Visibility = System.Windows.Visibility.Visible;
                Dorn.Visibility = System.Windows.Visibility.Visible;
                Lift.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Squeeze.Visibility = System.Windows.Visibility.Collapsed;
                Clamp.Visibility = System.Windows.Visibility.Collapsed;
                Press.Visibility = System.Windows.Visibility.Collapsed;
                Dorn.Visibility = System.Windows.Visibility.Collapsed;
                Lift.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
