using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarkCNC._3DViewer.Services;
using StarkCNC._3DViewer.ViewModels;
using StarkCNC._3DViewer.Views;
using StarkCNC.Controls;
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
        public static IConfiguration Configuration { get; private set; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        public App()
        {
            IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IConfiguration>(App.Configuration);
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<ProgramViewModel>();
                services.AddSingleton<ProgramControllerView>();
                services.AddSingleton<ProgramControllerViewModel>();
                services.AddSingleton<FlyoutMenuControl>();
                services.AddSingleton<IBendingModelsLoadingService, BendingModelsLoadingService>();
            })
            .Build();
            host.Start();

            InitializeComponent();
            MainWindow = host.Services.GetRequiredService<MainWindow>();
            MainWindow.Visibility = Visibility.Visible;
            Run();
        }
    }
}
