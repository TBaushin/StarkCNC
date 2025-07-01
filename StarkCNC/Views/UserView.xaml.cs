using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Page
    {
        private UserViewModel ViewModel;

        public UserView(UserViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            SaveButton.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            EditButton.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var command = (sender as Button)?.Command;
            var commandParameter = (sender as Button)?.CommandParameter;
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }

            EditButton.Focus();
        }
    }
}
