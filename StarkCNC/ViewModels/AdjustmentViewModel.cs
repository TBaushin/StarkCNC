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

        [ObservableProperty]
        private bool _canForward = false;

        [ObservableProperty]
        private bool _canBackward = false;

        public AdjustmentViewModel(INavigationService navigationService, IAdjustmentRepository adjustmentRepository) 
        {
            _adjustmentListPage = new AdjustmentListView(this);
            _adjustmentSettingsView = new AdjustmentSettingsView(this);

            _navigationService = navigationService;
            _repository = adjustmentRepository;

            UpdateForwardAndBackwardProperty();

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
            _navigationService.Navigate(_adjustmentSettingsView);
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
            _navigationService.Navigate(_adjustmentSettingsView);
        }

        [RelayCommand]
        private void SaveAdjustment()
        {
            SelectedAdjustment = null;
            _navigationService.Navigate(_adjustmentListPage);
        }

        [RelayCommand]
        private void GoNextPage()
        {
            _skiped += 10;
            var elements = _repository.GetTenElements(_skiped);

            Adjustments.Clear();
            foreach (var item in elements)
            {
                Adjustments.Add(new AdjustmentParameters(item));
            }

            UpdateForwardAndBackwardProperty();
        }

        [RelayCommand]
        private void GoPreviousPage()
        {
            _skiped = Math.Max(0, _skiped - 10);
            var elements = _repository.GetTenElements(_skiped);

            Adjustments.Clear();
            foreach (var item in elements)
            {
                Adjustments.Add(new AdjustmentParameters(item));
            }

            UpdateForwardAndBackwardProperty();
        }

        [RelayCommand]
        private void GoToAdjustmentList()
        {
            _navigationService.Navigate(_adjustmentListPage);
        }

        private void UpdateForwardAndBackwardProperty()
        {
            var total = _repository.Count();
            CanForward = total > _skiped + 10;
            CanBackward = _skiped >= 10;
        }
    }
}
