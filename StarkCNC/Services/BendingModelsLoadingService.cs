using HelixToolkit.Wpf;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services
{
    public class BendingModelsLoadingService : IBendingModelsLoadingService
    {
        public ModelVisual3D ModelVisual3D { get; private set; } = new ModelVisual3D();

        public Model3DGroup ModelsGroup { get; private set; }

        public BendingModelsLoadingService() 
        {
            ModelsGroup = new Model3DGroup();
        }

        public void Load()
        {
            var modelsPathSection = App.Configuration.GetValue(typeof(string), "ModelsPath") as string;
            if (modelsPathSection is null) return;

            Load(modelsPathSection);
        }

        public void Load(string path)
        {
            ModelsGroup.Children.Clear();

            ModelImporter modelImporter = new ModelImporter();

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var materialGroup = new MaterialGroup();
                Color mainColor = Colors.White;
                Color secondColor = Colors.Gray;
                Color thirdColor = Colors.Blue;
                EmissiveMaterial emissiveMaterial = new EmissiveMaterial(new SolidColorBrush(mainColor));
                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(secondColor));
                SpecularMaterial specularMaterial = new SpecularMaterial(new SolidColorBrush(thirdColor), 200);
                materialGroup.Children.Add(emissiveMaterial);
                materialGroup.Children.Add(diffuseMaterial);
                materialGroup.Children.Add(specularMaterial);

                var link = modelImporter.Load(file);
                GeometryModel3D? model = link.Children[0] as GeometryModel3D;
                if (model is null) continue;

                model.Material = materialGroup;
                model.BackMaterial = materialGroup;
                ModelsGroup.Children.Add(model);
            }
            ModelVisual3D.Content = ModelsGroup;
        }

        public ModelVisual3D GetModelVisual3D()
        {
            return ModelVisual3D;
        }
    }
}
