using HelixToolkit.Wpf;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC._3DViewer.Services
{
    public class BendingModelsLoadingService : IBendingModelsLoadingService
    {
        public ModelVisual3D ModelVisual3D { get; private set; } = new ModelVisual3D();

        public Model3DGroup ModelsGroup { get; private set; }

        public GeometryModel3D Bend { get; private set; }

        public GeometryModel3D Carriage { get; private set; }

        public GeometryModel3D Clamp { get; private set; }

        public GeometryModel3D Console { get; private set; }

        public GeometryModel3D Press { get; private set; }

        public BendingModelsLoadingService()
        {
            ModelsGroup = new Model3DGroup();
        }

        public void Load(string path)
        {
            ModelImporter modelImporter = new ModelImporter();

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var materialGroup = new MaterialGroup();
                SetMaterial(materialGroup);

                var link = modelImporter.Load(file);
                GeometryModel3D? model = link.Children[0] as GeometryModel3D;
                if (model is null) continue;

                model.Material = materialGroup;
                model.BackMaterial = materialGroup;
                ModelsGroup.Children.Add(model);
            }
            ModelVisual3D.Content = ModelsGroup;
        }

        public void Load(string path, ModelType type)
        {
            ModelImporter modelImporter = new ModelImporter();

            var materialGroup = new MaterialGroup();
            SetMaterial(materialGroup);

            var link = modelImporter.Load(path);
            GeometryModel3D? model = link.Children[0] as GeometryModel3D;
            if (model is null) return;

            model.Material = materialGroup;
            model.BackMaterial = materialGroup;

            switch (type)
            {
                case ModelType.Bend:
                    Bend = model;
                    ModelsGroup.Children.Add(Bend);
                    break;
                case ModelType.Carriage:
                    Carriage = model;
                    ModelsGroup.Children.Add(Carriage);
                    break;
                case ModelType.Clamp:
                    Clamp = model;
                    ModelsGroup.Children.Add(Clamp);
                    break;
                case ModelType.Console:
                    Console = model;
                    ModelsGroup.Children.Add(Console);
                    break;
                case ModelType.Press:
                    Press = model;
                    ModelsGroup.Children.Add(Press);
                    break;
            }

            ModelVisual3D.Content = ModelsGroup;
        }

        public ModelVisual3D GetModelVisual3D()
        {
            return ModelVisual3D;
        }

        private void SetMaterial(MaterialGroup materialGroup)
        { 
            Color mainColor = Colors.White;
            Color secondColor = Colors.Gray;
            Color thirdColor = Colors.Blue;
            EmissiveMaterial emissiveMaterial = new EmissiveMaterial(new SolidColorBrush(mainColor));
            DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(secondColor));
            SpecularMaterial specularMaterial = new SpecularMaterial(new SolidColorBrush(thirdColor), 200);
            materialGroup.Children.Add(emissiveMaterial);
            materialGroup.Children.Add(diffuseMaterial);
            materialGroup.Children.Add(specularMaterial);
        }
    }
}
