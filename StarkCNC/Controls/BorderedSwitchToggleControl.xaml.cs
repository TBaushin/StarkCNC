using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for BorderedSwitchToggleControl.xaml
/// </summary>
public partial class BorderedSwitchToggleControl : UserControl
{
    public static readonly DependencyProperty TextContentProperty = DependencyProperty
        .Register(nameof(TextContent), typeof(object), typeof(BorderedSwitchToggleControl), new PropertyMetadata());
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(BorderedSwitchToggleControl), new PropertyMetadata(false));

    public object TextContent
    {
        get => (object)GetValue(TextContentProperty);
        set => SetValue(TextContentProperty, value);
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public BorderedSwitchToggleControl()
    {
        InitializeComponent();
    }
}