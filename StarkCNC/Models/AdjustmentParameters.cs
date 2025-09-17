using CommunityToolkit.Mvvm.ComponentModel;
using StarkCNC.MachineCommunication.Services;

namespace StarkCNC.Models;

internal class AdjustmentParameters : ObservableObject
{
    private readonly IManualConfigurationService _manualConfigurationService;

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

    private Task? _updateTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public AdjustmentParameters(IManualConfigurationService manualConfigurationService, bool autoRunUpdate)
    {
        _manualConfigurationService = manualConfigurationService;

        if (autoRunUpdate)
            StartUpdateTask();
    }

    public void StartUpdateTask()
    {
        if (TaskIsRunning())
            return;

        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        _updateTask = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(150).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
                // Нормально: задача отменена
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateTask error: {ex}");
            }
        }, token);
    }

    private bool TaskIsRunning() =>
        _updateTask is not null && !_updateTask.IsCompleted && !_updateTask.IsCanceled && !_updateTask.IsFaulted;

    public void StopUpdateTask()
    {
        if (_updateTask is null)
            return;

        _cancellationTokenSource?.Cancel();
    }
}
