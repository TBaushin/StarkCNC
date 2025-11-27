using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly Visual3D _pipe;

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    [ObservableProperty]
    private double _pipeLength;

    [ObservableProperty]
    private double _ySetup;

    [ObservableProperty]
    private double _estimatedRemainingLength;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; set; } = new ObservableCollection<BendingDataViewModel>();

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
        if (!await SaveFile().ConfigureAwait(true))
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

            var firstItem = data.FirstOrDefault();
            if (firstItem is not null)
            {
                PipeLength = firstItem.PipeLength;
                YSetup = firstItem.YSetup;
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
            BendingDatas.Add(new BendingDataViewModel() { Id = 1, PipeLength = PipeLength, YSetup = YSetup });
        else
            BendingDatas.Add(new BendingDataViewModel() { Id = lastElement.Id + 1, PipeLength = PipeLength, YSetup = YSetup });
        UpdateBend();
    }

    [RelayCommand]
    private void SetSupplySpeedToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.SupplySpeed = firstItem.SupplySpeed;
        }
    }

    [RelayCommand]
    private void SetRotationSpeedToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.RotationSpeed = firstItem.RotationSpeed;
        }
    }

    [RelayCommand]
    private void SetBendSpeedToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.BendingAngleSpeed = firstItem.BendingAngleSpeed;
        }
    }

    [RelayCommand]
    private void SetBendCoefficientToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.BendingAngleCoefficient = firstItem.BendingAngleCoefficient;
        }
    }

    [RelayCommand]
    private void SetRadiusModeToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.BendingRadiusMode = firstItem.BendingRadiusMode;
        }
    }

    [RelayCommand]
    private void SetOffsetSpeedToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.OffsetSpeed = firstItem.OffsetSpeed;
        }
    }

    [RelayCommand]
    private void SetOffsetCoefficientToAll()
    {
        var firstItem = BendingDatas.FirstOrDefault();
        if (firstItem is null)
            return;

        foreach (var item in BendingDatas)
        {
            item.OffsetCoefficient = firstItem.OffsetCoefficient;
        }
    }

    [RelayCommand]
    private void UpdateEstimatedRemainingLength()
    {
        if (PipeLength > 0)
        {
            double result = PipeLength;
            foreach (var data in BendingDatas)
            {
                result -= data.Supply + (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
            }

            EstimatedRemainingLength = result;
        }
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
                PipeLength = data.PipeLength,
                YSetup = data.YSetup,
                Supply = data.Supply,
                SupplySpeed = data.SupplySpeed,
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