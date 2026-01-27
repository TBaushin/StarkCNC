using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for ManualView.xaml
/// </summary>
public partial class ManualView : Page
{
    private readonly ManualViewModel ViewModel;

    public ManualView(ManualViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        IsVisibleChanged += ManualView_IsVisibleChanged;
    }

    private void SqueezeBackButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.FirstSqueeze.BackwardStartCommand.Execute(null);
    }

    private void SqueezeBackButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.FirstSqueeze.BackwardCancelCommand.Execute(null);
    }

    private void SqueezeForwardButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.FirstSqueeze.ForwardStartCommand.Execute(null);
    }

    private void SqueezeForwardButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.FirstSqueeze.ForwardCancelCommand.Execute(null);
    }

    private void BendAndSqueezeButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.BendAndSqueezeRunCommand.Execute(null);
    }

    private void BendAndSqueezeButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        ViewModel.BendAndSqueezeCancelCommand.Execute(null);
    }

    private void ManualView_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not bool value)
            return;

        if (value)
            ViewModel.ManualModeTurnOnCommand.Execute(null);
        else
            ViewModel.ManualModeTurnOffCommand.Execute(null);
    }
}