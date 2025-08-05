using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC._3DViewer.Services;
using StarkCNC._3DViewer.Views;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Services;
using StarkCNC.Helpers;
using StarkCNC.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace StarkCNC.ViewModels
{
    public partial class ProgramViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGCodeService _gCodeService;
        private readonly BendingDataConverter _bendingConverter = new BendingDataConverter();
        private readonly PositionConverter _positionConverter = new PositionConverter();
       
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

        public ProgramControlView GetProgramControllerView()
        {
            return _serviceProvider.GetRequiredService<ProgramControlView>();
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
            {
                var bendingData = _bendingConverter.Convert(item, typeof(BendingData), null, CultureInfo.CurrentCulture) as BendingData;

                if (bendingData is null)
                    continue;

                BendingDatas.Add(bendingData);
            }

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
                /*   var data = _bendingConverter.Convert(bendingData, typeof(StarkCNC.Core.Models.BendingData), null, CultureInfo.CurrentCulture) as StarkCNC.Core.Models.BendingData;

                  if (data is null)
                      continue;

                  var modelsLoadingService = _serviceProvider.GetRequiredService<IBendingModelsLoadingService>();

                  var bendCalculation = new StarkCNC.Core.Calculations.WireBuilder(data);

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

                  var resultPositions = new List<StarkCNC._3DViewer.Models.BendPositions>();
                  foreach (var item in positions)
                  {
                      var value = _positionConverter
                          .Convert(item, typeof(StarkCNC._3DViewer.Models.BendPositions), null, CultureInfo.CurrentCulture) as StarkCNC._3DViewer.Models.BendPositions;

                      if (value is null)
                          continue;

                      resultPositions.Add(value);
                  }

                
                modelsLoadingService.UpdatePipeBend(resultPositions);  */
            }
        }
        }
}
