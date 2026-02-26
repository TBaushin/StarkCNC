using StarkCNC.Utilities;
using StarkCNC.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace StarkCNC.Controls;

/// <summary>
/// Interaction logic for BorderedLabeledTextBoxControl.xaml
/// </summary>
public partial class BorderedLabeledTextBoxControl : UserControl
{
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty
        .Register(nameof(LabelText), typeof(object), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty TextProperty = DependencyProperty
        .Register(nameof(Text), typeof(string), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty
        .Register(nameof(IsReadOnly), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty NeedCallNumberInputProperty = DependencyProperty
        .Register(nameof(NeedCallNumberInput), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata());
    public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty
        .Register(nameof(IsNumericOnly), typeof(bool), typeof(BorderedLabeledTextBoxControl), new PropertyMetadata(false));
    public static readonly RoutedEvent TextChangedEvent = EventManager.
        RegisterRoutedEvent(nameof(TextChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BorderedLabeledTextBoxControl));

    public object? LabelText
    {
        get => (object?)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public event RoutedEventHandler TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    public BorderedLabeledTextBoxControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var binding = BindingOperations.GetBindingExpression(this, BorderedLabeledTextBoxControl.TextProperty);

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

    private void InputTextBlock_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetCurrentValue(TextProperty, InputTextBlock.Text);

        var args = new RoutedEventArgs(TextChangedEvent, this);
        RaiseEvent(args);
    }

    private void InputTextBlock_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            InputTextBlock.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
        }
    }
}
