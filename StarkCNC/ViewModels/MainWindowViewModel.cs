using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private bool _canNavigateBack;

        public ObservableCollection<Page> Pages { get; set; }

        public MainWindowViewModel(INavigationService navigationService) {
            _navigationService = navigationService;

            Pages = [
                new ManualView(),
                new ProgramView(),
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
    }
}
