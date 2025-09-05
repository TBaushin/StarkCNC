using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for BorderedContentControl.xaml
/// </summary>
public partial class BorderedContentControl : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty
        .Register(nameof(Text), typeof(string), typeof(BorderedContentControl), new PropertyMetadata());
    public static readonly DependencyProperty BorderedContentProperty = DependencyProperty
        .Register(nameof(BorderedContent), typeof(object), typeof(BorderedContentControl), new PropertyMetadata());

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public object? BorderedContent
    {
        get => (object)GetValue(BorderedContentProperty);
        set => SetValue(BorderedContentProperty, value);
    }

    public BorderedContentControl()
    {
        InitializeComponent();
    }
}
