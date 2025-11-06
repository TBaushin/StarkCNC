using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for ButtonWithIndicator.xaml
/// </summary>
public partial class ButtonWithIndicator : UserControl
{
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty
        .Register("ButtonContent", typeof(object), typeof(ButtonWithIndicator), new PropertyMetadata());
    public static readonly DependencyProperty CommandProperty = DependencyProperty
        .Register("Command", typeof(ICommand), typeof(ButtonWithIndicator), new PropertyMetadata());
    public static readonly DependencyProperty CommandCancelProperty = DependencyProperty
        .Register("CommandCancel", typeof(ICommand), typeof(ButtonWithIndicator), new PropertyMetadata());
    public static readonly DependencyProperty IndicatorColorProperty = DependencyProperty
        .Register(nameof(IndicatorColor), typeof(Color), typeof(ButtonWithIndicator), new PropertyMetadata(Colors.DarkRed));

    public object? ButtonContent
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public ICommand CommandCancel
    {
        get => (ICommand)GetValue(CommandCancelProperty);
        set => SetValue(CommandCancelProperty, value);
    }

    public Color IndicatorColor
    {
        get => (Color)GetValue(IndicatorColorProperty);
        set => SetValue(IndicatorColorProperty, value);
    }

    public ButtonWithIndicator()
    {
        InitializeComponent();
    }

    private void Button_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Command.Execute(null);
    }

    private void Button_MouseUp(object sender, MouseButtonEventArgs e)
    {
        CommandCancel.Execute(null);
    }

    private void SetColor(object? value)
    {
        if (value is null || value is not bool)
            return;

        var booleanValue = (bool)value;
        if (booleanValue)
            Indicator.Color = Colors.Green;
        else
            Indicator.Color = Colors.DarkRed;
    }
}