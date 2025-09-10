using StarkCNC.Models;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentCoordinateSettingsView.xaml
    /// </summary>
    public partial class AdjustmentCoordinateSettingsView : Page
    {
        public AdjustmentParameters Adjustment { get; set; }

        public AdjustmentCoordinateSettingsView(AdjustmentParameters adjustment)
        {
            DataContext = this;

            InitializeComponent();
        }
    }
}
