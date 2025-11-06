using StarkCNC.Core.Models;
using StarkCNC.DTO;
using StarkCNC.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    public static DependencyProperty SelectedAdjustmentProperty = DependencyProperty
        .Register(nameof(SelectedAdjustment), typeof(AdjustmentParametersDto), typeof(FloorControl), new PropertyMetadata());

    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty EnabledProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(FloorControl), new PropertyMetadata());

    [SuppressMessage("Usage", "CA2211", Justification = "Особенность XAML UI")]
    public static DependencyProperty NavigateProperty = DependencyProperty
        .Register(nameof(Navigate), typeof(ICommand), typeof(FloorControl), new PropertyMetadata());

    private AdjustmentTypeToStringConverter _converter = new AdjustmentTypeToStringConverter();

    public string FloorText
    {
        get => (string)GetValue(FloorTextProperty);
        set => SetValue(FloorTextProperty, value);
    }

    public AdjustmentParametersDto? SelectedAdjustment
    {
        get => (AdjustmentParametersDto?)GetValue(SelectedAdjustmentProperty);
        set
        {
            SetValue(SelectedAdjustmentProperty, value);

            if (value is null || value.Type is null)
                SelectedAdjustmentLabel.Content = string.Empty;
            else
                SelectedAdjustmentLabel.Content =
                    $"{value.Name} {_converter.Convert(value.Type, typeof(AdjustmentType), new {}, CultureInfo.CurrentCulture)} R{value.Radius}";
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
        if (SelectedAdjustment is not null)
            Navigate.Execute(SelectedAdjustment);
    }
}