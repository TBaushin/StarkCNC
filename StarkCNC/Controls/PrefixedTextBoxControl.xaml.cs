using StarkCNC.Utilities;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty
        .Register(nameof(IsReadOnly), typeof(bool), typeof(PrefixedTextBoxControl), new PropertyMetadata(false));
    public static readonly RoutedEvent TextChangedEvent = EventManager.
        RegisterRoutedEvent(nameof(TextChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PrefixedTextBoxControl));

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

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public event RoutedEventHandler TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    public PrefixedTextBoxControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var binding = BindingOperations.GetBindingExpression(this, PrefixedTextBoxControl.TextProperty);

        if (NeedCallNumberInput)
        {
            var value = NumberInputViewModel.ShowDialog(Text);
            Text = value.ToString(CultureInfo.InvariantCulture);

            binding.UpdateSource();
            binding.UpdateTarget();

            e.Handled = true; // ???
        }
    }

    private void InputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (IsNumericOnly)
            e.Handled = !OnlyNumberEnterHelper.IsTextAllowed(e.Text);
    }

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetCurrentValue(TextProperty, InputTextBox.Text);

        var args = new RoutedEventArgs(TextChangedEvent, this);
        RaiseEvent(args);
    }
}
