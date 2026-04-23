using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AdjustmentParametersView.xaml
/// </summary>
public partial class AdjustmentParametersView : Page
{
    private AdjustmentParametersViewModel ViewModel;

    public AdjustmentParametersView(AdjustmentParametersViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        Loaded += async (_, _) => await ViewModel.InitializeAsync().ConfigureAwait(true);

        InitializeComponent();

        SetVisibilityForRollingAndWindingStackPanels();
    }

    private void SetVisibilityForRollingAndWindingStackPanels()
    {
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
        ViewModel.GoToSettingCoordinatesCommand.Execute(null);
    }

    private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox is null)
            return;

        var value = NumberInputViewModel.ShowDialog(textBox.Text);

        BindingExpression binding = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
        if (binding is not null)
        {
            textBox.Text = value.ToString(CultureInfo.InvariantCulture);
            binding.UpdateSource();
            binding.UpdateTarget();
        }
    }
}