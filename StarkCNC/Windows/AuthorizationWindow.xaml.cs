using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StarkCNC.Windows;

/// <summary>
/// Interaction logic for AuthorizationWindow.xaml
/// </summary>
public partial class AuthorizationWindow : Window
{
    private UserViewModel ViewModel;

    public AuthorizationWindow(UserViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    private async void AuthButton_Click(object sender, RoutedEventArgs e)
    {
        bool saveSession = false;
        if (RememberAuth.IsChecked is bool remember)
            saveSession = remember;

        var error = await ViewModel.Authorization(Login.Text, Password.Password, saveSession).ConfigureAwait(true);
        if (string.IsNullOrEmpty(error))
            Close();

        ErrorsForm.Visibility = Visibility.Visible;
        ErrorsText.Content = error;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            AuthButton_Click(new object(), new RoutedEventArgs());
    }
}
