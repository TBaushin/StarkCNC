using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using SharpDX.Mathematics.Interop;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class PreviewFileSectorViewModel : ViewModelBase
{
    private readonly IBendingDataUnitOfWork _unitOfWork;

    private string _currentPath = string.Empty;

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
    private TextureModel? textureModel;

    [ObservableProperty]
    private HelixToolkit.Wpf.SharpDX.Camera? _camera;

    [ObservableProperty]
    private ObservableCollection<PathInformation> _currentFolderContent = new ObservableCollection<PathInformation>();

    [ObservableProperty]
    private PathInformation? _selectedItem;

    public PreviewFileSectorViewModel(IBendingDataUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

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
            LookDirection = new Vector3D(0, -10, -10),
            Position = new Point3D(0, 10, 10),
            UpDirection = new Vector3D(0, 1, 0),
            FarPlaneDistance = 50000,
            NearPlaneDistance = 0.5f
        };

        SetDrivers();
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        if (SelectedItem is not null)
        {
            await _unitOfWork.OpenFile(Path.Combine(_currentPath, SelectedItem.Path)).ConfigureAwait(true);
        }
    }

    [RelayCommand]
    private void OpenFolder()
    {
        if (SelectedItem is null)
            return;

        _currentPath = Path.Combine(_currentPath, SelectedItem.Path);
        UpdateFolders();
    }

    [RelayCommand]
    private void GoBack()
    {
        var parent = Directory.GetParent(_currentPath);
        if (parent is null)
        {
            SetDrivers();
        }
        else
        {
            _currentPath = parent.FullName;
            UpdateFolders();
        }
    }

    private void SetDrivers()
    {
        CurrentFolderContent.Clear();

        foreach (var drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady)
                continue;
            
            string driveName;
            if (string.IsNullOrEmpty(drive.VolumeLabel))
                driveName = drive.Name;
            else
                driveName = $"{drive.Name} ({drive.VolumeLabel})";

            CurrentFolderContent.Add(new PathInformation(driveName, drive.Name, "\xEDA2", OpenFolderCommand));
        }
    }

    private void UpdateFolders()
    {
        CurrentFolderContent.Clear();
        CurrentFolderContent.Add(
            new PathInformation(
                "..",
                Directory.GetParent(_currentPath)?.FullName ?? string.Empty,
                "\xE8B7",
                GoBackCommand));

        var dirs = Directory.GetDirectories(_currentPath);
        foreach (var dir in dirs)
        {
            CurrentFolderContent.Add(new PathInformation(Path.GetFileName(dir), dir, "\xE8B7", OpenFolderCommand));
        }

        var files = Directory.GetFiles(_currentPath);
        foreach (var file in files)
        {
            if (file.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
                CurrentFolderContent.Add(new PathInformation(Path.GetFileName(file), file, "\xE8A5", OpenFolderCommand));
        }
    }

    partial void OnSelectedItemChanged(PathInformation? value)
    {
        Pipe = null;
        if (value is null)
            return;

        var fileInfo = new FileInfo(System.IO.Path.Combine(_currentPath, value.Path));
        if (fileInfo.Exists && fileInfo.Extension == ".csv")
        {
            var result = ICSVService.Import(fileInfo.FullName);
            if (result is not null)
            {
                Pipe = PipeGenerator.GeneratePipeBend(
                    WireBuilder.BuildWirePath(result.ToList(), 32),
                    32);
            }
        }
    }
}
