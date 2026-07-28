using HelixToolkit.Geometry;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services;

internal static class PipeGenerator
{
    public static Element3D? GeneratePipeBend(ICollection<Point3D> positions, double diameter)
    {
        if (positions.Count < 2)
            return null;

        var builder = new MeshBuilder();
        int thetaDiv = 32;
        if (positions.Count < thetaDiv)
            thetaDiv = 16;

        builder.AddTube(positions.Select(p => p.ToVector3()).ToList(), (float)diameter, thetaDiv, false);
        return new MeshGeometryModel3D()
        {
            Geometry = builder.ToMeshGeometry3D(),
            Material = PhongMaterials.Green
        };
    }
}
