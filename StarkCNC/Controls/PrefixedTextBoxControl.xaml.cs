using StarkCNC.Helpers;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for PrefixedTextBoxControl.xaml
/// </summary>
public partial class PrefixedTextBoxControl : UserControl
{
    public static readonly DependencyProperty PrefixProperty = DependencyProperty
        .Register(nameof(Prefix), typeof(string), typeof(PrefixedTextBoxControl), new PropertyMetadata(""));
    public static readonly DependencyProperty TextProperty = DependencyProperty
        .Register(nameof(Text), typeof(string), typeof(PrefixedTextBoxControl), new PropertyMetadata(""));
    public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty
        .Register(nameof(NeedCallNumberInput), typeof(bool), typeof(PrefixedTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty
        .Register(nameof(IsNumericOnly), typeof(bool), typeof(PrefixedTextBoxControl), new PropertyMetadata(false));

    public string Prefix
    {
        get => (string)GetValue(PrefixProperty);
        set => SetValue(PrefixProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public PrefixedTextBoxControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (NeedCallNumberInput)
        {
            var value = NumberInputViewModel.ShowDialog();
            Text = value.ToString(CultureInfo.CurrentCulture);
        }
        e.Handled = true;
    }

    private void InputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (IsNumericOnly)
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }
}