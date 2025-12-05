using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for AutomaticView.xaml
/// </summary>
public partial class AutomaticView : Page
{
    private AutomaticViewModel ViewModel;

    public AutomaticView(AutomaticViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

        ViewModel.SetAutomaticModeCommand.Execute(null);
    }
}
