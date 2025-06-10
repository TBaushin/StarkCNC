using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Views;

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
        private ICollection<ViewData> _pages;

        public MainWindowViewModel(IServiceProvider serviceProvider, INavigationService navigationService) 
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;

            _pages = [
                new ViewData(new ManualView()) { IconGlyph = "\uE726" },
                new ViewData(new ProgramView(_serviceProvider.GetRequiredService(typeof(ProgramViewModel)) as ProgramViewModel)) { IconGlyph = "\uE726" },
                new ViewData(new VisualizationView()) { IconGlyph = "\uE726" },
                new ViewData (new AdjustmentView()) { IconGlyph = "\uE726" }
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

        public void UpdateCanNavigateBack()
        {
            CanNavigateBack = _navigationService.CanGoBack;
        }

        public ViewData? GetNavigationItem(string title)
        {
            foreach (var item in Pages)
            {
                if (item.Title == title)
                    return item;
            }

            return null;
        }
    }
}
