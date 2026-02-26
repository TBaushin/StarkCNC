using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

    private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null)
            return;

        border.Background = (Brush)FindResource("CardBackgroundFillColorSecondaryBrush");
    }

    private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null)
            return;

        border.Background = (Brush)FindResource("CardBackgroundFillColorDefaultBrush");
    }
}