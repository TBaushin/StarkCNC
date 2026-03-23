using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
///     <MyNamespace:PersonPictureControl/>
///
/// </summary>
public class PersonPictureControl : Control
{
    TextBlock? _initialsTextBlock;
    Ellipse? _pictureEllipse;
    Ellipse? _statusEllipse;

    public static readonly DependencyProperty StatusTextProperty = DependencyProperty
        .Register("StatusText", typeof(string), typeof(PersonPictureControl), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty
        .Register("StrokeThickness", typeof(double), typeof(PersonPictureControl), new PropertyMetadata(default(double)));
    public static readonly DependencyProperty PictureSourceProperty = DependencyProperty
        .Register("PictureSource", typeof(ImageSource), typeof(PersonPictureControl), new PropertyMetadata(default(ImageSource)));
    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty
        .Register("DisplayName", typeof(string), typeof(PersonPictureControl), new PropertyMetadata(default(string)));
    public static readonly DependencyProperty StatusPictureSourceProperty = DependencyProperty
        .Register("StatusPictureSource", typeof(ImageSource), typeof(PersonPictureControl), new PropertyMetadata(default(ImageSource)));

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public ImageSource PictureSource
    {
        get => (ImageSource)GetValue(PictureSourceProperty);
        set => SetValue(PictureSourceProperty, value);
    }

    public string DisplayName
    {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public ImageSource StatusPictureSource
    {
        get => (ImageSource)GetValue(StatusPictureSourceProperty);
        set => SetValue(StatusPictureSourceProperty, value);
    }

    static PersonPictureControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PersonPictureControl), new FrameworkPropertyMetadata(typeof(PersonPictureControl)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var tb = GetTemplateChild("InitialsTextBlock") as TextBlock;
        if (tb is not null)
        {
            _initialsTextBlock = tb;
            _initialsTextBlock.Background = Brushes.BlueViolet;
        }

        var picture = GetTemplateChild("PictureEllipse") as Ellipse;
        if (picture is not null)
            _pictureEllipse = picture;

        var status = GetTemplateChild("StatusEllipse") as Ellipse;
        if (status is not null)
            _statusEllipse = status;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_initialsTextBlock is null || _pictureEllipse is null)
            return;

        if (PictureSource is not null)
        {
            _initialsTextBlock.Visibility = Visibility.Collapsed;
            _pictureEllipse.Visibility = Visibility.Visible;
        }
        else
        {
            _initialsTextBlock.Visibility = Visibility.Visible;
            _pictureEllipse.Visibility = Visibility.Collapsed;

            var names = DisplayName.Split(' ');
            var initials = string.Join("", names.Select(name => name[0]));
            _initialsTextBlock.Text = initials;
        }
    }
}
