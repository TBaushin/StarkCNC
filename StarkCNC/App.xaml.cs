using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarkCNC.Controls;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Database;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Repository;
using StarkCNC.Services;
using StarkCNC.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace StarkCNC;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static IHost _host = RegisterServices();

    public static IConfiguration Configuration { get; private set; } = ConfigureStartup();

    public App()
    {
        _host.Start();

        LiveCharts.Configure(c =>
        {
            c.AddDarkTheme();
        });

        ViewLocator.Initialize(_host.Services);

        ConfigureRoutes(_host.Services.GetRequiredService<IRouter>());

        InitializeComponent();
        
        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Visibility = Visibility.Visible;
    }

    private static IConfiguration ConfigureStartup() =>
        new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    private static IHost RegisterServices() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppJsonContext>(opt => opt.UseInMemoryDatabase("StarkCNC"));
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IRouter, Router>();
                services.AddSingleton<IBreadcrumbService, BreadcrumbService>();
                services.AddSingleton<IStatusService, StatusService>();
                services.AddSingleton<ISettingsRepository, SettingsRepository>();
                services.AddSingleton<IGCodeService, GCodeService>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<VisualizationViewModel>();
                services.AddSingleton<ProgramViewModel>();
                services.AddSingleton<ManualViewModel>();
                services.AddSingleton<AdjustmentViewModel>();
                services.AddSingleton<UserViewModel>();
                services.AddSingleton<FlyoutMenuControl>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<IBendingModelsLoadingService, BendingModelsLoadingService>();
#if DEBUG
                Debug.WriteLine($"Подставился {nameof(FakeManualConfigurationService)}");
                services.AddSingleton<IManualConfigurationService, FakeManualConfigurationService>();
#else
                Debug.WriteLine($"Подставился {nameof(ManualConfigurationService)}");
                services.AddSingleton<IManualConfigurationService, ManualConfigurationService>();
#endif
                services.AddSingleton<IAdjustmentRepository, AdjustmentRepository>();
            })
            .Build();

    private static void ConfigureRoutes(IRouter router) =>
        router.ConfigureRoutes(configure =>
        {
            configure.AddRoute("/manual", typeof(ManualViewModel));
            configure.AddRoute("/visualization", typeof(VisualizationViewModel));
            configure.AddRoute("/program", typeof(ProgramViewModel));
            configure.AddRoute("/adjustment", typeof(AdjustmentViewModel));
            configure.AddRoute("/adjustment/list", typeof(AdjustmentListViewModel));
            //configure.AddRoute("/adjustment/configure");
            //configure.AddRoute("/adjustment/configure/coordinates");
            //configure.AddRoute("/adjustment/first-level", typeof(AdjustmentViewModel));
            //configure.AddRoute("/adjustment/second-level", typeof(AdjustmentViewModel));
            //configure.AddRoute("/adjustment/third-level", typeof(AdjustmentViewModel));
            configure.AddRoute("/settings", typeof(SettingsViewModel));
            configure.AddRoute("/users", typeof(UserViewModel));
        });
}