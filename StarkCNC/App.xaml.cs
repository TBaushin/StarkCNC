using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarkCNC.Controls;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Database;
using StarkCNC.Exceptions;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Repository;
using StarkCNC.Services;
using StarkCNC.UoW;
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

#if !DEBUG
        GlobalExceptionHandler.StartHandling();
#endif

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
                services.AddTransient<AdjustmentParametersConstructor>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddTransient<AdjustmentViewModel>();
                services.AddSingleton<FlyoutMenuControl>();
                services.AddTransient<IBendingModelsLoadingService, BendingModelsLoadingService>();
                services.AddSingleton<IBendingDataUnitOfWork, BendingDataUnitOfWork>();
#if DEBUG
                Debug.WriteLine($"Подставился {nameof(FakeManualConfigurationService)}");
                services.AddSingleton<IManualConfigurationService, FakeManualConfigurationService>();
#else
                Debug.WriteLine($"Подставился {nameof(ManualConfigurationService)}");
                services.AddSingleton<IManualConfigurationService, ManualConfigurationService>();
#endif
                services.AddSingleton<IAdjustmentRepository, AdjustmentRepository>();
                services.AddSingleton<IUsersRepository, UsersRepository>();
            })
            .Build();

    private static void ConfigureRoutes(IRouter router) =>
        router.ConfigureRoutes(configure =>
        {
            configure.AddRoute("/manual", typeof(ManualViewModel), "Ручной режим", "\xEBFC"); // \
            configure.AddRoute("/visualization", typeof(VisualizationViewModel), "Визуализация", "\xE809"); // \xF158
            configure.AddRoute("/program", typeof(ProgramViewModel), "Программа", "\xF259");
            configure.AddRoute("/automatic", typeof(AutomaticViewModel), "Автомат", "\uF8A6");
            configure.AddRoute("/adjustment", typeof(AdjustmentViewModel), "Оснастка", "\uE835");
            configure.AddRoute("/adjustment/list", typeof(AdjustmentListViewModel), "Управление оснастками", "\uE8FD"); // \uEA37
            configure.AddRoute("/adjustment/edit", typeof(AdjustmentParametersViewModel), iconGlyph: "\uE90F"); // \uEC7A
            configure.AddRoute("/adjustment/edit/coordinates", typeof(AdjustmentParametersCoordinatesViewModel)); // \uEC7A \uF73D
            configure.AddRoute("/settings", typeof(SettingsViewModel), "Настройки", "\xE713");
            configure.AddRoute("/users", typeof(UserViewModel), iconGlyph: "\xE77B");
        });
}