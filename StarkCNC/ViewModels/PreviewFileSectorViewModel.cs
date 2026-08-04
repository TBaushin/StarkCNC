using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Win32;
using SharpDX.Mathematics.Interop;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class PreviewFileSectorViewModel : ViewModelBase
{
    private const string _registryKey = "Software\\StarkCNC";

    private readonly IRouter _router;

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
    private ObservableCollection<PathInformation> _latestFiles = new ObservableCollection<PathInformation>();

    [ObservableProperty]
    private PathInformation? _selectedItem;

    public PreviewFileSectorViewModel(IRouter router, IBendingDataUnitOfWork unitOfWork)
    {
        _router = router;
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
            LookDirection = new Vector3D(-7, -10, -10),
            Position = new Point3D(0, 0, 10),
            UpDirection = new Vector3D(0, 1, 0),
            FarPlaneDistance = 50000,
            NearPlaneDistance = 0.5f
        };

        ReadLastOpenedFolder();
        ReadLatestOpenedFiles();
        if (string.IsNullOrEmpty(_currentPath))
            SetDrivers();
        else
            UpdateFolders();
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        if (SelectedItem is not null)
        {
            var path = Path.Combine(_currentPath, SelectedItem.Path);
            var pathInfo = new PathInformation(Path.GetFileName(path), path, "\xE8A5", OpenFolderCommand);
            if (!LatestFiles.Contains(pathInfo))
                LatestFiles.Add(pathInfo);
            await _unitOfWork.OpenFile(path).ConfigureAwait(true);
            WriteLatestOpenedFiles();
            _router.Navigate("/program");
        }
    }

    [RelayCommand]
    private void OpenFolder()
    {
        if (SelectedItem is null)
            return;

        _currentPath = Path.Combine(_currentPath, SelectedItem.Path);
        UpdateFolders();
        WriteLastOpenedFolder();
    }

    [RelayCommand]
    private void GoBack()
    {
        var parent = Directory.GetParent(_currentPath);
        if (parent is null)
        {
            _currentPath = string.Empty;
            SetDrivers();
            WriteLastOpenedFolder();
        }
        else
        {
            _currentPath = parent.FullName;
            UpdateFolders();
            WriteLastOpenedFolder();
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

    private void ReadLastOpenedFolder()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, false);
        if (registry is null)
            return;

        var directory = registry.GetValue("LastOpenedFolder") as string;
        if (string.IsNullOrEmpty(directory))
            return;

        if (Directory.Exists(directory))
            _currentPath = directory;
    }

    private void WriteLastOpenedFolder()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, true);
        if (registry is null)
            registry = Registry.CurrentUser.CreateSubKey(_registryKey);

        if (File.Exists(_currentPath))
        {
            var directory = Path.GetDirectoryName(_currentPath) ?? string.Empty;
            registry.SetValue("LastOpenedFolder", directory);
        }
        else if (Directory.Exists(_currentPath))
        {
            registry.SetValue("LastOpenedFolder", _currentPath);
        }
    }

    private void WriteLatestOpenedFiles()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, true);
        if (registry is null)
            registry = Registry.CurrentUser.CreateSubKey(_registryKey);

        var latestFiles = new List<string>();
        foreach (var file in LatestFiles)
        {
            latestFiles.Add(file.Path);
        }

        registry.SetValue("LatestFiles", JsonSerializer.Serialize(latestFiles));
    }

    private void ReadLatestOpenedFiles()
    {
        var registry = Registry.CurrentUser.OpenSubKey(_registryKey, false);
        if (registry is null)
            return;

        var json = registry.GetValue("LatestFiles") as string;
        if (string.IsNullOrEmpty(json))
            return;

        var latestFiles = JsonSerializer.Deserialize<List<string>>(json);
        if (latestFiles is null)
            return;

        LatestFiles.Clear();
        latestFiles.ForEach(file =>
        {
            LatestFiles.Add(new PathInformation(Path.GetFileName(file), file, "\xE8A5", OpenFolderCommand));
        });
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
