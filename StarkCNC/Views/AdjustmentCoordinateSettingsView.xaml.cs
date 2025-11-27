using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentCoordinateSettingsView.xaml
    /// </summary>
    public partial class AdjustmentCoordinateSettingsView : Page
    {
        private AdjustmentParametersCoordinatesViewModel ViewModel;

        public AdjustmentCoordinateSettingsView(AdjustmentParametersCoordinatesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();

            ViewModel.PropertyChanged += Settings_PropertyChanged;

            ShowOrHideElements();
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AdjustmentParametersCoordinatesViewModel.IsElectricMachine))
                ShowOrHideElements();
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            var parameter = btn.Name.Replace("EditButton", "", System.StringComparison.CurrentCulture);

            ViewModel.EditParametersCommand.Execute(parameter);
        }

        private void ShowOrHideElements()
        {
            if (ViewModel.IsElectricMachine)
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