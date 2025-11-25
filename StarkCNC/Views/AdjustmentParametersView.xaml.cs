using StarkCNC.Core.Models;
using StarkCNC.DTO;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentParametersView.xaml
    /// </summary>
    public partial class AdjustmentParametersView : Page
    {
        private AdjustmentParametersViewModel? ViewModel;

        public AdjustmentParametersView()
        {
            InitializeComponent();

            if (DataContext is AdjustmentParametersViewModel viewModel)
                ViewModel = viewModel;

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
            if (ViewModel is null)
                return;

            if (ViewModel.AdjustmentType == AdjustmentType.Winding)
            {
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (ViewModel.AdjustmentType == AdjustmentType.Rolling)
            {
                RollingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Visible;
                WindingAdjustmentTypeStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Border_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel?.GoToSettingCoordinatesCommand.Execute(null);
        }

        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox is null)
                return;

            var value = NumberInputViewModel.ShowDialog();
            textBox.Text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}