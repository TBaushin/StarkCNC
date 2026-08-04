using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using SharpDX.Mathematics.Interop;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class ProgramViewModel : ViewModelBase
{
    private readonly IRouter _router;
    private readonly IBendingDataUnitOfWork _unitOfWork;
    private readonly ISettingsRepository _settingsRepository;
    private readonly IAdjustmentService _adjustmentService;

    public TextureModel? EnvironmentMap { get; }

    [ObservableProperty]
    private Element3D? _pipe;

    [ObservableProperty]
    private Point3D _modelCentroid = default;

    [ObservableProperty]
    private bool _renderEnvironmentMap = false;

    [ObservableProperty]
    private IEffectsManager _effectsManager;

    [ObservableProperty]
    private TextureModel? _textureModel;

    [ObservableProperty]
    private HelixToolkit.Wpf.SharpDX.Camera? _camera;

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    [ObservableProperty]
    private float _pipeLength;

    [ObservableProperty]
    private float _ySetup;

    [ObservableProperty]
    private float _estimatedRemainingLength;

    [ObservableProperty]
    private float _colletOffsetLength;

    [ObservableProperty]
    private BendingDataViewModel? _selectedBendingData;

    public ObservableCollection<BendingDataViewModel> BendingDatas { get; } = new ObservableCollection<BendingDataViewModel>();

    public static ICollection<string> BendingModes { get; } = new List<string>();

    public ProgramViewModel(
        IRouter router,
        IBendingDataUnitOfWork unitOfWork,
        ISettingsRepository settingsRepository,
        IAdjustmentService adjustmentService)
    {
        _router = router;
        _unitOfWork = unitOfWork;
        _settingsRepository = settingsRepository;
        _adjustmentService = adjustmentService;

        CurrentFilePath = _unitOfWork.CurrentFilePath;

        EffectsManager = new DefaultEffectsManager();

        var gradientStops = new SharpDX.Direct2D1.GradientStop[]
        {
            new SharpDX.Direct2D1.GradientStop { Color = new RawColor4(0, 0, 0, 1), Position = 0f },
            new SharpDX.Direct2D1.GradientStop { Color = new RawColor4(0, 0, 255, 1), Position = 1f }
        };
        var stream = BitmapExtensions.CreateLinearGradientBitmapStream(
            EffectsManager,
            256,
            256,
            Direct2DImageFormat.Bmp,
            new System.Numerics.Vector2(0, 0),
            new System.Numerics.Vector2(0, 256),
            gradientStops);
        if (stream is not null)
            TextureModel = TextureModel.Create(stream);

        Camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
        {
            LookDirection = new Vector3D(-7, -10, -10),
            Position = new Point3D(0, 0, 10),
            UpDirection = new Vector3D(0, 1, 0),
            FarPlaneDistance = 50000,
            NearPlaneDistance = 0.5f
        };

        BendingDatas.CollectionChanged += BendingDatas_CollectionChanged;

        foreach (var item in _unitOfWork.BendingDatas)
        {
            var bendingDataViewModel = new BendingDataViewModel(item);
            bendingDataViewModel.PropertyChanged += BendingDataViewModel_PropertyChanged;
            BendingDatas.Add(bendingDataViewModel);
        }

        _unitOfWork.HasUnsavedData = false;

        EstimatedRemainingLength = _unitOfWork.EstimatedRemainingLength;
        PipeLength = _unitOfWork.PipeLength;
        YSetup = _unitOfWork.SetUpPoint;
        ColletOffsetLength = _unitOfWork.ColletOffsetLength;
    }

    public async Task InitializeAsync()
    {
        await GenerateBendingModes().ConfigureAwait(true);
    }

    private async Task GenerateBendingModes()
    {
        BendingModes.Clear();
        var settings = await _settingsRepository.GetAsync().ConfigureAwait(true);

        BendingModes.Add("Гибка");

        if (settings is not null && settings.WithPunchingCylinder)
        {
            BendingModes.Add("Пробивка 1");
            BendingModes.Add("Пробивка 2");
        }

        BendingModes.Add("Перехват");

        if (_adjustmentService.FirstLevelAdjustment is not null && _adjustmentService.FirstLevelAdjustment.Type == AdjustmentType.Rolling) // TODO: AdjustmentService должен иметь CurrentAdjustment!
            BendingModes.Add("Спираль");

        BendingModes.Add("Гибка с уездом");
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
        _router.Navigate("/file-selector");
        //await _unitOfWork.OpenFile().ConfigureAwait(true);

        //try
        //{
        //    BendingDatas.Clear();
        //    foreach (var item in _unitOfWork.BendingDatas)
        //    {
        //        BendingDatas.Add(new BendingDataViewModel(item));
        //    }

        //    PipeLength = _unitOfWork.PipeLength;
        //    YSetup = _unitOfWork.SetUpPoint;
        //    ColletOffsetLength = _unitOfWork.ColletOffsetLength;
        //}
        //catch (Exception)
        //{
        //    // Ignore
        //}

        //UpdateBend();
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
            BendingDatas.Add(new BendingDataViewModel() { Id = 1, PipeLength = PipeLength, YSetup = YSetup, ColletOffsetLength = ColletOffsetLength });
        else
            BendingDatas.Add(new BendingDataViewModel() { Id = lastElement.Id + 1, PipeLength = PipeLength, YSetup = YSetup, ColletOffsetLength = ColletOffsetLength });
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

        var cuted = BendingDatas.Where(bd => bd.IsCuted).ToList();
        cuted.ForEach(c => c.IsCuted = false);

        var json = JsonSerializer.Serialize<BendingDataViewModel>(SelectedBendingData);
        Clipboard.SetData(DataFormats.Text, Convert.ToBase64String(Encoding.UTF8.GetBytes(json))); // TODO: Устарел, использовать SetDataAsJson https://learn.microsoft.com/ru-ru/dotnet/desktop/winforms/migration/clipboard-dataobject-net10
    }

    [RelayCommand]
    private void PasteBendingDataFromClipboard()
    {
        var data = Clipboard.GetData(DataFormats.Text) as string; // TODO: Устарел, использовать TryGetData https://learn.microsoft.com/ru-ru/dotnet/desktop/winforms/migration/clipboard-dataobject-net10
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

            var cuted = BendingDatas.FirstOrDefault(i => i.IsCuted);
            if (cuted is not null)
                BendingDatas.Remove(cuted);

            if (bd.IsCuted)
                bd.IsCuted = false;

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

    [RelayCommand]
    private void CutBendingDataToClipboard()
    {
        foreach (var item in BendingDatas)
        {
            if (item.IsCuted)
                item.IsCuted = false;
        }

        if (SelectedBendingData is null)
            return;

        SelectedBendingData.IsCuted = true;
        var json = JsonSerializer.Serialize<BendingDataViewModel>(SelectedBendingData);
        Clipboard.SetData(DataFormats.Text, Convert.ToBase64String(Encoding.UTF8.GetBytes(json))); // TODO: Устарел, использовать SetDataAsJson https://learn.microsoft.com/ru-ru/dotnet/desktop/winforms/migration/clipboard-dataobject-net10
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
        float pipeDiameter = _adjustmentService.FirstLevelAdjustment is null ? 50f : _adjustmentService.FirstLevelAdjustment.PipeDiameter;
        pipeDiameter = BendingDatas.Count > 0 ? pipeDiameter : 5;

        Pipe = PipeGenerator.GeneratePipeBend(
            WireBuilder.BuildWirePath(_unitOfWork.BendingDatas, pipeDiameter),
            pipeDiameter);
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
                ColletOffsetLength = data.ColletOffsetLength,
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

    partial void OnEstimatedRemainingLengthChanged(float oldValue, float newValue)
    {
        _unitOfWork.EstimatedRemainingLength = newValue;
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnPipeLengthChanged(float oldValue, float newValue)
    {
        _unitOfWork.PipeLength = newValue;
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnYSetupChanged(float oldValue, float newValue)
    {
        _unitOfWork.SetUpPoint = newValue;
        _unitOfWork.HasUnsavedData = true;
    }

    partial void OnColletOffsetLengthChanged(float oldValue, float newValue)
    {
        _unitOfWork.ColletOffsetLength = newValue;
        _unitOfWork.HasUnsavedData = true;
    }
}