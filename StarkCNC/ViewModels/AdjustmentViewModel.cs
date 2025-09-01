using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Repository;
using StarkCNC.Models;
using StarkCNC.Services;
using StarkCNC.Views;
using System.Collections.ObjectModel;

namespace StarkCNC.ViewModels
{
    public partial class AdjustmentViewModel : ObservableObject
    {
        private readonly AdjustmentListView _adjustmentListPage;
        private readonly AdjustmentSettingsView _adjustmentSettingsView;

        private readonly INavigationService _navigationService;
        private readonly IAdjustmentRepository _repository;

        private int _skiped = 0;

        public ObservableCollection<AdjustmentParameters> Adjustments { get; set; } = new ObservableCollection<AdjustmentParameters>();

        public List<string> AdjustmentsString { get; } = new List<string> { Localization.Language.Winding, Localization.Language.Rolling };

        [ObservableProperty]
        private AdjustmentParameters? _selectedAdjustment;

        public AdjustmentViewModel(INavigationService navigationService, IAdjustmentRepository adjustmentRepository) 
        {
            _adjustmentListPage = new AdjustmentListView(this);
            _adjustmentSettingsView = new AdjustmentSettingsView(this);

            _navigationService = navigationService;
            _repository = adjustmentRepository;

            foreach (var item in _repository.GetAll())
            {
                Adjustments.Add(new AdjustmentParameters(item));
            }
        }

        [RelayCommand]
        private void CreateAdjustment()
        {
            var item = new StarkCNC.Core.Models.AdjustmentParameters("");
            _repository.AddElement(item);

            var adjustment = new AdjustmentParameters(item);
            Adjustments.Add(adjustment);

            SelectedAdjustment = adjustment;
            var settingsWindow = new AdjustmentSettingsWindow("Добавление новой оснастки");
            settingsWindow.ShowDialog();
            //_navigationService.Navigate(_adjustmentSettingsView);
        }

        [RelayCommand]
        private void DeleteAdjustment(AdjustmentParameters adjustment)
        {
            _repository.RemoveElement(adjustment.Cast());
            Adjustments.Remove(adjustment);
        }

        [RelayCommand]
        private void EditAdjustment(AdjustmentParameters adjustment)
        {
            SelectedAdjustment = adjustment;

            var settingsWindow = new AdjustmentSettingsWindow("Изменение оснастки");
            settingsWindow.ShowDialog();
        }

        [RelayCommand]
        private void SaveAdjustment()
        {
            SelectedAdjustment = null;
            _navigationService.Navigate(_adjustmentListPage);
        }

        [RelayCommand]
        private void GoToAdjustmentList()
        {
            _navigationService.Navigate(_adjustmentListPage);
        }
    }
}
