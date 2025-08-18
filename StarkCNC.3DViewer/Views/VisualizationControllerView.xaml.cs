using HelixToolkit.Wpf;
using StarkCNC._3DViewer.ViewModels;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows;

namespace StarkCNC._3DViewer.Views
{
    public class TubeBendingAnimator : DependencyObject, IDisposable
    {
        public struct BendingSegment
        {
            public double StraightLength { get; set; }
            public double BendRadius { get; set; }
            public double BendAngle { get; set; }
            public double RotationAngle { get; set; }
            public BendingSegment(double straightLength, double bendRadius, double bendAngle, double rotationAngle)
            {
                StraightLength = straightLength;
                BendRadius = bendRadius;
                BendAngle = bendAngle;
                RotationAngle = rotationAngle;
            }
        }

        private class TubePoint
        {
            public Point3D Position { get; set; }
            public Point3D BasePosition { get; set; }
            public Vector3D Direction { get; set; }
            public Vector3D UpVector { get; set; }
            public double DistanceFromBack { get; set; }
            public bool IsFixed { get; set; }

            public TubePoint(Point3D position, Vector3D direction, Vector3D upVector, double distance)
            {
                Position = position;
                BasePosition = position;
                Direction = direction;
                UpVector = upVector;
                DistanceFromBack = distance;
                IsFixed = false;
            }

            public void UpdateBasePosition()
            {
                BasePosition = Position;
            }
        }

        private readonly HelixViewport3D viewport;
        private TubeVisual3D tubeVisual;
        private readonly List<TubePoint> tubePoints;
        private DispatcherTimer animationTimer;
        private List<BendingSegment> bendingProgram;
        private int currentSegmentIndex;
        private double animationProgress;
        private bool isAnimating;

        public double TubeRadius { get; set; } = 8.0;
        public double TotalTubeLength { get; set; } = 2000.0;
        public int TubeSegments { get; set; } = 200;
        public double AnimationSpeed { get; set; } = 0.008;
        public double BendHeight { get; set; } = 500.0;

        private double currentFeedLength = 0.0;
        private Vector3D currentDirection = new Vector3D(0, 1, 0);
        private Vector3D currentUpVector = new Vector3D(0, 0, 1);

        // Точка гиба (X=0, Z=500)
        private readonly Point3D bendPoint = new Point3D(0, 0, 500);

        public event EventHandler<string> AnimationPhaseChanged;
        public event EventHandler AnimationCompleted;

        public TubeBendingAnimator(HelixViewport3D viewport)
        {
            this.viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            tubePoints = new List<TubePoint>();
            InitializeTube();
            InitializeTimer();
        }

        private void InitializeTube()
        {
            GenerateInitialStraightTube();
            tubeVisual = new TubeVisual3D
            {
                Diameter = TubeRadius * 2,
                ThetaDiv = 24,
                Fill = Brushes.Gray
            };
            UpdateTubeVisual();
            viewport.Children.Add(tubeVisual);
        }

        private void GenerateInitialStraightTube()
        {
            tubePoints.Clear();
            double segmentLength = TotalTubeLength / TubeSegments;
            for (int i = 0; i <= TubeSegments; i++)
            {
                double yPos = -i * segmentLength; // Труба вниз по Y от 0
                Point3D position = new Point3D(0, yPos, BendHeight);
                tubePoints.Add(new TubePoint(position, currentDirection, currentUpVector, i * segmentLength));
            }
            currentFeedLength = 0.0;
        }

        private void InitializeTimer()
        {
            animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            animationTimer.Tick += OnAnimationTick;
        }

        public void StartBendingAnimation(List<BendingSegment> program)
        {
            if (program is null || program.Count == 0)
                throw new ArgumentException("Программа гибки не может быть пустой.", nameof(program));
            bendingProgram = new List<BendingSegment>(program);
            currentSegmentIndex = 0;
            animationProgress = 0.0;
            isAnimating = true;
            ResetToInitialState();
            AnimationPhaseChanged?.Invoke(this, $"Начало гибки сегмента {currentSegmentIndex + 1}");
            animationTimer.Start();
        }

        public void StopAnimation()
        {
            isAnimating = false;
            animationTimer?.Stop();
        }

        public void ResetToInitialState()
        {
            currentFeedLength = 0.0;
            currentDirection = new Vector3D(0, 1, 0);
            currentUpVector = new Vector3D(0, 0, 1);
            GenerateInitialStraightTube();
            UpdateTubeVisual();
        }

