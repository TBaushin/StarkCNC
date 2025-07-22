using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Core.Services;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "StarkCNC";

        [ObservableProperty]
        private ViewData? _selectedPage;

        private readonly INavigationService _navigationService;

        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private bool _canNavigateBack;

        [ObservableProperty]
        private ObservableCollection<ViewData> _pages;

        [ObservableProperty]
        private string _status = string.Empty;

        public MainWindowViewModel(IServiceProvider serviceProvider, INavigationService navigationService, IStatusService statusService) 
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;

            statusService.PropertyChanged += (_, _) => Status = statusService.Status;

            _pages = [
               new ViewData(new ManualView(_serviceProvider.GetRequiredService<ManualViewModel>())) { IconGlyph = "\uE726" },
                new ViewData(new VisualizationView(_serviceProvider.GetRequiredService<VisualizationViewModel>())) { IconGlyph = "\uE726" },
                new ViewData(new ProgramView(_serviceProvider.GetRequiredService<ProgramViewModel>())) { IconGlyph = "\uE726" },
                new ViewData (new AdjustmentView(_serviceProvider.GetRequiredService<AdjustmentViewModel>())) { IconGlyph = "\uE726" }
            ];
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
            _navigationService.Navigate(new SettingsView());
        }

        [RelayCommand]
        private void GoUsers()
        {
            _navigationService.Navigate(new UserView(_serviceProvider.GetRequiredService<UserViewModel>()));
        }

        public void UpdateCanNavigateBack()
        {
            CanNavigateBack = _navigationService.CanGoBack;
        }

        public ViewData? GetNavigationItem(string title)
        {
            return Pages.FirstOrDefault(e => e.Title == title);
        }
    }
}
