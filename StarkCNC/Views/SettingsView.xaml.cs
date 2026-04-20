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

    private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        var tb = sender as TextBox;
        if (tb is null)
            return;

        ViewModel.Server = tb.Text;
        ViewModel.ServerReconnectCommand.Execute(null);
    }
}