        private void OnAnimationTick(object sender, EventArgs e)
        {
            if (!isAnimating || currentSegmentIndex >= bendingProgram.Count)
            {
                animationTimer.Stop();
                isAnimating = false;
                AnimationCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }

            var segment = bendingProgram[currentSegmentIndex];

            if (animationProgress < 0.4)
                AnimateTubeFeed(segment);
            else if (animationProgress < 0.8)
                AnimateBending(segment);
            else if (animationProgress < 1.0)
                AnimateRotation(segment);

            animationProgress += AnimationSpeed;

            if (animationProgress >= 1.0)
            {
                CompleteSegment(segment);
                currentSegmentIndex++;
                animationProgress = 0.0;
                if (currentSegmentIndex >= bendingProgram.Count)
                {
                    isAnimating = false;
                    animationTimer.Stop();
                    AnimationPhaseChanged?.Invoke(this, "Гибка завершена");
                    AnimationCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    AnimationPhaseChanged?.Invoke(this, $"Начало гибки сегмента {currentSegmentIndex + 1}");
                }
            }

            UpdateTubeVisual();
        }

        private void AnimateTubeFeed(BendingSegment segment)
        {
            double phase = animationProgress / 0.4;
            double feedDistance = segment.StraightLength * phase;
            Vector3D movement = currentDirection * feedDistance;

            foreach (var pt in tubePoints)
            {
                if (!pt.IsFixed && pt.DistanceFromBack > currentFeedLength)
                    pt.Position = pt.BasePosition + movement;
            }
        }

        private void AnimateBending(BendingSegment segment)
        {
            if (Math.Abs(segment.BendAngle) < 0.01 || segment.BendRadius < 0.01)
                return;

            double phase = (animationProgress - 0.4) / 0.4;
            double bendAngleRad = segment.BendAngle * Math.PI / 180.0 * phase;

            Vector3D bendAxis = currentUpVector;
            if (segment.BendAngle < 0)
                bendAxis = -bendAxis;
            bendAxis.Normalize();

            double bendStart = currentFeedLength + segment.StraightLength;
            Point3D bendCenter = new Point3D(0, bendStart, BendHeight) + bendAxis * segment.BendRadius;

            foreach (var pt in tubePoints)
            {
                if (pt.IsFixed || pt.DistanceFromBack < currentFeedLength)
                    continue;

                double dist = pt.DistanceFromBack - currentFeedLength;

                if (dist <= segment.BendRadius)
                {
                    double localProgress = dist / segment.BendRadius;
                    double localAngle = bendAngleRad * localProgress;
                    Point3D startPoint = new Point3D(0, bendStart, BendHeight);
                    Vector3D radiusVec = startPoint - bendCenter;
                    Matrix3D rot = CreateRotationMatrix(bendAxis, localAngle);
                    Vector3D rotatedRadius = rot.Transform(radiusVec);
                    pt.Position = bendCenter + rotatedRadius;
                }
                else
                {
                    double straightDist = dist - segment.BendRadius;
                    Point3D startPoint = new Point3D(0, bendStart, BendHeight);
                    Vector3D radiusVec = startPoint - bendCenter;
                    Matrix3D rot = CreateRotationMatrix(bendAxis, bendAngleRad);
                    Vector3D rotatedRadius = rot.Transform(radiusVec);
                    Point3D arcEnd = bendCenter + rotatedRadius;
                    Vector3D endDir = rot.Transform(currentDirection);
                    pt.Position = arcEnd + (endDir * straightDist);
                }
            }

            if (phase >= 0.99)
            {
                Matrix3D finalRot = CreateRotationMatrix(bendAxis, segment.BendAngle * Math.PI / 180.0);
                currentDirection = finalRot.Transform(currentDirection);
                currentUpVector = finalRot.Transform(currentUpVector);
                currentDirection.Normalize();
                currentUpVector.Normalize();
            }
        }

        private void AnimateRotation(BendingSegment segment)
        {
            if (Math.Abs(segment.RotationAngle) < 0.01)
                return;

            double phase = (animationProgress - 0.8) / 0.2;
            double rotationAngleRad = segment.RotationAngle * Math.PI / 180.0 * phase;

            double rotationStart = currentFeedLength + segment.StraightLength;
            Point3D rotationCenter = new Point3D(0, rotationStart, BendHeight);

            Vector3D rotationAxis = currentDirection;
            Matrix3D rotationMatrix = CreateRotationMatrix(rotationAxis, rotationAngleRad);

            foreach (var pt in tubePoints)
            {
                if (pt.IsFixed || pt.DistanceFromBack < currentFeedLength)
                    continue;

                Vector3D rel = pt.Position - rotationCenter;
                Vector3D rotatedRel = rotationMatrix.Transform(rel);
                pt.Position = rotationCenter + rotatedRel;

                pt.UpVector = rotationMatrix.Transform(pt.UpVector);
                pt.Direction = rotationMatrix.Transform(pt.Direction);
            }

            if (phase >= 0.99)
            {
                Matrix3D finalRot = CreateRotationMatrix(rotationAxis, segment.RotationAngle * Math.PI / 180.0);
                currentUpVector = finalRot.Transform(currentUpVector);
                currentUpVector.Normalize();
            }
        }

