using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StarkCNC.Controls
{
    /// <summary>
    /// Interaction logic for PersonPicture.xaml
    /// </summary>
    public partial class PersonPicture : UserControl
    {
        private TextBlock _initialsTextBlock;
        private Ellipse _pictureEllipse;
        private Ellipse _statusEllipse;

        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register("StatusText", typeof(string), typeof(PersonPicture), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(PersonPicture), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty PictureSourceProperty = DependencyProperty.Register("PictureSource", typeof(ImageSource), typeof(PersonPicture), new PropertyMetadata(default(ImageSource)));
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PersonPicture), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty StatusPictureSourceProperty = DependencyProperty.Register("StatusPictureSource", typeof(ImageSource), typeof(PersonPicture), new PropertyMetadata(default(ImageSource)));

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

        static PersonPicture()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PersonPicture), new FrameworkPropertyMetadata(typeof(PersonPicture)));
        }

        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            set => SetValue(StatusTextProperty, value);
        }

        public PersonPicture()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _initialsTextBlock = GetTemplateChild("InitialsTextBlock") as TextBlock;
            _initialsTextBlock.Background = Brushes.BlueViolet;
            _pictureEllipse = GetTemplateChild("PictureEllipse") as Ellipse;
            _statusEllipse = GetTemplateChild("StatusEllipse") as Ellipse;

            UpdateDisplay();
        }

        private static void OnPictureSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PersonPicture)d;
            control.UpdateDisplay();
        }

        private void UpdateDisplay()
        {
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
}
