using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for FloorControl.xaml
/// </summary>
public partial class FloorControl : UserControl
{
    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty FloorTextProperty = DependencyProperty
        .Register(nameof(FloorText), typeof(string), typeof(FloorControl), new PropertyMetadata());

    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty  AdjustmentNameProperty = DependencyProperty
        .Register(nameof(AdjustmentName), typeof(string), typeof(FloorControl), new PropertyMetadata());

    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty EnabledProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(FloorControl), new PropertyMetadata());

    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty NavigateProperty = DependencyProperty
        .Register(nameof(Navigate), typeof(ICommand), typeof(FloorControl), new PropertyMetadata());

    public string FloorText
    {
        get => (string)GetValue(FloorTextProperty);
        set => SetValue(FloorTextProperty, value);
    }

    public string AdjustmentName
    {
        get => (string)GetValue(AdjustmentNameProperty);
        set
        {
            SetValue(AdjustmentNameProperty, value);

            if (value is null)
                SelectedAdjustmentLabel.Content = string.Empty;
            else
                SelectedAdjustmentLabel.Content = value;
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

    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Navigate.Execute(null);
        //if (SelectedAdjustment is not null)
            //Navigate.Execute(SelectedAdjustment);
    }
}