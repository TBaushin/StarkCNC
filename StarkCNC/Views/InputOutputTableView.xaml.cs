using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for InputOutputTableView.xaml
/// </summary>
public partial class InputOutputTableView : UserControl
{
    public InputOutputTableView()
    {
        InitializeComponent();
    }

    private void Page_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        var page = sender as Page;
        if (page is null)
            return;

        var viewModel = DataContext as InputOutputTableViewModel;
        if (viewModel is null)
            return;

        if (page.Visibility == System.Windows.Visibility.Collapsed)
            viewModel.Unsubscribe();
        else
            viewModel.Subscribe();
    }
}
