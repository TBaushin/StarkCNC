using CommunityToolkit.Mvvm.ComponentModel;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using StarkCNC.Core.Services;
using StarkCNC.Services;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class VisualizationViewModel : ViewModelBase
{
    private readonly IMachineLoader _loader;
    private readonly IAdjustmentService _adjustmentService;

    public TextureModel? EnvironmentMap { get; }

    [ObservableProperty]
    private SceneNodeGroupModel3D _groupModel;

    [ObservableProperty]
    private Point3D _modelCentroid = default;

    [ObservableProperty]
    private bool _renderEnvironmentMap = true;

    [ObservableProperty]
    private IEffectsManager _effectsManager;

    [ObservableProperty]
    private HelixToolkit.Wpf.SharpDX.Camera? _camera;

    [ObservableProperty]
    private double _console;

    [ObservableProperty]
    private double _bend;

    [ObservableProperty]
    private double _supply;

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private double _clamp;

    [ObservableProperty]
    private double _press;

    public VisualizationViewModel(IMachineLoader loader, IAdjustmentService adjustmentService)
    {
        _loader = loader;
        _adjustmentService = adjustmentService;

        EffectsManager = new DefaultEffectsManager();

        Camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera()
        {
            LookDirection = new Vector3D(0, -10, -10),
            Position = new Point3D(0, 10, 10),
            UpDirection = new Vector3D(0, 1, 0),
            FarPlaneDistance = 50000,
            NearPlaneDistance = 0.5f
        };

        _loader.Load(EffectsManager, RenderEnvironmentMap);
        if (_loader.Carriage.Figure.Root.TryGetCentroid(out var centroid))
            ModelCentroid = centroid.ToPoint3D();

        GroupModel = _loader.Group;

        SetDefaultSlidersValue();
    }

    private void SetDefaultSlidersValue()
    {
        Dictionary<string, double> positions = IMachineLoader.GetDefault();
        Console = positions["console"];
        Bend = positions["bend"];
        Supply = positions["carriage"];
        Height = positions["height"];
        Clamp = positions["clamp"];
        Press = positions["press"];
    }

    partial void OnConsoleChanged(double value)
    {
        _loader.Console.Coordinates.PositionX = value;
        _loader.Console.UpdateTransform();
    }

    partial void OnBendChanged(double value)
    {
        _loader.Bend.Coordinates.RotationZ = -value;
        _loader.Bend.UpdateTransform(-value);
    }

    partial void OnSupplyChanged(double value)
    {
        _loader.Carriage.Coordinates.PositionY = value - 3000;
        _loader.Carriage.UpdateTransform();
    }

    partial void OnHeightChanged(double value)
    {
        _loader.Console.Coordinates.PositionZ = value;
        _loader.Console.UpdateTransform();
    }

    partial void OnClampChanged(double value)
    {
        _loader.Clamp.Coordinates.PositionX = -180 - value;
        _loader.Clamp.UpdateTransform();
    }

    partial void OnPressChanged(double value)
    {
        _loader.Press.Coordinates.PositionX = -180 - value;
        _loader.Press.UpdateTransform();
    }
}