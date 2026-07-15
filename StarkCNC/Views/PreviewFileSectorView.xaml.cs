using StarkCNC.Controls;
using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for PreviewFileSectorView.xaml
/// </summary>
public partial class PreviewFileSectorView : Page
{
    private PreviewFileSectorViewModel viewModel;

    public PreviewFileSectorView(PreviewFileSectorViewModel vm)
    {
        viewModel = vm;
        DataContext = viewModel;

        InitializeComponent();
    }

    private async void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (viewModel.SelectedItem is not null)
            await ICommandControl.ExecuteCommand(viewModel.SelectedItem.Command).ConfigureAwait(true);
    }
}
