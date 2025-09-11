using StarkCNC.Models;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentSettingsView.xaml
    /// </summary>
    public partial class AdjustmentSettingsView : Page
    {
        private AdjustmentViewModel ViewModel;
        public AdjustmentParameters Adjustment { get; set; }

        public AdjustmentSettingsView(AdjustmentViewModel viewModel, AdjustmentParameters adjustment)
        {
            ViewModel = viewModel;
            Adjustment = adjustment;
            DataContext = this;

            InitializeComponent();

            Adjustment.PropertyChanged += SelectedAdjustment_PropertyChanged;
            SetVisibilityForRollingAndWindingStackPanels();
        }

        private void SelectedAdjustment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var adjustment = sender as AdjustmentParameters;
            if (adjustment is null)
                return;

            if (e.PropertyName == "Type")
                SetVisibilityForRollingAndWindingStackPanels();
        }

        private void SetVisibilityForRollingAndWindingStackPanels()
        {
            if (Adjustment.Type.Name == "Намоткой")
            {
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (Adjustment.Type.Name == "Прокатная")
            {
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Border_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.GoToCoordinateSettingsCommand.Execute(null);
        }

        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox is null)
                return;

            var value = NumberInputViewModel.ShowDialog();
            textBox.Text = value.ToString(CultureInfo.CurrentCulture);
        }
    }
}
