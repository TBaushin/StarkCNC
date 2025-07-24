using StarkCNC.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for NumberInputBlockWindow.xaml
    /// </summary>
    public partial class NumberInputBlockWindow : Window
    {
        NumberInputViewModel ViewModel;

        public NumberInputBlockWindow(NumberInputViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            var content = button.Content;
            if (content is null)
                return;

            var number = content.ToString() ?? string.Empty;
            ViewModel.AddNumber(number);
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
