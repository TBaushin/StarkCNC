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

namespace StarkCNC.ViewModels;

public partial class ProgramViewModel : ObservableObject
{
    private readonly IBendingModelsLoadingService _bendingModelsLoadingService;
    private readonly IGCodeService _gCodeService;
   
    private const string _gcodeExtension = ".gcode";
    private const string _gcodeFilter = "GCode (.gc, .g, .gcode, .txt)|*.gc;*.g;*.gcode;*.txt;";

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    public ObservableCollection<BendingData> BendingDatas { get; set; } = new ObservableCollection<BendingData>();

    public ProgramViewModel(IServiceProvider serviceProvider, IBendingModelsLoadingService bendingModelsLoadingService, IGCodeService gCodeService)
    {

        _bendingModelsLoadingService = bendingModelsLoadingService;
        _gCodeService = gCodeService;

        serviceProvider.GetRequiredService<AdjustmentViewModel>().PropertyChanging += (sender, args) => UpdateBend();
        _bendingModelsLoadingService.PropertyChanged += (sender, args) =>
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
        if(!await SaveFile().ConfigureAwait(false))
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

        var data = await _gCodeService.ReadAsync(CurrentFilePath).ConfigureAwait(false);
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

        await _gCodeService.SaveAsync(CurrentFilePath, BendingDatas).ConfigureAwait(false);
        return true;
    }

    public Visual3D GetPipe()
    {
        return _bendingModelsLoadingService.Pipe;
    }

    public void UpdateBend()
    {
        double pipeDiameter = 50;
        pipeDiameter = BendingDatas.Count > 0 ? pipeDiameter : 5;

        _bendingModelsLoadingService.UpdatePipeBend(WireBuilder.BuildWirePath(BendingDatas, pipeDiameter), pipeDiameter);
    }
}
