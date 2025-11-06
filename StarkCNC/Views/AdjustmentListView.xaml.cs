using StarkCNC.DTO;
using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AdjustmentListView.xaml
/// </summary>
public partial class AdjustmentListView : Page
{
    AdjustmentViewModel ViewModel;

    public AdjustmentListView(AdjustmentViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
    }

    private void Label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var label = sender as Label;
        if (label is null)
            return;

        var adjustment = label.DataContext as AdjustmentParametersDto;
        if (adjustment is null)
            return;

        ViewModel.GoToEditSettingsCommand.Execute(adjustment);
    }

    private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button is null)
            return;

        var adjustment = button.DataContext as AdjustmentParametersDto;
        if (adjustment is null)
            return;

        ViewModel.DeleteAdjustmentCommand.Execute(adjustment);
    }
}