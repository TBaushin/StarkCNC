using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ManualView.xaml
    /// </summary>
    public partial class ManualView : Page
    {
        public ManualView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => Char.IsNumber(c) || c == '.' || c == ','); // Only digit and point for float
            base.OnPreviewTextInput(e);
        }
    }
}
