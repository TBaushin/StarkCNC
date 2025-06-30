using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC._3DViewer.Services;
using StarkCNC._3DViewer.Views;
using StarkCNC.Core.Services;
using StarkCNC.Helpers;
using StarkCNC.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace StarkCNC.ViewModels
{
    public partial class ProgramViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private IGCodeService _gCodeService;
        private BendingDataConverter _bendingConverter = new BendingDataConverter();
        private PositionConverter _positionConverter = new PositionConverter();

        private string _gcodeExtension = ".gcode";
        private string _gcodeFilter = "GCode (.gc, .g, .gcode, .txt)|*.gc;*.g;*.gcode;*.txt;";

        [ObservableProperty]
        private string _currentFilePath;

        public ObservableCollection<BendingData> BendingDatas { get; set; } = new ObservableCollection<BendingData>();

        public IAsyncRelayCommand CreateNewFileCommand { get; }
        public IAsyncRelayCommand OpenFileCommand { get; }
        public IAsyncRelayCommand SaveFileCommand { get; }

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

            CreateNewFileCommand = new AsyncRelayCommand(CreateNewFile);
            OpenFileCommand = new AsyncRelayCommand(OpenFile);
            SaveFileCommand = new AsyncRelayCommand(SaveFile);
        }

        public ProgramControllerView GetProgramControllerView()
        {
            return _serviceProvider.GetRequiredService<ProgramControllerView>();
        }

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
            {
                var bendingData = _bendingConverter.Convert(item, typeof(BendingData), null, CultureInfo.CurrentCulture) as BendingData;

                if (bendingData is null)
                    continue;

                BendingDatas.Add(bendingData);
            }

            UpdateBend();
        }

        private async Task<bool> SaveFile()
        {
            if (CurrentFilePath is null)
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

            var data = new List<StarkCNC.Core.Models.BendingData>();
            foreach (var item in BendingDatas)
            {
                var bendingData = _bendingConverter
                    .Convert(item, typeof(StarkCNC.Core.Models.BendingData), null, CultureInfo.CurrentCulture) as StarkCNC.Core.Models.BendingData;

                if (bendingData is null)
                    continue;
                
                data.Add(bendingData);
            }

            await _gCodeService.SaveAsync(CurrentFilePath, data);
            return true;
        }

        public void UpdateBend()
        {
            foreach (var bendingData in BendingDatas)
            {
                var data = _bendingConverter.Convert(bendingData, typeof(StarkCNC.Core.Models.BendingData), null, CultureInfo.CurrentCulture) as StarkCNC.Core.Models.BendingData;
                
                if (data is null)
                    continue;

                var modelsLoadingService = _serviceProvider.GetRequiredService<IBendingModelsLoadingService>();

                var bendCalculation = new StarkCNC.Core.Calculations.BendCalculation(data);

                double carriagePosition = 1000;
                var carriageCoordinates = modelsLoadingService.GetModelPosition(ModelType.Carriage);
                if (carriageCoordinates is not null)
                {
                    carriagePosition = carriageCoordinates.PositionY;
                }

                var pipeDiameter = _serviceProvider.GetRequiredService<AdjustmentViewModel>().Parameters.PipeDiameter;

                var positions = bendCalculation.CalculateBend(pipeDiameter, carriagePosition);

                var resultPositions = new List<StarkCNC._3DViewer.Models.BendPositions>();
                foreach (var item in positions)
                {
                    var value = _positionConverter
                        .Convert(item, typeof(StarkCNC._3DViewer.Models.BendPositions), null, CultureInfo.CurrentCulture) as StarkCNC._3DViewer.Models.BendPositions;
                    
                    if (value is null)
                        continue;

                    resultPositions.Add(value);
                }

                modelsLoadingService.UpdatePipeBend(resultPositions);
            }
        }
    }
}
