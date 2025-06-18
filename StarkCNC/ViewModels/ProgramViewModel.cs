using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using StarkCNC._3DViewer.Views;
using StarkCNC.Core.Services;
using StarkCNC.Helpers;
using StarkCNC.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace StarkCNC.ViewModels
{
    public partial class ProgramViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private IGCodeService _gCodeService;
        private BendingDataConverter _bendingConverter = new BendingDataConverter();

        private string _gcodeExtension = ".gcode";
        private string _gcodeFilter = "GCode (.gc, .g, .gcode, .txt)|*.gc;*.g;*.gcode;*.txt;";

        [ObservableProperty]
        private string _currentFilePath;

        public ObservableCollection<BendingData> BendingDatas { get; set; } = new ObservableCollection<BendingData>();

        public ProgramViewModel(IServiceProvider serviceProvider, IGCodeService gCodeService)
        {
            _serviceProvider = serviceProvider;
            _gCodeService = gCodeService;
        }

        public ProgramControllerView GetProgramControllerView()
        {
            return _serviceProvider.GetRequiredService<ProgramControllerView>();
        }

        [RelayCommand]
        public async Task CreateNewFile()
        {
            await SaveFile();

            var dialog = new SaveFileDialog();
            dialog.DefaultExt = _gcodeExtension;
            dialog.Filter = _gcodeFilter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                CurrentFilePath = dialog.FileName;
            else
                return;

            BendingDatas.Clear();
        }

        [RelayCommand]
        public async Task OpenFile()
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
        }

        [RelayCommand]
        public async Task SaveFile()
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
                    return;
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
        }
    }
}
