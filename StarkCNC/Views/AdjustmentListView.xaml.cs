using StarkCNC.Models;
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

        var adjustment = label.DataContext as AdjustmentParameters;
        if (adjustment is null)
            return;

        ViewModel.EditAdjustmentCommand.Execute(adjustment);
    }
}
