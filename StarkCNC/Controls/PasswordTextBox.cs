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
///     <MyNamespace:PasswordTextBox/>
///
/// </summary>
public class PasswordTextBox : Control
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty
        .Register(nameof(Password), typeof(string), typeof(PasswordTextBox));
    public static readonly DependencyProperty ShowPasswordProperty = DependencyProperty
        .Register(nameof(ShowPassword), typeof(bool), typeof(PasswordTextBox));
    public static readonly RoutedEvent PasswordChangedEvent = EventManager
        .RegisterRoutedEvent(nameof(PasswordChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PasswordTextBox));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    public bool ShowPassword
    {
        get => (bool)GetValue(ShowPasswordProperty);
        set => SetValue(ShowPasswordProperty, value);
    }

    public event RoutedEventHandler? PasswordChanged;

    static PasswordTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordTextBox), new FrameworkPropertyMetadata(typeof(PasswordTextBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var showBtnBorder = GetTemplateChild("ShowBtnBorder") as Border;
        if (showBtnBorder is not null)
            showBtnBorder.MouseDown += (sender, args) => ShowPassword = !ShowPassword;

        var pb = GetTemplateChild("Pb") as PasswordBox;
        var tbPb = GetTemplateChild("PbTb") as TextBox;

        if (pb is not null && tbPb is not null)
        {
            pb.PasswordChanged += (sender, args) =>
            {
                if (pb.Password != tbPb.Text)
                    Password = pb.Password;
                PasswordChanged?.Invoke(this, args);
            };

            tbPb.TextChanged += (sender, args) =>
            {
               if (pb.Password != tbPb.Text)
                    pb.Password = tbPb.Text;
                PasswordChanged?.Invoke(this, args);
            };
        }
    }

    public void Clear()
    {
        var pb = GetTemplateChild("Pb") as PasswordBox;
        var tbPb = GetTemplateChild("PbTb") as TextBox;

        pb?.Clear();
        tbPb?.Clear();
    }
}
