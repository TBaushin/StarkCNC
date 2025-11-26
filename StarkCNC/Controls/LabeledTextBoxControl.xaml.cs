using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for LabeledTextBoxControl.xaml
/// </summary>
public partial class LabeledTextBoxControl : UserControl
{
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty
        .Register(nameof(LabelText), typeof(object), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty
        .Register(nameof(TextBoxText), typeof(string), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty
        .Register(nameof(IsReadOnly), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty
        .Register(nameof(NeedCallNumberInput), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty
        .Register(nameof(IsNumericOnly), typeof(bool), typeof(LabeledTextBoxControl), new PropertyMetadata(false));

    public object? LabelText
    {
        get => (object?)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string TextBoxText
    {
        get => (string)GetValue(TextBoxTextProperty);
        set => SetValue(TextBoxTextProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public bool NeedCallNumberInput
    {
        get => (bool)GetValue(NeedCallNumberInputProperty);
        set => SetValue(NeedCallNumberInputProperty, value);
    }

    public bool IsNumericOnly
    {
        get => (bool)GetValue(IsNumericOnlyProperty);
        set => SetValue(IsNumericOnlyProperty, value);
    }

    public LabeledTextBoxControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (NeedCallNumberInput)
        {
            var value = NumberInputViewModel.ShowDialog();
            TextBoxText = value.ToString(CultureInfo.CurrentCulture);
        }
        e.Handled = true; // ???
    }

    private void InputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (IsNumericOnly)
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }
}