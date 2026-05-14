using CommunityToolkit.Mvvm.ComponentModel;
using HelixToolkit.Wpf;
using StarkCNC.Core.Models;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public partial class PipeViewModel : ViewModelBase
{
    [ObservableProperty]
    private ModelVisual3D _pipe = new ModelVisual3D();

    public void UpdatePipeBend(ICollection<Point3D> positions, double diameter)
    {
        var tube = new TubeVisual3D
        {
            Path = new Point3DCollection(positions),
            Diameter = diameter,
            Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)),
            ThetaDiv = 32,
            IsPathClosed = false
        };

        //var builder = new MeshBuilder(true, true);
        //builder.AddTube(positions.ToList(), diameter, 32, false);
        //Pipe.Content = new GeometryModel3D(builder.ToMesh(), Materials.Green)); - Аналог
        Pipe.Content = tube.Content;

        OnPropertyChanged(nameof(Pipe));
    }

    public void UpdatePipeBend(ICollection<BendPositions> positions)
    {
        if (positions is null)
            return;

        var builder = new MeshBuilder(true, true);
        foreach (var pos in positions)
        {
            builder.AddCylinder(pos.StartPosition, pos.EndPosition, 60, 60);

            Pipe.Content = new GeometryModel3D(builder.ToMesh(), Materials.Red);
        }

        OnPropertyChanged(nameof(Pipe));
    }
}
