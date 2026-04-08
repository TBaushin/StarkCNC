using Microsoft.AspNetCore.Identity;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StarkCNC.Windows;

/// <summary>
/// Interaction logic for RegistrationWindow.xaml
/// </summary>
public partial class RegistrationWindow : Window
{
    private RegistrationWindowViewModel ViewModel;

    public RegistrationWindow(RegistrationWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var role = UserRole.SelectedItem as IdentityRole;
        var error = await ViewModel.RegisterNewUser(Login.Text, Password.Text, role).ConfigureAwait(true);

        if (string.IsNullOrEmpty(error))
            Close();

        ErrorsForm.Visibility = Visibility.Visible;
        ErrorsText.Content = error;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            RegisterButton_Click(new object(), new RoutedEventArgs());
    }
}
