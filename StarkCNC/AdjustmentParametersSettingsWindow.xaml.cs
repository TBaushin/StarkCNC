using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentParametersSettingsWindow.xaml
/// </summary>
public partial class AdjustmentParametersSettingsWindow : Window, INotifyPropertyChanged
{
    private IAdjustmentService _adjustmentService;
    private IManualConfigurationService _manualConfigurationService;
    private double _currentPositionCoordinate;

    private Task? _updateCurrentPositionCoordinate;
    private CancellationTokenSource? _cancellationTokenSource;

    public event PropertyChangedEventHandler? PropertyChanged;

    public AdjustmentParameters Adjustment { get; set; }

    public double CurrentPositionCoordinate
    {
        get => _currentPositionCoordinate;
        set
        {
            _currentPositionCoordinate = value;
            OnPropertyChanged(nameof(CurrentPositionCoordinate));
        }
    }

    public AdjustmentParametersSettingsWindow(AdjustmentParameters adjustment, string parameter, IAdjustmentService adjustmentService, IManualConfigurationService manualConfigurationService)
    {
        Adjustment = adjustment;
        DataContext = this;
        _adjustmentService = adjustmentService;
        _manualConfigurationService = manualConfigurationService;

        InitializeComponent();

        ShowParamatersEdits(parameter);

        WindowChrome.SetWindowChrome(this,
            new WindowChrome
            {
                CaptionHeight = 50,
                CornerRadius = new CornerRadius(12),
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = true,
                NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
            }
        );

        Topmost = true;
    }

    private void ShowParamatersEdits(string parameter)
    {
        switch (parameter)
        {
            case nameof(Adjustment.Supply):
                SupplyStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Supply;
                TitleTextBlock.Text = "Подача";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_SUPPLY_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // ignore
                    }                    
                }, _cancellationTokenSource.Token);
                break;
            case nameof(Adjustment.Console):
                ConsoleStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Console;
                TitleTextBlock.Text = "Консоль";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_CONSOLE_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // ignore
                    }
                }, _cancellationTokenSource.Token);
                break;
            case nameof(Adjustment.Rotation):
                RotationStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Rotation;
                TitleTextBlock.Text = "Поворот";
                break;
            case nameof(Adjustment.Bend):
                BendStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "Гиб";
                break;
            case nameof(Adjustment.Squeeze):
                SqueezeStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Squeeze;
                TitleTextBlock.Text = "Дожим";
                break;
            case nameof(Adjustment.Clamp):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Clamp;
                SpeedCoefficient.DataContext = Adjustment.Clamp;
                TitleTextBlock.Text = "Зажим";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_CLAMP_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // ignore
                    }
                }, _cancellationTokenSource.Token);
                break;
            case nameof(Adjustment.Dorn):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Dorn;
                SpeedCoefficient.DataContext = Adjustment.Dorn;
                TitleTextBlock.Text = "Дорн";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_DORN_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // ignore
                    }
                }, _cancellationTokenSource.Token);
                break;
            case nameof(Adjustment.Press):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Press;
                SpeedCoefficient.DataContext = Adjustment.Press;
                TitleTextBlock.Text = "Прижим";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_PRESS_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // Ignore
                    }
                }, _cancellationTokenSource.Token);
                break;
            case nameof(Adjustment.Lift):
                LiftStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Lift;
                TitleTextBlock.Text = "Подъём";
                _cancellationTokenSource = new CancellationTokenSource();
                _updateCurrentPositionCoordinate = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            CurrentPositionCoordinate = await _manualConfigurationService
                                .ReadAsync<double>(ControllerRequestStrings.GET_LIFT_CURRENT_POSITION(_adjustmentService.CurrentLevel))
                                .ConfigureAwait(false);
                            Thread.Sleep(150);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // Ignore
                    }
                }, _cancellationTokenSource.Token);
                break;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_updateCurrentPositionCoordinate is not null && _cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
        }

        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        if (_updateCurrentPositionCoordinate is not null && _cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
        }

        Close();
    }

    private void SupplyPressZoneSetCurrentPositionButton_Click(object sender, RoutedEventArgs e)
    {
        SupplyPressZone.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void SupplyForwardDangerZoneSetCurrentPositionButton_Click(object sender, RoutedEventArgs e)
    {
        SupplyForwardDangerZonePosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void SupplyColletJawsDepthSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        SupplyColletJawsDepth.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ConsoleBendSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        ConsoleBendPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ConsoleSecondFloorSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        ConsoleSecondFloorPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ConsoleThirdFloorSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        ConsoleThirdFloorPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ClampDornPressForwardSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        ForwardPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ClampDornPressMiddleSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        MiddlePosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void ClampDornPressBackwardSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        BackwardPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void LiftUpperSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        LiftUpperPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void LiftMiddleSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        LiftMiddlePosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void LiftLowerSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        LiftLowerPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}