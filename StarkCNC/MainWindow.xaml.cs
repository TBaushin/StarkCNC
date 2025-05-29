using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace StarkCNC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowChrome.SetWindowChrome(this,
                new WindowChrome
                {
                    CaptionHeight = 50,
                    CornerRadius = new CornerRadius(12),
                    GlassFrameThickness = new Thickness(-1),
                    ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                    UseAeroCaptionButtons = true,
                    NonClientFrameEdges = SystemParameters.HighContrast ? NonClientFrameEdges.None :
                        NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
                }
            );

            WindowChrome ws = WindowChrome.GetWindowChrome(this);
            ws.NonClientFrameEdges = SystemParameters.HighContrast ? NonClientFrameEdges.None :
                        NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left;
        }
    }
}