        private void CompleteSegment(BendingSegment segment)
        {
            double newFeedLength = currentFeedLength + segment.StraightLength;
            foreach (var pt in tubePoints)
            {
                if (pt.DistanceFromBack <= newFeedLength)
                {
                    pt.IsFixed = true;
                    pt.UpdateBasePosition();
                }
            }
            currentFeedLength = newFeedLength;
        }

        private void UpdateTubeVisual()
        {
            if (tubePoints.Count < 2)
                return;

            var pathPoints = new Point3DCollection(tubePoints.Select(pt => pt.Position));
            if (Application.Current?.Dispatcher is not null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tubeVisual.Path = pathPoints;
                }), DispatcherPriority.Render);
            }
        }

        private static Matrix3D CreateRotationMatrix(Vector3D axis, double angle)
        {
            if (axis.Length < 1e-10)
                return Matrix3D.Identity;

            axis.Normalize();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double oneMinusCos = 1.0 - cos;
            double x = axis.X, y = axis.Y, z = axis.Z;

            return new Matrix3D(
                cos + x * x * oneMinusCos, x * y * oneMinusCos - z * sin, x * z * oneMinusCos + y * sin, 0,
                y * x * oneMinusCos + z * sin, cos + y * y * oneMinusCos, y * z * oneMinusCos - x * sin, 0,
                z * x * oneMinusCos - y * sin, z * y * oneMinusCos + x * sin, cos + z * z * oneMinusCos, 0,
                0, 0, 0, 1); // TODO: Откуда эти формулы? Где их можно посмотреть, почитать о них?
        }

        public static List<BendingSegment> CreateSampleProgram()
        {
            return new List<BendingSegment>
            {
                new BendingSegment(300, 100, 90, 0),
                new BendingSegment(200, 80, -45, 90),
                new BendingSegment(250, 120, 60, -45),
                new BendingSegment(150, 0, 0, 180),
            };
        }

        public void Dispose()
        {
            animationTimer?.Stop();
            if (tubeVisual != null && viewport.Children.Contains(tubeVisual))
                viewport.Children.Remove(tubeVisual);
            tubePoints.Clear();
        }
    }

    public partial class VisualizationControllerView : UserControl
    {
        private readonly VisualizationControllerViewModel ViewModel;
        private TubeBendingAnimator _animator;

        public VisualizationControllerView(VisualizationControllerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
            InitializeAnimation();
            BendingView.RotateGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.RightClick);
            BendingView.PanGesture = new System.Windows.Input.MouseGesture(System.Windows.Input.MouseAction.LeftClick);
            BendingView.Children.Add(ViewModel.GetModels());
            
            SetDefaultValue();

            //BendingView.Children.Add(ViewModel.GetPipe());
        }

        private void InitializeAnimation()
        {
            // Инициализируем аниматор с viewport
            _animator = new TubeBendingAnimator(BendingView);

            // Подписываемся на события
            _animator.AnimationPhaseChanged += (sender, phase) =>
            {
               // StatusTextBlock.Text = phase;
            };

            _animator.AnimationCompleted += (sender, e) =>
            {
                //StatusTextBlock.Text = "Анимация завершена";
                //StartButton.IsEnabled = true;
            };
        }

        private void StartAnimation_Click(object sender, RoutedEventArgs e)
        {
            var program = TubeBendingAnimator.CreateSampleProgram();
            _animator.StartBendingAnimation(program);
            //StartButton.IsEnabled = false;
        }

        private void ResetAnimation_Click(object sender, RoutedEventArgs e)
        {
            _animator.StopAnimation();
            _animator.ResetToInitialState();
           // StatusTextBlock.Text = "Готов к анимации";
            //StartButton.IsEnabled = true;
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_animator is not null)
            {
                _animator.AnimationSpeed = e.NewValue;
            }
        }

        /*protected override void OnClosed(EventArgs e)
        {
            _animator?.Dispose();
            base.OnClosed(e);
        }*/

        private void SetDefaultValue()
        {
            Dictionary<string, double> positions = ViewModel.GetDefaults();
            ConsoleSlider.Value = positions["console"];
            BendSlider.Value = positions["bend"];
            SupplySlider.Value = positions["carriage"];
            HeightSlider.Value = positions["height"];
            ClampSlider.Value = positions["clamp"];
            PressSlider.Value = positions["press"];
        }

        private void Sliders_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.UpdatePositions(
                ConsoleSlider.Value,
                BendSlider.Value,
                SupplySlider.Value,
                HeightSlider.Value,
                ClampSlider.Value,
                PressSlider.Value
            );
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


