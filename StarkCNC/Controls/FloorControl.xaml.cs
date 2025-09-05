using StarkCNC.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for FloorControl.xaml
/// </summary>
public partial class FloorControl : UserControl
{
    public static DependencyProperty FloorTextProperty = DependencyProperty
        .Register(nameof(FloorText), typeof(string), typeof(FloorControl), new PropertyMetadata());
    public static DependencyProperty SelectedAdjustmentProperty = DependencyProperty
        .Register(nameof(SelectedAdjustment), typeof(AdjustmentParameters), typeof(FloorControl), new PropertyMetadata());
    public static DependencyProperty EnabledProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(FloorControl), new PropertyMetadata());
    public static DependencyProperty NavigateProperty = DependencyProperty
        .Register(nameof(Navigate), typeof(ICommand), typeof(FloorControl), new PropertyMetadata());

    public string FloorText
    {
        get => (string)GetValue(FloorTextProperty);
        set => SetValue(FloorTextProperty, value);
    }

    public AdjustmentParameters? SelectedAdjustment
    {
        get => (AdjustmentParameters?)GetValue(SelectedAdjustmentProperty);
        set
        {
            SetValue(SelectedAdjustmentProperty, value);

            if (value is null)
                SelectedAdjustmentLabel.Content = string.Empty;
            else
                SelectedAdjustmentLabel.Content =
                    $"{value.Name} {value.Type.Name} R{value.Radius}";
        }
    }

    public bool IsChecked
    {
        get => (bool)GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    public ICommand Navigate
    {
        get => (ICommand)GetValue(NavigateProperty);
        set => SetValue(NavigateProperty, value);
    }

    public FloorControl()
    {
        InitializeComponent();
    }

    private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Cursor = Cursors.Hand;
    }

    private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Cursor = Cursors.Arrow;
    }

    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Navigate.Execute(null);
    }
}
