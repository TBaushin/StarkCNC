using StarkCNC.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainWindow = new MainWindow(new MainWindowViewModel());
            MainWindow.Visibility = Visibility.Visible;
            Run();
        }
    }
}
