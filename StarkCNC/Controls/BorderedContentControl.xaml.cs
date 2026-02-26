using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for BorderedContentControl.xaml
/// </summary>
public partial class BorderedContentControl : UserControl
{
    public static readonly DependencyProperty ChangeColorOnHoverProperty = DependencyProperty
        .Register(nameof(ChangeColorOnHover), typeof(bool), typeof(BorderedContentControl), new PropertyMetadata());

    public static readonly DependencyProperty BorderedContentProperty = DependencyProperty
        .Register(nameof(BorderedContent), typeof(object), typeof(BorderedContentControl), new PropertyMetadata());

    public bool ChangeColorOnHover
    {
        get => (bool)GetValue(ChangeColorOnHoverProperty);
        set => SetValue(ChangeColorOnHoverProperty, value);
    }

    public object? BorderedContent
    {
        get => GetValue(BorderedContentProperty);
        set => SetValue(BorderedContentProperty, value);
    }

    public BorderedContentControl()
    {
        InitializeComponent();
    }

    private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null)
            return;

        if (ChangeColorOnHover)
            border.Background = (Brush)FindResource("CardBackgroundFillColorSecondaryBrush");
    }

    private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var border = sender as Border;
        if (border is null)
            return;

        if (ChangeColorOnHover)
            border.Background = (Brush)FindResource("CardBackgroundFillColorDefaultBrush");
    }
}