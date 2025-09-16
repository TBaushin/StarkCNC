using StarkCNC.DTO;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentCoordinateSettingsView.xaml
    /// </summary>
    public partial class AdjustmentCoordinateSettingsView : Page
    {
        public AdjustmentParametersDto Adjustment { get; set; }

        public AdjustmentCoordinateSettingsView(AdjustmentParametersDto adjustment)
        {
            DataContext = this;

            InitializeComponent();
        }
    }
}
