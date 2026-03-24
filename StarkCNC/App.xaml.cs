using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarkCNC.Controls;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Database;
using StarkCNC.Database.Seeders;
#if !DEBUG
using StarkCNC.Exceptions;
#endif
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
    private static DirectoryInfo _directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "secrets"));

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
        AppJsonContext.Initialize(Configuration);

        ConfigureRoutes(_host.Services.GetRequiredService<IRouter>());

        InitializeComponent();

#if !DEBUG
        GlobalExceptionHandler.StartHandling();
#endif

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Visibility = Visibility.Visible;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await IdentitySeeder.SeedRolesAsync(_host.Services.GetRequiredService<RoleManager<IdentityRole>>()).ConfigureAwait(true);
        await IdentitySeeder.SeedAdminAsync(_host.Services.GetRequiredService<UserManager<User>>()).ConfigureAwait(true);
    }

    private static IConfiguration ConfigureStartup() =>
        new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    private static IHost RegisterServices()
    {
        if (!_directory.Exists)
            _directory.Create();

        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=StarkCNC.sql"));
                // Настройка Identity
                services.AddIdentity<User, IdentityRole>(opt =>
                    {
                        opt.SignIn.RequireConfirmedAccount = false;
                        opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+абвгдёежзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ ";
                        opt.Password.RequireDigit = false;
                        opt.Password.RequireLowercase = false;
                        opt.Password.RequireUppercase = false;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequiredUniqueChars = 0;
                    })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
                services.AddHttpContextAccessor();
                services.AddDataProtection()
                    .PersistKeysToFileSystem(_directory)
                    .SetApplicationName("StarkCNC");

                services.AddDbContext<AppJsonContext>(opt => opt.UseInMemoryDatabase("StarkCNC"));
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IRouter, Router>();
                services.AddSingleton<IBreadcrumbService, BreadcrumbService>();
                services.AddSingleton<IStatusService, StatusService>();
                services.AddSingleton<ISettingsRepository, SettingsRepository>();
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
                services.AddSingleton<IAdjustmentService, AdjustmentService>();
                services.AddSingleton<IUserService, UserService>();
            })
            .Build();
    }

    private static void ConfigureRoutes(IRouter router) =>
        router.ConfigureRoutes(configure =>
        {
            configure.AddRoute("/manual", typeof(ManualViewModel), "Ручной режим", "\xEBFC"); // \
            configure.AddRoute("/visualization", typeof(VisualizationViewModel), "Визуализация", "\xE809"); // \xF158
            configure.AddRoute("/program", typeof(ProgramViewModel), "Программа", "\xF259");
            configure.AddRoute("/automatic", typeof(AutomaticViewModel), "Автомат", "\uF8A6");
            configure.AddRoute("/adjustment", typeof(AdjustmentViewModel), "Оснастка", "\uE835", new Roles[] { Roles.Service });
            configure.AddRoute("/adjustment/list", typeof(AdjustmentListViewModel), "Управление оснастками", "\uE8FD", new Roles[] { Roles.Service }); // \uEA37
            configure.AddRoute("/adjustment/list/edit", typeof(AdjustmentParametersViewModel), iconGlyph: "\uE90F", rolesHasAccess: new Roles[] { Roles.Service }); // \uEC7A
            configure.AddRoute("/adjustment/list/edit/coordinates", typeof(AdjustmentParametersCoordinatesViewModel), "Настройка координат", rolesHasAccess: new Roles[] { Roles.Service }); // \uEC7A \uF73D
            configure.AddRoute("/settings", typeof(SettingsViewModel), "Настройки", "\xE713", new Roles[] { Roles.Service, Roles.Administrator });
            configure.AddRoute("/users", typeof(UserViewModel), "Пользователи", iconGlyph: "\xE77B");
        });
}