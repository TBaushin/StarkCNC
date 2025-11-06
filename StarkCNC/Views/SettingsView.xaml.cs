using StarkCNC.ViewModels;
using System.Windows.Controls;

namespace StarkCNC.Views;

/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : Page
{
    private SettingsViewModel ViewModel;

    public SettingsView(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
    }
}