using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.Core.UoW;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class ProgramViewModel : ObservableObject
{
    private readonly IBendingModelsLoadingService _bendingModelsLoadingService;
    private readonly IBendingDataUnitOfWork _unitOfWork;
   
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

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    public static IReadOnlyCollection<string> BendingModes { get; } = new List<string>()
    {
        "Режим гибки",
        "Гибка",
        "Пробивка 1",
        "Пробивка 2",
        "Перехват",
        "Спираль",
        "Гибка с уездом"
    };

    public Visual3D Pipe
    {
        get => _pipe;
    }

    public ProgramViewModel(IBendingModelsLoadingService bendingModelsLoadingService, IBendingDataUnitOfWork unitOfWork)
    {

        _bendingModelsLoadingService = bendingModelsLoadingService;
        _unitOfWork = unitOfWork;

        //App.ServiceProvider.GetRequiredService<AdjustmentViewModel>().PropertyChanging += (sender, args) => UpdateBend();
        _bendingModelsLoadingService.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IBendingModelsLoadingService.Carriage))
            {
                UpdateBend();
            }
        };

        _pipe = _bendingModelsLoadingService.Pipe;

        int i = 1;
        foreach (var item in _unitOfWork.BendingDatas)
        {
            BendingDatas.Add(new BendingDataViewModel(i, item));
            i++;
        }

        EstimatedRemainingLength = _unitOfWork.EstimatedRemainingLength;
        PipeLength = _unitOfWork.PipeLength;
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
            await _unitOfWork.ReadFileAsync(CurrentFilePath).ConfigureAwait(false);

            int i = 1;
            foreach (var item in _unitOfWork.BendingDatas)
            {
                BendingDatas.Add(new BendingDataViewModel(i, item));
                i++;
            }

            var firstItem = _unitOfWork.BendingDatas.FirstOrDefault();
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

        CastToModel();
        await _unitOfWork.WriteFileAsync(CurrentFilePath).ConfigureAwait(false);
        return true;
    }

    [RelayCommand]
    private void AddBendingData()
    {
        if (BendingDatas.Count == 0)
        {
            BendingDatas.Add(new BendingDataViewModel() { Id = 1 });
            CastToModel();
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

        CastToModel();
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

        CastToModel();
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

        CastToModel();
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

        CastToModel();
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

        CastToModel();
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

        CastToModel();
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

        CastToModel();
    }

    private void UpdateEstimatedRemainingLengthAndPipeLength()
    {
        if (PipeLength > 0)
        {
            EstimatedRemainingLength = _unitOfWork.CalculateEstimatedRemainingLength();
        }
        else if (PipeLength <= 0 && EstimatedRemainingLength > 0)
        {
            PipeLength = _unitOfWork.CalculatePipeLength();
        }
    }

    public void UpdateBend()
    {
        CastToModel();
        UpdateEstimatedRemainingLengthAndPipeLength();
        double pipeDiameter = 50;
        pipeDiameter = BendingDatas.Count > 0 ? pipeDiameter : 5;

        _bendingModelsLoadingService
            .UpdatePipeBend(WireBuilder.BuildWirePath(_unitOfWork.BendingDatas, pipeDiameter), pipeDiameter);
    }

    private void CastToModel()
    {
        _unitOfWork.BendingDatas.Clear();
        foreach (var data in BendingDatas)
        {
            _unitOfWork.BendingDatas.Add(new BendingData()
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
    }

    partial void OnEstimatedRemainingLengthChanged(double oldValue, double newValue)
    {
        _unitOfWork.EstimatedRemainingLength = newValue;
    }

    partial void OnPipeLengthChanged(double oldValue, double newValue)
    {
        _unitOfWork.PipeLength = newValue;
    }
}