using StarkCNC.Core.Models;
using StarkCNC.DTO;
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
        public AdjustmentParametersDto Adjustment { get; set; }

        public AdjustmentSettingsView(AdjustmentViewModel viewModel, AdjustmentParametersDto adjustment)
        {
            ViewModel = viewModel;
            Adjustment = adjustment;
            DataContext = this;

            if (ViewModel.SelectedAdjustment is null)
                ViewModel.SelectAdjustment(adjustment);

            InitializeComponent();

            Adjustment.PropertyChanged += SelectedAdjustment_PropertyChanged;
            SetVisibilityForRollingAndWindingStackPanels();
        }

        private void SelectedAdjustment_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var adjustment = sender as AdjustmentParametersDto;
            if (adjustment is null)
                return;

            if (e.PropertyName == "Type")
                SetVisibilityForRollingAndWindingStackPanels();
        }

        private void SetVisibilityForRollingAndWindingStackPanels()
        {
            if (Adjustment.Type == AdjustmentType.Winding)
            {
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (Adjustment.Type == AdjustmentType.Rolling)
            {
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Border_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.GoToCoordinateSettingsCommand.Execute(Adjustment);
        }

        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox is null)
                return;

            var value = NumberInputViewModel.ShowDialog();
            textBox.Text = value.ToString(CultureInfo.CurrentCulture);
        }

        private void EditAdjustmentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.EditAdjustmentCommand.Execute(Adjustment);
        }
    }
}
