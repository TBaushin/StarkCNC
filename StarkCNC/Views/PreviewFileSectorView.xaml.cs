using HelixToolkit.Wpf.SharpDX;
using StarkCNC.Controls;
using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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

        viewModel.PropertyChanged += ViewModel_PropertyChanged;

        InitializeComponent();
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.Pipe))
        {
            var camera = BendingView.Camera;
            if (camera is not null && viewModel.Pipe is not null)
                camera.ZoomExtents(BendingView, viewModel.Pipe.Bounds);
        }
    }

    private async void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (viewModel.SelectedItem is not null)
            await ICommandControl.ExecuteCommand(viewModel.SelectedItem.Command).ConfigureAwait(true);
    }
}
