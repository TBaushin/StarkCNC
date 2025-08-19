using HelixToolkit.Wpf;
using StarkCNC.Core.Calculations;
using StarkCNC.Core.Models;
using StarkCNC.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ProgramView.xaml
    /// </summary>
    public partial class ProgramView : Page
    {
        private readonly ProgramViewModel ViewModel;

        public ProgramView(ProgramViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);

            //BendingView.Children.Add(ViewModel.GetPipe());
            UpdateBend();
        }

        public void UpdateBend()
        {
            var segments = new List<BendingData>
            {
                new BendingData { BendingRadius = 800, StraightLength = 400, BendingAngle = 90, RotationAngle = 0 },
                new BendingData { BendingRadius = 200, StraightLength = 700,  BendingAngle = 45, RotationAngle = -45 },
                new BendingData { BendingRadius = 300, StraightLength = 700,  BendingAngle = 20, RotationAngle = 90 },
                new BendingData { BendingRadius = 150, StraightLength = 400,  BendingAngle = 180, RotationAngle = 180 },
                new BendingData { BendingRadius = 500, StraightLength = 400,  BendingAngle = 45, RotationAngle = 45 },
                // Заполните по вашей таблице
                // Заполните по вашей таблице
            };
            var path = WireBuilder.BuildWirePath(segments);
            var mediaColor = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
            var brush = new SolidColorBrush(mediaColor);
            //var adjustment = _serviceProvider.GetRequiredService<AdjustmentViewModel>().SelectedAdjustment;
            double pipeDiameter = 50;
            //if (adjustment is not null)
            //pipeDiameter = adjustment.PipeDiameter;
            var tube = new TubeVisual3D
            {
                Path = new Point3DCollection(path),
                Diameter = segments.Count > 0 ? pipeDiameter : 5, //50- диаметр можно варьировать, если разный диаметр

                Fill = brush,
                ThetaDiv = 32,
                IsPathClosed = false
            };
            //BendingView.Children.Clear();
            var toRemove = BendingView.Children.OfType<TubeVisual3D>().ToList();
            foreach (var obj in toRemove)
                BendingView.Children.Remove(obj);

            BendingView.Children.Add(tube);
        }

        private void PipeBendParametersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var item = PipeBendParametersDataGrid.CurrentItem;
            if (item is not BendingData data)
                return;

            var value = NumberInputViewModel.ShowDialog();

            switch (PipeBendParametersDataGrid.CurrentColumn.DisplayIndex)
            {
                case 0:
                    data.StraightLength = value;
                    break;
                case 1:
                    data.BendingAngle = value;
                    break;
                case 2:
                    data.BendingRadius = value;
                    break;
                case 3:
                    data.RotationAngle = value;
                    break;
            }
        }

        private void PipeBendParametersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ViewModel.UpdateBend();
        }

        private void ZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BendingView.CameraController.Zoom(-0.1); // Не знаю, но отрицательное число приближает, а положительное отодвигает
        }

        private void ZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BendingView.CameraController.Zoom(0.1);
        }
    }
}
