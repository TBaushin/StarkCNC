using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Windows.Controls;

namespace StarkCNC.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "StarkCNC";

        [ObservableProperty]
        private Page? _selectedPage;

        private readonly INavigationService _navigationService;

        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private bool _canNavigateBack;

        [ObservableProperty]
        private ICollection<Page> _pages;

        public MainWindowViewModel(IServiceProvider serviceProvider ,INavigationService navigationService) 
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;

            _pages = [
                new ManualView(),
                new ProgramView(_serviceProvider.GetRequiredService(typeof(ProgramViewModel)) as ProgramViewModel),
                new VisualizationView(),
                new AdjustmentView()
            ];
        }

        [RelayCommand]
        public void Back()
        {
            _navigationService.GoBack();
        }

        [RelayCommand]
        public void Forward()
        {
            _navigationService.GoForward();
        }

        [RelayCommand]
        public void GoSettings()
        {
            _navigationService.Navigate(new SettingsView());
        }

        public void UpdateCanNavigateBack()
        {
            CanNavigateBack = _navigationService.CanGoBack;
        }

        public IEnumerable<Page> GetNavigationItem()
        {
            IEnumerable<Page> items = new List<Page>();
            foreach (var item in Pages)
            {
                items.Append(item);
            }

            return items;
        }

        public Page? GetNavigationItem(string title)
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
