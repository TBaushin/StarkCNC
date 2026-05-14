using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.Core.Services;
using StarkCNC.Services;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class VisualizationViewModel : ViewModelBase
{
    private readonly IMachineLoader _loader;
    private readonly IAdjustmentService _adjustmentService;

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

        SetDefaultSlidersValue();
    }

    public ModelVisual3D GetMachineVizualization()
    {
        return new ModelVisual3D() { Content = _loader.Group };
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