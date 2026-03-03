using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Controls;

/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:StarkCNC.Controls;assembly=StarkCNC.Controls"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:ToggleSwitchControl/>
///
/// </summary>
public class ToggleSwitchControl : Control
{
    public static readonly DependencyProperty TextProperty = DependencyProperty
        .Register(nameof(Text), typeof(string), typeof(ToggleSwitchControl), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty ShowTextProperty = DependencyProperty
        .Register(nameof(ShowText), typeof(bool), typeof(ToggleSwitchControl), new PropertyMetadata(true));
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty
        .Register(nameof(IsChecked), typeof(bool), typeof(ToggleSwitchControl), new PropertyMetadata(false));
    public static readonly RoutedEvent OnCheckedChangedEvent = EventManager
        .RegisterRoutedEvent(nameof(OnCheckedChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleSwitchControl));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool ShowText
    {
        get => (bool)GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set
        {
            SetValue(IsCheckedProperty, value);

            UpdateText();

            if (OnCheckedChanged is not null)
                OnCheckedChanged.Invoke(this, new RoutedEventArgs());
        }
    }

    public event RoutedEventHandler? OnCheckedChanged;

    static ToggleSwitchControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitchControl), new FrameworkPropertyMetadata(typeof(ToggleSwitchControl)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var border = GetTemplateChild("ToggleSwitchBorder") as Border;
        if (border is null)
            return;

        border.PreviewMouseLeftButtonDown += Border_PreviewMouseLeftButtonDown;
    }

    private void Border_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        IsChecked = !IsChecked;
    }

    void UpdateText()
    {
        var isDefault = (Text == "Вкл." || Text == "Выкл." || string.IsNullOrEmpty(Text)) && ShowText == true;
        if (!isDefault)
            return;

        if (IsChecked)
            Text = "Вкл.";
        else
            Text = "Выкл.";
    }
}
