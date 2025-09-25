using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for SwitchToggleControl.xaml
/// </summary>
public partial class SwitchToggleControl : UserControl
{
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(SwitchToggleControl), new PropertyMetadata(false, OnIsCheckedChanged));
    public static readonly RoutedEvent ClickEvent = EventManager
        .RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwitchToggleControl));

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set
        {
            SetValue(IsCheckedProperty, value);
            ChangePosition();
        }
    }

    public event RoutedEventHandler? Click;

    public SwitchToggleControl()
    {
        InitializeComponent();
        ChangePosition();
        MouseLeftButtonDown += SwitchToggleControl_MouseLeftButtonDown;
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as SwitchToggleControl;
        if (control is null)
            return;

        control.ChangePosition();
    }

    private void SwitchToggleControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        IsChecked = !IsChecked;
    }

    private void ChangePosition()
    {
        if (IsChecked)
        {
            SetSwitchOnStyles();
        }
        else
        {
            SetSwitchOffStyles();
        }
    }

    private void SetSwitchOnStyles()
    {
        SwitchBorder.Background = (Brush)FindResource("AccentFillColorDefaultBrush");
        SwitchBorder.BorderBrush = null;
        SwitchBorder.BorderThickness = new Thickness(0);
        SwitchBorder.MinHeight = 21;
        SwitchBorder.MaxHeight = 21;
        SwitchBorder.MinWidth = 40;
        SwitchBorder.MaxWidth = 40;
        SwitchBorder.CornerRadius = new CornerRadius(10);

        SwitchEllipse.Fill = (Brush)FindResource("TextOnAccentFillColorPrimaryBrush");
        SwitchEllipse.HorizontalAlignment = HorizontalAlignment.Right;
        SwitchEllipse.Margin = new Thickness(4);
        SwitchEllipse.Width = 13;
    }

    private void SetSwitchOffStyles()
    {
        SwitchBorder.Background = (Brush)FindResource("ControlAltFillColorTertiaryBrush");
        SwitchBorder.BorderBrush = (Brush)FindResource("ControlAltFillColorQuarternaryBrush");
        SwitchBorder.BorderThickness = new Thickness(1);
        SwitchBorder.MinHeight = 22;
        SwitchBorder.MaxHeight = 22;
        SwitchBorder.MinWidth = 40;
        SwitchBorder.MaxWidth = 40;
        SwitchBorder.CornerRadius = new CornerRadius(10);

        SwitchEllipse.Fill = (Brush)FindResource("ControlStrongFillColorDefaultBrush");
        SwitchEllipse.HorizontalAlignment = HorizontalAlignment.Left;
        SwitchEllipse.Margin = new Thickness(3);
        SwitchEllipse.Width = 14;
    }

    private void SwitchBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Cursor = Cursors.Hand;
    }

    private void SwitchBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Cursor = Cursors.Arrow;
    }
}
