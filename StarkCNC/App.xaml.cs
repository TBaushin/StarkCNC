using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.IO;
using System.Windows;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<ProgramViewModel>();
                services.AddSingleton<IBendingModelsLoadingService, BendingModelsLoadingService>();
            })
            .Build();

        public static IConfiguration Configuration { get; private set; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        public App()
        {
            _host.Start();

            InitializeComponent();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Visibility = Visibility.Visible;
            Run();
        }
    }
}
