using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels
{
    public partial class ProgramViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGCodeService _gCodeService;
       
        private readonly string _gcodeExtension = ".gcode";
        private readonly string _gcodeFilter = "GCode (.gc, .g, .gcode, .txt)|*.gc;*.g;*.gcode;*.txt;";

        [ObservableProperty]
        private string _currentFilePath = string.Empty;

        public ObservableCollection<BendingData> BendingDatas { get; set; } = new ObservableCollection<BendingData>();

        public ProgramViewModel(IServiceProvider serviceProvider, IGCodeService gCodeService)
        {
            
            _serviceProvider = serviceProvider;
            _gCodeService = gCodeService;

            _serviceProvider.GetRequiredService<AdjustmentViewModel>().PropertyChanging += (sender, args) => UpdateBend();
            _serviceProvider.GetRequiredService<IBendingModelsLoadingService>().PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(IBendingModelsLoadingService.Carriage))
                {
                    UpdateBend();
                }
            };
        }

        [RelayCommand]
        private async Task CreateNewFile()
        {
            if(!await SaveFile())
                return;

            var dialog = new SaveFileDialog();
            dialog.DefaultExt = _gcodeExtension;
            dialog.Filter = _gcodeFilter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                CurrentFilePath = dialog.FileName;
            else
                return;

            BendingDatas.Clear();
            UpdateBend();
        }

        [RelayCommand]
        private async Task OpenFile()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = _gcodeExtension;
            dialog.Filter = _gcodeFilter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                CurrentFilePath = dialog.FileName;
            else
                return;

            BendingDatas.Clear();

            var data = await _gCodeService.ReadAsync(CurrentFilePath);
            foreach (var item in data)
                BendingDatas.Add(item);

            UpdateBend();
        }

        [RelayCommand]
        private async Task<bool> SaveFile()
        {
            if (string.IsNullOrEmpty(CurrentFilePath))
            {
                var dialog = new SaveFileDialog();
                dialog.DefaultExt = _gcodeExtension;
                dialog.Filter = _gcodeFilter;

                bool? result = dialog.ShowDialog();

                if (result == true)
                    CurrentFilePath = dialog.FileName;
                else
                    return false;
            }

            await _gCodeService.SaveAsync(CurrentFilePath, BendingDatas);
            return true;
        }

        public Visual3D GetPipe()
        {
            return _serviceProvider.GetRequiredService<IBendingModelsLoadingService>().Pipe;
        }

        /*public void UpdateBend()
        {
            foreach (var bendingData in BendingDatas)
            {
                var modelsLoadingService = _serviceProvider.GetRequiredService<IBendingModelsLoadingService>();

                var bendCalculation = new StarkCNC.Core.Calculations.BendCalculation(bendingData);

                double carriagePosition = 1000;
                var carriageCoordinates = modelsLoadingService.GetModelPosition(ModelType.Carriage);
                if (carriageCoordinates is not null)
                {
                    carriagePosition = carriageCoordinates.PositionY;
                }

                var adjustment = _serviceProvider.GetRequiredService<AdjustmentViewModel>().SelectedAdjustment;
                double pipeDiameter = 50;
                if (adjustment is not null)
                    pipeDiameter = adjustment.PipeDiameter;

                var positions = bendCalculation.CalculateBend(pipeDiameter, carriagePosition);
                
                modelsLoadingService.UpdatePipeBend(positions);
            }
        }*/

        public void UpdateBend()
        {
            var modelsLoadingService = _serviceProvider.GetRequiredService<IBendingModelsLoadingService>();

            double pipeDiameter = 50;

            modelsLoadingService.UpdatePipeBend(WireBuilder.BuildWirePath(BendingDatas), BendingDatas.Count > 0 ? pipeDiameter : 5);
        }
    }
}
