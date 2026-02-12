using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.Core.UoW;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class ProgramViewModel : ObservableObject
{
    private readonly IBendingModelsLoadingService _bendingModelsLoadingService;
    private readonly IBendingDataUnitOfWork _unitOfWork;
   
    private readonly Visual3D _pipe;

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    [ObservableProperty]
    private double _pipeLength;

    [ObservableProperty]
    private double _ySetup;

    [ObservableProperty]
    private double _estimatedRemainingLength;

    [ObservableProperty]
    private BendingDataViewModel? _selectedBendingData;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    public static IReadOnlyCollection<string> BendingModes { get; } = new List<string>()
    {
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

        CurrentFilePath = _unitOfWork.CurrentFilePath;

        //App.ServiceProvider.GetRequiredService<AdjustmentViewModel>().PropertyChanging += (sender, args) => UpdateBend();
        _bendingModelsLoadingService.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(IBendingModelsLoadingService.Carriage))
            {
                UpdateBend();
            }
        };

        _pipe = _bendingModelsLoadingService.Pipe;
        BendingDatas.CollectionChanged += BendingDatas_CollectionChanged;

        foreach (var item in _unitOfWork.BendingDatas)
        {
            var bendingDataViewModel = new BendingDataViewModel(item);
            bendingDataViewModel.PropertyChanged += BendingDataViewModel_PropertyChanged;
            BendingDatas.Add(bendingDataViewModel);
        }

        EstimatedRemainingLength = _unitOfWork.EstimatedRemainingLength;
        PipeLength = _unitOfWork.PipeLength;
        YSetup = _unitOfWork.SetUpPoint;
    }

    [RelayCommand]
    private async Task CreateNewFile()
    {
        if (!await SaveFile().ConfigureAwait(true))
            return;

        await _unitOfWork.CreateNewFile().ConfigureAwait(true);

        foreach (var item in BendingDatas)
        {
            item.PropertyChanged -= BendingDataViewModel_PropertyChanged;
        }

        BendingDatas.Clear();
        UpdateBend();
    }

    [RelayCommand]
    private async Task OpenFile()
    {
        await _unitOfWork.OpenFile().ConfigureAwait(true);

        try
        {
            BendingDatas.Clear();
            foreach (var item in _unitOfWork.BendingDatas)
            {
                BendingDatas.Add(new BendingDataViewModel(item));
            }

            PipeLength = _unitOfWork.PipeLength;
            YSetup = _unitOfWork.SetUpPoint;
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
        var result = await _unitOfWork.SaveFile().ConfigureAwait(true);
        if (!result)
            return false;

        UpdateBend();
        return true;
    }

    [RelayCommand]
    private void AddBendingData()
    {
        if (BendingDatas.Count == 0)
        {
            BendingDatas.Add(new BendingDataViewModel() { Id = 1 });
            UpdateBend();
            return;
        }

        var lastElement = BendingDatas.Last();
        if (lastElement is null)
            BendingDatas.Add(new BendingDataViewModel() { Id = 1, PipeLength = PipeLength, YSetup = YSetup });
        else
            BendingDatas.Add(new BendingDataViewModel() { Id = lastElement.Id + 1, PipeLength = PipeLength, YSetup = YSetup });
        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
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

        UpdateBend();
        _unitOfWork.HasUnsavedData = false;
    }

    [RelayCommand]
    private void RemoveBendingData(BendingDataViewModel data)
    {
        data.PropertyChanged -= BendingDataViewModel_PropertyChanged;
        BendingDatas.Remove(data);
        UpdateBend();
        _unitOfWork.HasUnsavedData = true;
    }

    [RelayCommand]
    private void CurrentBendingDataToClipboard()
    {
        if (SelectedBendingData is null)
            return;

        var json = JsonSerializer.Serialize<BendingDataViewModel>(SelectedBendingData);
        Clipboard.SetData(DataFormats.Text, Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
    }

    [RelayCommand]
    private void PasteBendingDataFromClipboard()
    {
        var data = Clipboard.GetData(DataFormats.Text) as string;
        if (string.IsNullOrEmpty(data))
            return;
        try
        {
            var bytes = Convert.FromBase64String(data);
            if (bytes is null)
                return;

            var json = Encoding.UTF8.GetString(bytes);
            var result = JsonSerializer.Deserialize<BendingDataViewModel>(json);

            if (result is not BendingDataViewModel bd)
                return;

            if (SelectedBendingData is not null)
                BendingDatas.Insert(BendingDatas.IndexOf(SelectedBendingData) + 1, bd);
            else
                BendingDatas.Add(bd);

            UpdateBend();
            _unitOfWork.HasUnsavedData = true;
        }
        catch (FormatException)
        {
#if DEBUG
            Debug.WriteLine($"Ошибка форматирования из буфера обмена в {nameof(ProgramViewModel)} переменной {nameof(data)}, её содержимое: {data}");
#endif
        }
        catch (DecoderFallbackException)
        {
#if DEBUG
            Debug.WriteLine($"Ошибка преобразовывания в текст с кодировкой UTF8 из буфера обмена в {nameof(ProgramViewModel)} переменной {nameof(data)}, её содержимое: {data}");
#endif
        }
        catch (JsonException)
        {
#if DEBUG
            Debug.WriteLine($"Ошибка преобразования в json текста с кодировкой UTF8 из буфера обмена в {nameof(ProgramViewModel)} переменной {nameof(data)}, её содержимое: {data}");
#endif
        }
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

    private void BendingDatas_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        int i = 1;
        foreach (var item in BendingDatas)
        {
            item.Id = i;
            i++;
        }
        _unitOfWork.HasUnsavedData = true;
    }

    private void BendingDataViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnEstimatedRemainingLengthChanged(double oldValue, double newValue)
    {
        _unitOfWork.EstimatedRemainingLength = newValue;
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnPipeLengthChanged(double oldValue, double newValue)
    {
        _unitOfWork.PipeLength = newValue;
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnYSetupChanged(double oldValue, double newValue)
    {
        _unitOfWork.SetUpPoint = newValue;
        _unitOfWork.HasUnsavedData = true;
    }
}