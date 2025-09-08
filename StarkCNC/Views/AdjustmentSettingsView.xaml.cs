using StarkCNC.Models;
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

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var selectedAdjustment = ViewModel.SelectedAdjustment;
            if (selectedAdjustment is not null)
            {
                selectedAdjustment.PropertyChanged += SelectedAdjustment_PropertyChanged;

                SetVisibilityForRollingAndWindingStackPanels(selectedAdjustment);
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAdjustment")
            {
                var selectedAdjustment = ViewModel.SelectedAdjustment;
                if (selectedAdjustment is not null)
                    SetVisibilityForRollingAndWindingStackPanels(selectedAdjustment);
            }
        }

        private void SelectedAdjustment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var adjustment = sender as AdjustmentParameters;
            if (adjustment is null)
                return;

            if (e.PropertyName == "Type")
                SetVisibilityForRollingAndWindingStackPanels(adjustment);
        }

        private void SetVisibilityForRollingAndWindingStackPanels(AdjustmentParameters adjustment)
        {
            if (adjustment.Type.Name == "Намоткой")
            {
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (adjustment.Type.Name == "Прокатная")
            {
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
