using StarkCNC.DTO;
using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AdjustmentListView.xaml
/// </summary>
public partial class AdjustmentListView : Page
{
    private AdjustmentListViewModel? ViewModel;

    public AdjustmentListView()
    {
        InitializeComponent();

        if (DataContext is AdjustmentListViewModel viewModel)
            ViewModel = viewModel;
    }

    private void Label_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        UpdateViewModel();

        var label = sender as Label;
        if (label is null)
            return;

        var adjustment = label.DataContext as AdjustmentParametersVisibleDto;

        ViewModel?.EditAdjustmentCommand.Execute(adjustment);
    }

    private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        UpdateViewModel();

        var button = sender as Button;
        if (button is null)
            return;

        var adjustment = button.DataContext as AdjustmentParametersVisibleDto;

        ViewModel?.DeleteAdjustmentCommand.Execute(adjustment);
    }

    private void UpdateViewModel()
    {
        if (DataContext is AdjustmentListViewModel vm && ViewModel is not AdjustmentListViewModel)
            ViewModel = vm;
    }
}