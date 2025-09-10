using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "StarkCNC";

    [ObservableProperty]
    private ViewData? _selectedPage;

    private readonly INavigationService _navigationService;

    private readonly IServiceProvider _serviceProvider;

    private readonly IAdjustmentRepository _adjustmentRepository;

    [ObservableProperty]
    private bool _canNavigateBack;

    [ObservableProperty]
    private ObservableCollection<ViewData> _pages;

    private readonly SettingsView _settingsPage;
    private readonly UserView _userPage;

    [ObservableProperty]
    private string _status = string.Empty;

    public MainWindowViewModel(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        IStatusService statusService,
        IAdjustmentRepository adjustmentRepository) 
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _adjustmentRepository = adjustmentRepository;

        statusService.PropertyChanged += (_, _) => Status = statusService.Status;

        var AdjustmentPage = new ViewData(new AdjustmentView(_serviceProvider.GetRequiredService<AdjustmentViewModel>())) { IconGlyph = "\uE726" };
        AdjustmentVisibleElements(AdjustmentPage);
        _pages = [
            new ViewData(new ManualView(_serviceProvider.GetRequiredService<ManualViewModel>())) { IconGlyph = "\uE732" },
            new ViewData(new VisualizationView(_serviceProvider.GetRequiredService<VisualizationViewModel>())) { IconGlyph = "\uE726" },
            new ViewData(new ProgramView(_serviceProvider.GetRequiredService<ProgramViewModel>())) { IconGlyph = "\uE726" },
            AdjustmentPage
        ];

        _settingsPage = new SettingsView(_serviceProvider.GetRequiredService<SettingsViewModel>());
        _userPage = new UserView(_serviceProvider.GetRequiredService<UserViewModel>());
    }

    [RelayCommand]
    private void Back()
    {
        _navigationService.GoBack();
    }

    [RelayCommand]
    private void Forward()
    {
        _navigationService.GoForward();
    }

    [RelayCommand]
    private void GoSettings()
    {
        _navigationService.Navigate(_settingsPage);
    }

    [RelayCommand]
    private void GoUsers()
    {
        _navigationService.Navigate(_userPage);
    }

    public void UpdateCanNavigateBack()
    {
        CanNavigateBack = _navigationService.CanGoBack;
    }

    public ViewData? GetNavigationItem(string title)
    {
        return Pages.FirstOrDefault(e => e.Title == title);
    }

    private void AdjustmentVisibleElements(ViewData adjustmentPage)
    {
        var adjustmentsWithLevel = _adjustmentRepository
            .GetAdjustmentsWithLevel()
            .ToList();

        var adjustmentViewModel = _serviceProvider.GetRequiredService<AdjustmentViewModel>();
        adjustmentsWithLevel.ForEach(a =>
        {
            var adjustment = adjustmentViewModel.Adjustments
                .FirstOrDefault(e => e.Name == a.Name &&
                    e.PipeDiameter == a.PipeDiameter &&
                    e.Radius == a.Radius &&
                    e.InstalledLevel == a.InstalledLevel);

            if (adjustment is not null)
            {
                var adjustmentSettingPage = new AdjustmentSettingsView(adjustmentViewModel, adjustment);
                var viewData = new ViewData(adjustmentSettingPage) { Title = a.Name };

                var adjustmentCoordinateSettingsPage = new AdjustmentCoordinateSettingsView(adjustment);
                viewData.Items.Add(new ViewData(adjustmentCoordinateSettingsPage));

                adjustmentPage.Items.Add(viewData);
            }
        });
    }
}
