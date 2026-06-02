using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using StarkCNC.Views;
using StarkCNC.Windows;
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

    public App() { }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        await _host.StartAsync().ConfigureAwait(true);

        await IdentitySeeder.SeedRolesAsync(_host.Services.GetRequiredService<RoleManager<IdentityRole>>()).ConfigureAwait(true);
        await IdentitySeeder.SeedAdminAsync(_host.Services.GetRequiredService<UserManager<User>>()).ConfigureAwait(true);

        LiveCharts.Configure(c =>
        {
            c.AddDarkTheme();
        });

        ViewLocator.Initialize(_host.Services);
        ConfigureRoutes(_host.Services.GetRequiredService<IRouter>());

#if !DEBUG
        GlobalExceptionHandler.StartHandling();
#endif
        var isAuthorized = await RunAuthorization(_host.Services.GetRequiredService<IUserService>()).ConfigureAwait(true);
        if (!isAuthorized)
            Shutdown();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        ShutdownMode = ShutdownMode.OnMainWindowClose;
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
                        opt.Password.RequiredLength = 0;
                    })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
                services.AddHttpContextAccessor();
                services.AddDataProtection()
                    .PersistKeysToFileSystem(_directory)
                    .SetApplicationName("StarkCNC");

                services.AddDbContextFactory<AppJsonContext>(opt => opt.UseInMemoryDatabase("StarkCNC"));
                services.AddSingleton<IRouter, Router>();
                services.AddSingleton<IBreadcrumbService, BreadcrumbService>();
                services.AddSingleton<IStatusService, StatusService>();
                services.AddSingleton<ISettingsRepository, SettingsRepository>();
                services.AddTransient<AdjustmentParametersConstructor>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddTransient<AdjustmentListView>();
                services.AddTransient<AdjustmentListViewModel>();
                services.AddTransient<AdjustmentView>();
                services.AddTransient<AdjustmentViewModel>();
                services.AddTransient<AutomaticView>();
                services.AddTransient<AutomaticViewModel>();
                services.AddTransient<ManualView>();
                services.AddTransient<ManualViewModel>();
                services.AddTransient<ProgramView>();
                services.AddTransient<ProgramViewModel>();
                services.AddTransient<VisualizationView>();
                services.AddTransient<VisualizationViewModel>();
                services.AddTransient<IMachineLoader, MachineLoader>();
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

    private static async Task<bool> RunAuthorization(IUserService userService)
    {
        if (userService.CurrentUser is not null)
            return true;

        var vm = await AuthorizationWindowViewModel.InitializeAsync(userService, true).ConfigureAwait(true);
        var authorization = new AuthorizationWindow(vm);
        authorization.ShowDialog();

        return userService.CurrentUser is not null;
    }
}