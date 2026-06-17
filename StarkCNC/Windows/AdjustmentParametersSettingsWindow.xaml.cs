using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Utilities;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace StarkCNC;

/// <summary>
/// Interaction logic for AdjustmentParametersSettingsWindow.xaml
/// </summary>
public partial class AdjustmentParametersSettingsWindow : Window, INotifyPropertyChanged
{
    private IAdjustmentService _adjustmentService;
    private IManualConfigurationService _configurationService;
    private float _currentPositionCoordinate;

    private string _parameter;

    public event PropertyChangedEventHandler? PropertyChanged;

    public AdjustmentParameters Adjustment { get; set; }

    public float CurrentPositionCoordinate
    {
        get => _currentPositionCoordinate;
        set
        {
            _currentPositionCoordinate = value;
            OnPropertyChanged(nameof(CurrentPositionCoordinate));
        }
    }

    public Color ResetIndicatorColor { get; set; } = Brushes.Red.Color;
    public Color BackwardIndicatorColor { get; set; } = Brushes.Red.Color;
    public Color ForwardIndicatorColor { get; set; } = Brushes.Red.Color;

    public AdjustmentParametersSettingsWindow(
        AdjustmentParameters adjustment,
        string parameter,
        IAdjustmentService adjustmentService,
        IManualConfigurationService configurationService)
    {
        Adjustment = adjustment;
        DataContext = this;
        _adjustmentService = adjustmentService;
        _configurationService = configurationService;
        _parameter = parameter;

        InitializeComponent();

        this.Initialized += async (object? sender, EventArgs e) =>
        {
            await _configurationService
                .WriteAsync(false, ControllerRequestStrings.AUTOMATIC_TAGS_TURN_ON)
                .ConfigureAwait(true);
            await _configurationService
                .WriteAsync(true, ControllerRequestStrings.ADJUSTMENT_TURN_ON)
                .ConfigureAwait(true);
        };

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
        SpeedCoefficientGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        switch (parameter)
        {
            case nameof(Adjustment.Supply):
                SupplyStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "Подача";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_SUPPLY_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_SUPPLY_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_SUPPLY_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_SUPPLY_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
            case nameof(Adjustment.Console):
                ConsoleStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.Visibility = Visibility.Collapsed;
                TitleTextBlock.Text = "Консоль";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_CONSOLE_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CONSOLE_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CONSOLE_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CONSOLE_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
            case nameof(Adjustment.Rotation):
                RotationStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.Visibility = Visibility.Collapsed;
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
                SpeedCoefficientGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.6, GridUnitType.Star) });
                TitleTextBlock.Text = "Зажим";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_CLAMP_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CLAMP_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CLAMP_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_CLAMP_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
            case nameof(Adjustment.Dorn):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Dorn;
                SpeedCoefficient.DataContext = Adjustment.Dorn;
                SpeedCoefficientGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.6, GridUnitType.Star) });
                TitleTextBlock.Text = "Дорн";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_DORN_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_DORN_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_DORN_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_DORN_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
            case nameof(Adjustment.Press):
                ClampDornPressStackPanel.Visibility = Visibility.Visible;
                ClampDornPressStackPanel.DataContext = Adjustment.Press;
                SpeedCoefficient.DataContext = Adjustment.Press;
                SpeedCoefficientGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.6, GridUnitType.Star) });
                TitleTextBlock.Text = "Прижим";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_PRESS_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_PRESS_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_PRESS_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_PRESS_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
            case nameof(Adjustment.Lift):
                LiftStackPanel.Visibility = Visibility.Visible;
                SpeedCoefficient.DataContext = Adjustment.Lift;
                SpeedCoefficientGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.6, GridUnitType.Star) });
                TitleTextBlock.Text = "Подъём";
                _configurationService.Subscribe<float>(ControllerRequestStrings.GET_LIFT_CURRENT_POSITION(_adjustmentService.CurrentLevel), value => CurrentPositionCoordinate = value);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_LIFT_RESET(_adjustmentService.CurrentLevel), value => ResetIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_LIFT_BACKWARD(_adjustmentService.CurrentLevel), value => BackwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                _configurationService.Subscribe<bool>(ControllerRequestStrings.GET_LIFT_FORWARD(_adjustmentService.CurrentLevel), value => ForwardIndicatorColor = value ? Brushes.Green.Color : Brushes.Red.Color);
                break;
        }
    }

    private void Unsubscribe()
    {
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_SUPPLY_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_SUPPLY_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_SUPPLY_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_SUPPLY_FORWARD(_adjustmentService.CurrentLevel));
                break;
            case nameof(Adjustment.Console):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CONSOLE_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CONSOLE_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CONSOLE_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CONSOLE_FORWARD(_adjustmentService.CurrentLevel));
                break;
            case nameof(Adjustment.Clamp):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CLAMP_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CLAMP_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CLAMP_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_CLAMP_FORWARD(_adjustmentService.CurrentLevel));
                break;
            case nameof(Adjustment.Dorn):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_DORN_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_DORN_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_DORN_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_DORN_FORWARD(_adjustmentService.CurrentLevel));
                break;
            case nameof(Adjustment.Press):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_PRESS_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_PRESS_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_PRESS_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_PRESS_FORWARD(_adjustmentService.CurrentLevel));
                break;
            case nameof(Adjustment.Lift):
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_LIFT_CURRENT_POSITION(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_LIFT_RESET(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_LIFT_BACKWARD(_adjustmentService.CurrentLevel));
                _configurationService.Unsubscribe(ControllerRequestStrings.GET_LIFT_FORWARD(_adjustmentService.CurrentLevel));
                break;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
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

    private void BendForwardPositionSetCurrentPosition_Click(object sender, RoutedEventArgs e)
    {
        BendForwardPosition.Text = CurrentPositionCoordinate.ToString(CultureInfo.InvariantCulture);
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

    private async void Reset_Click(object sender, RoutedEventArgs e)
    {
        var level = _adjustmentService.CurrentLevel;
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_SUPPLY_RESET(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Console):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CONSOLE_RESET(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Rotation):
                break;
            case nameof(Adjustment.Bend):
                break;
            case nameof(Adjustment.Squeeze):
                break;
            case nameof(Adjustment.Clamp):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CLAMP_RESET(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Dorn):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_DORN_RESET(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Press):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_PRESS_RESET(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Lift):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_LIFT_RESET(level)).ConfigureAwait(true);
                break;
        }
    }

    private async void Backward_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var level = _adjustmentService.CurrentLevel;
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_SUPPLY_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Console):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CONSOLE_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Rotation):
                break;
            case nameof(Adjustment.Bend):
                break;
            case nameof(Adjustment.Squeeze):
                break;
            case nameof(Adjustment.Clamp):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CLAMP_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Dorn):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_DORN_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Press):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_PRESS_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Lift):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_LIFT_BACKWARD(level)).ConfigureAwait(true);
                break;
        }
    }

    private async void Backward_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var level = _adjustmentService.CurrentLevel;
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_SUPPLY_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Console):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_CONSOLE_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Rotation):
                break;
            case nameof(Adjustment.Bend):
                break;
            case nameof(Adjustment.Squeeze):
                break;
            case nameof(Adjustment.Clamp):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_CLAMP_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Dorn):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_DORN_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Press):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_PRESS_BACKWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Lift):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_LIFT_BACKWARD(level)).ConfigureAwait(true);
                break;
        }
    }

    private async void Forward_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var level = _adjustmentService.CurrentLevel;
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_SUPPLY_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Console):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CONSOLE_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Rotation):
                break;
            case nameof(Adjustment.Bend):
                break;
            case nameof(Adjustment.Squeeze):
                break;
            case nameof(Adjustment.Clamp):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_CLAMP_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Dorn):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_DORN_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Press):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_PRESS_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Lift):
                await _configurationService.WriteAsync(true, ControllerRequestStrings.GET_LIFT_FORWARD(level)).ConfigureAwait(true);
                break;
        }
    }

    private async void Forward_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var level = _adjustmentService.CurrentLevel;
        switch (_parameter)
        {
            case nameof(Adjustment.Supply):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_SUPPLY_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Console):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_CONSOLE_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Rotation):
                break;
            case nameof(Adjustment.Bend):
                break;
            case nameof(Adjustment.Squeeze):
                break;
            case nameof(Adjustment.Clamp):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_CLAMP_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Dorn):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_DORN_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Press):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_PRESS_FORWARD(level)).ConfigureAwait(true);
                break;
            case nameof(Adjustment.Lift):
                await _configurationService.WriteAsync(false, ControllerRequestStrings.GET_LIFT_FORWARD(level)).ConfigureAwait(true);
                break;
        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        Unsubscribe();
    }
}