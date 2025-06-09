using Calculation;
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

        public void UpdatePositions(double consolePosX, double bendRotationZ, double carriagePosY, double height, double clampPosX, double pressPosX)
        {
            UpdateConsolePosition(consolePosX, height);
            UpdatePressPosition(pressPosX);
            UpdateBendPosition(bendRotationZ);
            UpdateClampPosition(clampPosX, bendRotationZ);
            UpdateRollerPosition(bendRotationZ);
            UpdateCarriagePosition(carriagePosY);
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
            Vector3D axis = new Vector3D(0, 0, 1);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(0, 0, 0)
                .CalculateRotation(0, 0, 0, axis, -90);

            if (Console is not null)
            {
                calculations.SetObjectTransformAround(Console.Transform);
            }

            bend.Transform = calculations.GetResult();

            if (Clamp is not null)
            {
                SetClampDefaultPosition(Clamp);
            }
            if (Roller is not null)
            {
                SetRollerDefaultPosition(Roller);
            }
        }

        private void UpdateBendPosition(double bendRotationZ)
        {
            Vector3D axis = new Vector3D(0, 0, 1);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(0, 0, 0)
                .CalculateRotation(0, 0, 0, axis, -bendRotationZ);

            if (Console is not null)
            {
                calculations.SetObjectTransformAround(Console.Transform);
            }

            Bend.Transform = calculations.GetResult();
        }

        private void SetCarriageDefaultPosition(Model3DGroup carriage)
        {
            Vector3D axis = new Vector3D(0, 1, 0);
            carriage.Transform = new ModelsTransformCalculation()
                .CalculateTransform(0, -1000, 450)
                .CalculateRotation(0, 0, 0, axis, 0)
                .GetResult();
        }

        private void UpdateCarriagePosition(double carriagePosX)
        {
            Vector3D axis = new Vector3D(0, 1, 0);
            Carriage.Transform = new ModelsTransformCalculation()
                .CalculateTransform(carriagePosX, -1000, 450)
                .CalculateRotation(0, 0, 0, axis, 0)
                .GetResult();
        }

        private void SetClampDefaultPosition(Model3DGroup clamp)
        {
            Vector3D axis = new Vector3D(0, 1, 0);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(-180, 0, 380)
                .CalculateRotation(1815, 0, 2125, axis, 0);

            if (Bend is not null)
            {
                calculations.SetObjectTransformAround(Bend.Transform);
            }

            clamp.Transform = calculations.GetResult();
        }

        private void UpdateClampPosition(double clampPosX, double rotationClampZ)
        {
            Vector3D axis = new Vector3D(0, 1, 0);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(-180, 0, 380)
                .CalculateRotation(1815, 0, 2125, axis, rotationClampZ);

            if (Bend is not null)
            {
                calculations.SetObjectTransformAround(Bend.Transform);
            }

            Clamp.Transform = calculations.GetResult();
        }

        private void SetConsoleDefaultPosition(Model3DGroup console)
        {
            console.Transform = new ModelsTransformCalculation()
                .CalculateTransform(90, 0, 25)
                .GetResult();

            if (Bend is not null)
            {
                SetBendDefaultPosition(Bend);
            }
            if (Press is not null)
            {
                SetRollerDefaultPosition(Press);
            }
        }

        private void UpdateConsolePosition(double consolePosX, double heigth)
        {
            Console.Transform = new ModelsTransformCalculation()
                .CalculateTransform(consolePosX, 0, heigth)
                .GetResult();
        }

        private void SetPressDefaultPosition(Model3DGroup press)
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(-180, 0, 395)
                .CalculateRotation(2008, 0, 2125, axis, 0);

            if (Console is not null)
            {
                calculations.SetObjectTransformAround(Console.Transform);
            }

            press.Transform = calculations.GetResult();
        }

        private void UpdatePressPosition(double pressPosX)
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(-180, 0, 395)
                .CalculateRotation(pressPosX, 0, 2125, axis, 0);

            if (Console is not null)
            {
                calculations.SetObjectTransformAround(Console.Transform);
            }

            Press.Transform = calculations.GetResult();
        }

        private void SetRollerDefaultPosition(Model3DGroup roller)
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            ModelsTransformCalculation calculations = new ModelsTransformCalculation()
                .CalculateTransform(0, 0, 350)
                .CalculateRotation(60, 0, 2125, axis, 0);

            if (Bend is not null)
            {
                calculations.SetObjectTransformAround(Bend.Transform);
            }

            roller.Transform = calculations.GetResult();
        }

        private void UpdateRollerPosition(double rollerRotationZ) 
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            ModelsTransformCalculation calculation = new ModelsTransformCalculation()
                .CalculateTransform(0, 0, 350)
                .CalculateRotation(0, 0, 0, axis, rollerRotationZ);

            if (Bend is not null)
            {
                calculation.SetObjectTransformAround(Bend.Transform);
            }

            Roller.Transform = calculation.GetResult();
        }
    }
}
