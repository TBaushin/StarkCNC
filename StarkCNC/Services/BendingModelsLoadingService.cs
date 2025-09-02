using HelixToolkit.Wpf;
using StarkCNC.Core.Models;
using StarkCNC.Models;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StarkCNC.Services
{
    public class BendingModelsLoadingService : IBendingModelsLoadingService, INotifyPropertyChanged
    {
        public ModelVisual3D Pipe { get; private set; } = new ModelVisual3D();

        public ModelVisual3D Model { get; private set; } = new ModelVisual3D();

        public Model3DGroup ModelsGroup { get; private set; }

        public Model? Bend { get; private set; }

        public Model? Carriage { get; private set; }

        public Model? Clamp { get; private set; }

        public Model? Console { get; private set; }

        public Model? Press { get; private set; }

        public Model? Roller { get; private set; }

        public BendingModelsLoadingService()
        {
            ModelsGroup = new Model3DGroup();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
            Model.Content = ModelsGroup;
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
                    Bend = new Model();
                    Bend.Figure = link;
                    ModelsGroup.Children.Add(Bend.Figure);
                    SetBendDefaultPosition(Bend);
                    break;
                case ModelType.Carriage:
                    Carriage = new Model();
                    Carriage.Figure = link;
                    ModelsGroup.Children.Add(Carriage.Figure);
                    SetCarriageDefaultPosition(Carriage);
                    break;
                case ModelType.Clamp:
                    Clamp = new Model();
                    Clamp.Figure = link;
                    ModelsGroup.Children.Add(Clamp.Figure);
                    SetClampDefaultPosition(Clamp);
                    break;
                case ModelType.Console:
                    Console = new Model();
                    Console.Figure = link;
                    ModelsGroup.Children.Add(Console.Figure);
                    SetConsoleDefaultPosition(Console);
                    break;
                case ModelType.Press:
                    Press = new Model();
                    Press.Figure = link;
                    ModelsGroup.Children.Add(Press.Figure);
                    SetPressDefaultPosition(Press);
                    break;
                case ModelType.Roller:
                    Roller = new Model();
                    Roller.Figure = link;
                    ModelsGroup.Children.Add(Roller.Figure);
                    SetRollerDefaultPosition(Roller);
                    break;
            }
        }

        public ModelVisual3D GetModelVisual3D()
        {
            Model.Content = ModelsGroup;
            return Model;
        }

        public Dictionary<string, double> GetDefault()
        {
            var posDefault = new Dictionary<string, double>();
            posDefault.Add("console", 90);
            posDefault.Add("height", 25);
            posDefault.Add("bend", 90);
            posDefault.Add("carriage", 1000);
            posDefault.Add("clamp", 0);
            posDefault.Add("press", 0);
            return posDefault;
        }

        public void UpdatePositions(double consolePosX, double bendRotationZ, double carriagePosY, double height, double clampPosX, double pressPosX)
        {
            UpdateConsolePosition(consolePosX, height);
            UpdatePressPosition(pressPosX);
            UpdateBendPosition(bendRotationZ);
            UpdateClampPosition(clampPosX);
            UpdateRollerPosition();
            UpdateCarriagePosition(carriagePosY);
        }

        public void UpdatePipeBend(ICollection<BendPositions> positions)
        {
            var builder = new MeshBuilder(true, true);
            foreach (var pos in positions)
            {
                builder.AddCylinder(pos.StartPosition, pos.EndPosition, 60, 60);

                Pipe.Content = new GeometryModel3D(builder.ToMesh(), Materials.Red);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pipe)));
        }

        public void UpdatePipeBend(ICollection<Point3D> positions, double diameter)
        {
            var tube = new TubeVisual3D
            {
                Path = new Point3DCollection(positions),
                Diameter = diameter,
                Fill = new SolidColorBrush(Color.FromArgb(255,0,255,0)),
                ThetaDiv = 32,
                IsPathClosed = false
            };

            //var builder = new MeshBuilder(true, true);
            //builder.AddTube(positions.ToList(), diameter, 32, false);
            //Pipe.Content = new GeometryModel3D(builder.ToMesh(), Materials.Green)); - Аналог
            Pipe.Content = tube.Content;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pipe)));
        }

        public Coordinates? GetModelPosition(ModelType modelType)
        {
            switch (modelType)
            {
                case ModelType.Bend:
                    if (Bend is null)
                        return null;
                    return Bend.Coordinates;
                case ModelType.Carriage:
                    if (Carriage is null) 
                        return null;
                    return Carriage.Coordinates;
                case ModelType.Clamp:
                    if (Clamp is null) 
                        return null;
                    return Clamp.Coordinates;
                case ModelType.Console:
                    if (Console is null) 
                        return null;
                    return Console.Coordinates;
                case ModelType.Press:
                    if (Press is null) 
                        return null;
                    return Press.Coordinates;
                case ModelType.Roller:
                    if (Roller is null)
                        return null;
                    return Roller.Coordinates;
                default:
                    return null;
            }
        }

        private static void SetMaterial(MaterialGroup materialGroup)
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

        private void SetBendDefaultPosition(Model bend)
        {
            bend.Coordinates.SetPosition(0, 0, 0);
            bend.Coordinates.SetRotation(0, 0, 0);
            bend.Axis = new Vector3D(0, 0, 1);
            bend.UpdateTransform(-90, Console);

            if (Clamp is not null)
            {
                SetClampDefaultPosition(Clamp);
            }
            if (Roller is not null)
            {
                SetRollerDefaultPosition(Roller);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bend)));
        }

        private void UpdateBendPosition(double bendRotationZ)
        {
            if (Bend is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Bend.Coordinates.SetPosition(0, 0, 0);
            Bend.Coordinates.SetRotation(0, 0, 0);
            Bend.Axis = new Vector3D(0, 0, 1);
            Bend.UpdateTransform(-bendRotationZ, Console);
            Bend.Coordinates.RotationZ = -bendRotationZ;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bend)));
        }

        private void SetCarriageDefaultPosition(Model carriage)
        {
            carriage.Coordinates.SetPosition(0, -1000, 450);
            carriage.Coordinates.SetRotation(0, 0, 0);
            carriage.Axis = new Vector3D(0, 1, 0);
            carriage.UpdateTransform();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Carriage)));
        }

        private void UpdateCarriagePosition(double carriagePosY)
        {
            if (Carriage is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Carriage.Coordinates.SetPosition(0, carriagePosY - 3000, 450);
            Carriage.Coordinates.SetRotation(0, 0, 0);
            Carriage.Axis = new Vector3D(0, 1, 0);
            Carriage.UpdateTransform();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Carriage)));
        }

        private void SetClampDefaultPosition(Model clamp)
        {
            clamp.Coordinates.SetPosition(-180, 0, 380);
            clamp.Coordinates.SetRotation(1815, 0, 2125);
            clamp.Axis = new Vector3D(0, 1, 0);
            clamp.UpdateTransform(arountTransform: Bend);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clamp)));
        }

        private void UpdateClampPosition(double clampPosX)
        {
            if (Clamp is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Clamp.Coordinates.SetPosition(-180 - clampPosX, 0, 380);
            Clamp.Coordinates.SetRotation(1815, 0, 2125);
            Clamp.Axis = new Vector3D(0, 0, 1);
            Clamp.UpdateTransform(arountTransform: Bend);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clamp)));
        }

        private void SetConsoleDefaultPosition(Model console)
        {
            console.Coordinates.SetPosition(90, 0, 25);
            console.Coordinates.SetRotation(0, 0, 0);
            console.Axis = new Vector3D();
            console.UpdateTransform();

            if (Bend is not null)
            {
                SetBendDefaultPosition(Bend);
            }
            if (Press is not null)
            {
                SetRollerDefaultPosition(Press);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Console)));
        }

        private void UpdateConsolePosition(double consolePosX, double heigth)
        {
            if (Console is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Console.Coordinates.SetPosition(consolePosX, 0, heigth);
            Console.UpdateTransform();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Console)));
        }

        private void SetPressDefaultPosition(Model press)
        {
            press.Coordinates.SetPosition(-180, 0, 395);
            press.Coordinates.SetRotation(2008, 0, 2125);
            press.Axis = new Vector3D(1, 0, 0);
            press.UpdateTransform(arountTransform: Console);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Press)));
        }

        private void UpdatePressPosition(double pressPosX)
        {
            if (Press is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Press.Coordinates.SetPosition(-180 - pressPosX, 0, 395);
            Press.Coordinates.SetRotation(2008, 0, 2125);
            Press.Axis = new Vector3D(1, 0, 0);
            Press.UpdateTransform(arountTransform: Console);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Press)));
        }

        private void SetRollerDefaultPosition(Model roller)
        {
            roller.Coordinates.SetPosition(0, 0, 350);
            roller.Coordinates.SetRotation(60, 0, 2125);
            roller.Axis = new Vector3D(1, 0, 0);
            roller.UpdateTransform(arountTransform: Bend);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Roller)));
        }

        private void UpdateRollerPosition()
        {
            if (Roller is null)
                return; // TODO: Когда будет логирование или глобальная обработка ошибок с выводом информации пользователю

            Roller.Coordinates.SetPosition(0, 0, 350);
            Roller.Coordinates.SetRotation(0, 0, 0);
            Roller.Axis = new Vector3D(0, 0, 1);
            Roller.UpdateTransform(arountTransform: Bend);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Roller)));
        }
    }
}
