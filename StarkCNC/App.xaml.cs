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
                services.AddDbContext<AppJsonContext>(opt => opt.UseInMemoryDatabase("StarkCNC"));
                services.AddSingleton<INavigationService, NavigationService>();
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
        host.Start();

        LiveCharts.Configure(c =>
        {
            c.AddDarkTheme();
        });

        InitializeComponent();
        MainWindow = host.Services.GetRequiredService<MainWindow>();
        MainWindow.Visibility = Visibility.Visible;
    }
}