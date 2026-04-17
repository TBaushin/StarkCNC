using CommunityToolkit.Mvvm.ComponentModel;

namespace StarkCNC.Models;

internal partial class AdjustmentParameters : ObservableObject
{
    public string AdjustmentTypeRequestString { get; private set; } = string.Empty;
    public string PipeDiameterRequestString { get; private set; } = string.Empty;
    public string PressDangerZoneCoordinateRequestString { get; private set; } = string.Empty;
    public string ForwardDangerZoneCoordinateRequestString { get; private set; } = string.Empty;
    public string ClampDeepRequestString { get; private set; } = string.Empty;
    public string ConsoleBendPositionRequestString { get; private set; } = string.Empty;
    public string ConsoleSecondFloorPositionRequestString { get; private set; } = string.Empty;
    public string ConsoleSecondIntermediatePositionRequestString { get; private set; } = string.Empty;
    public string ClampLengthRequestString { get; private set; } = string.Empty;
    public string PressLengthRequestString { get; private set; } = string.Empty;
    public string BendRollerRadiusRequestString { get; private set; } = string.Empty;
    public string SqueezeTurnOnRequestString { get; private set; } = string.Empty;
    public string DistnceFromCenterRequestString { get; private set; } = string.Empty;
    public string BendRollerOuterRadiusRequestString { get; private set; } = string.Empty;
    public string ClampRollerOuterRadiusRequestString { get; private set; } = string.Empty;
    public string ClampRollerInnerRadiusRequestString { get; private set; } = string.Empty;

    public AdjustmentParameters()
    {
    }
}