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
///     <MyNamespace:BorderedControl/>
///
/// </summary>
public class BorderedControl : ContentControl
{
    public static readonly DependencyProperty ChangeColorOnHoverProperty = DependencyProperty
        .Register(nameof(ChangeColorOnHover), typeof(bool), typeof(BorderedControl), new PropertyMetadata());

    public static readonly DependencyProperty IsOneBorderProperty = DependencyProperty
        .Register(nameof(IsOneBorder), typeof(bool), typeof(BorderedControl), new PropertyMetadata());

    public static readonly DependencyProperty IsFirstProperty = DependencyProperty
        .Register(nameof(IsFirst), typeof(bool), typeof(BorderedControl), new PropertyMetadata());

    public static readonly DependencyProperty IsLastProperty = DependencyProperty
        .Register(nameof(IsLast), typeof(bool), typeof(BorderedControl), new PropertyMetadata());

    public bool ChangeColorOnHover
    {
        get => (bool)GetValue(ChangeColorOnHoverProperty);
        set => SetValue(ChangeColorOnHoverProperty, value);
    }

    public bool IsOneBorder
    {
        get => (bool)GetValue(IsOneBorderProperty);
        set => SetValue(IsOneBorderProperty, value);
    }

    public bool IsFirst
    {
        get => (bool)GetValue(IsFirstProperty);
        set
        {
            if (!IsOneBorder && value == true)
                throw new InvalidOperationException($"Установка значения для свойства {nameof(IsFirst)} невозможна, если свойству {nameof(IsOneBorder)} установление значение false");

            if (IsOneBorder && value == true && IsLast)
                throw new InvalidOperationException($"Невозможно установить значение true для свойства {nameof(IsFirst)}, если установлено значение true для свойства {nameof(IsLast)}");

            SetValue(IsFirstProperty, value);
        }
    }

    public bool IsLast
    {
        get => (bool)GetValue(IsLastProperty);
        set
        {
            if (!IsOneBorder && value == true)
                throw new InvalidOperationException($"Установка значения для свойства {nameof(IsLast)} невозможна, если свойству {nameof(IsOneBorder)} установление значение false");

            if (IsOneBorder && value == true && IsFirst)
                throw new InvalidOperationException($"Невозможно установить значение true для свойства {nameof(IsLast)}, если установлено значение true для свойства {nameof(IsFirst)}");

            SetValue(IsLastProperty, value);
        }
    }

    static BorderedControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderedControl), new FrameworkPropertyMetadata(typeof(BorderedControl)));
    }
}
