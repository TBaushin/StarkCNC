using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Opc.Ua;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.DTO;
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
    private readonly Visual3D _pipe;

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; set; } = new ObservableCollection<BendingDataViewModel>();
    public ObservableCollection<string> BendModeList = new ObservableCollection<string>() { "Hello", "World" };

    public Visual3D Pipe
    {
        get => _pipe;
    }

    public ProgramViewModel(IBendingModelsLoadingService bendingModelsLoadingService, IGCodeService gCodeService)
    {

        _bendingModelsLoadingService = bendingModelsLoadingService;
        _gCodeService = gCodeService;

        //App.ServiceProvider.GetRequiredService<AdjustmentViewModel>().PropertyChanging += (sender, args) => UpdateBend();
        _bendingModelsLoadingService.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IBendingModelsLoadingService.Carriage))
            {
                UpdateBend();
            }
        };

        _pipe = _bendingModelsLoadingService.Pipe;
    }

    [RelayCommand]
    private async Task CreateNewFile()
    {
        if(!await SaveFile().ConfigureAwait(true))
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

        try
        {
            var data = await _gCodeService
                .ReadAsync(CurrentFilePath)
                .ConfigureAwait(true);

            int i = 1;
            foreach (var item in data)
            {
                BendingDatas.Add(new BendingDataViewModel(i, item));
                i++;
            }
        }
        catch (Exception)
        {
            // Ignore
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

        await _gCodeService.SaveAsync(CurrentFilePath, CastToModel()).ConfigureAwait(false);
        return true;
    }

    [RelayCommand]
    private void AddBendingData()
    {
        if (BendingDatas.Count == 0)
        {
            BendingDatas.Add(new BendingDataViewModel() { Id = 1 });
            return;
        }

        var lastElement = BendingDatas.Last();
        if (lastElement is null)
            BendingDatas.Add(new BendingDataViewModel() { Id = 1 });
        else
            BendingDatas.Add(new BendingDataViewModel() { Id = lastElement.Id + 1 });
        UpdateBend();
    }

    public void UpdateBend()
    {
        double pipeDiameter = 50;
        pipeDiameter = BendingDatas.Count > 0 ? pipeDiameter : 5;

        _bendingModelsLoadingService
            .UpdatePipeBend(WireBuilder.BuildWirePath(CastToModel(), pipeDiameter), pipeDiameter);
    }

    private ICollection<BendingData> CastToModel()
    {
        var result = new List<BendingData>();
        foreach (var data in BendingDatas)
        {
            result.Add(new BendingData()
            {
                StraightLength = data.StraightLength,
                StraightSpeed = data.StraightSpeed,
                Offset = data.Offset,
                OffsetSpeed = data.OffsetSpeed,
                OffsetCoefficient = data.OffsetCoefficient,
                BendingAngle = data.BendingAngle,
                BendingAngleSpeed = data.BendingAngleSpeed,
                BendingAngleCoefficient = data.BendingAngleCoefficient,
                BendingRadius = data.BendingRadius,
                BendingRadiusMode = data.BendingRadiusMode,
                RotationAngle = data.RotationAngle,
                RotationSpeed = data.RotationSpeed
            });
        }

        return result;
    }
}