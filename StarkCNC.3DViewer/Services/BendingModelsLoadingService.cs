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

        public Model3DGroup? Bend { get; private set; }

        public Model3DGroup? Carriage { get; private set; }

        public Model3DGroup? Clamp { get; private set; }

        public Model3DGroup? Console { get; private set; }

        public Model3DGroup? Press { get; private set; }

        public Model3DGroup? Roller { get; private set; }

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
            if (model is null)
                return;

            model.Material = materialGroup;
            model.BackMaterial = materialGroup;
            model.SetName(type.ToString());

            switch (type)
            {
                case ModelType.Bend:
                    Bend = link;
                    ModelsGroup.Children.Add(Bend);
                    SetBendDefaultPosition(Bend);
                    break;
                case ModelType.Carriage:
                    Carriage = link;
                    ModelsGroup.Children.Add(Carriage);
                    SetCarriageDefaultPosition(Carriage);
                    break;
                case ModelType.Clamp:
                    Clamp = link;
                    ModelsGroup.Children.Add(Clamp);
                    SetClampDefaultPosition(Clamp);
                    break;
                case ModelType.Console:
                    Console = link;
                    ModelsGroup.Children.Add(Console);
                    SetConsoleDefaultPosition(Console);
                    break;
                case ModelType.Press:
                    Press = link;
                    ModelsGroup.Children.Add(Press);
                    SetPressDefaultPosition(Press);
                    break;
                case ModelType.Roller:
                    Roller = link;
                    ModelsGroup.Children.Add(Roller);
                    SetRollerDefaultPosition(Roller);
                    break;
            }
        }

        public ModelVisual3D GetModelVisual3D()
        {
            ModelVisual3D.Content = ModelsGroup;
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

        private void SetBendDefaultPosition(Model3DGroup bend)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(0, 0, 0);
            AxisAngleRotation3D angleRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90);
            RotateTransform3D rotateTransform = new RotateTransform3D(angleRotation, new Point3D(0, 0, 0));

            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            if (Console is not null)
            {
                transformGroup.Children.Add(Console.Transform);
            }

            bend.Transform = transformGroup;

            if (Clamp is not null)
            {
                SetClampDefaultPosition(Clamp);
            }
            if (Press is not null)
            {
                SetRollerDefaultPosition(Press);
            }
            if (Roller is not null)
            {
                SetRollerDefaultPosition(Roller);
            }
        }

        private void SetCarriageDefaultPosition(Model3DGroup carriage)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(0, -1000, 450);
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation, 0, 0, 0);

            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);

            carriage.Transform = transformGroup;
        }

        private void SetClampDefaultPosition(Model3DGroup clamp)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(-180, 0, 380);
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation, 1815, 0, 2125);

            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            if (Bend is not null)
            {
                transformGroup.Children.Add(Bend.Transform);
            }

            clamp.Transform = transformGroup;
        }

        private void SetConsoleDefaultPosition(Model3DGroup console)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(90, 0, 25);
            transformGroup.Children.Add(translateTransform);

            console.Transform = transformGroup;

            if (Bend is not null)
            {
                SetBendDefaultPosition(Bend);
            }
        }

        private void SetPressDefaultPosition(Model3DGroup press)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(-180, 0, 395);
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation, 2008, 0, 2125);

            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            if (Console is not null)
            {
                transformGroup.Children.Add(Console.Transform);
            }

            press.Transform = transformGroup;
        }

        private void SetRollerDefaultPosition(Model3DGroup carriage)
        {
            Transform3DGroup transformGroup = new Transform3DGroup();
            TranslateTransform3D translateTransform = new TranslateTransform3D(0, 0, 350);
            AxisAngleRotation3D axisAngleRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
            RotateTransform3D rotateTransform = new RotateTransform3D(axisAngleRotation, 60, 0, 2125);

            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            if (Bend is not null)
            {
                transformGroup.Children.Add(Bend.Transform);
            }

            carriage.Transform = transformGroup;
        }
    }
}
