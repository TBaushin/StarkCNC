using OpcUaHelper;
using System.Windows;
using System.Windows.Controls;

namespace StarkCNC.Views
{
    /// <summary>
    /// Interaction logic for ManualView.xaml
    /// </summary>
    public partial class ManualView : Page
    {
        const string plc = "ns=4;s=|var|HCQ0-1200D-1.04.00.04.Application.";// Могут отличаться от станка к станку
        public ManualView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => Char.IsNumber(c) || c == '.' || c == ','); // Only digit and point for float
            base.OnPreviewTextInput(e);
        }
        private OpcUaClient opcUaClient = new OpcUaClient();

        //Асинхронный метод, вызываемый при загрузке формы
        //В методе осуществляется подключение к Opc Ua серверу: opc.tcp://127.0.0.1:4840/Opc
        private async void Connect(object sender, EventArgs e)
        {
            //Авторизация (при необходимости)

            try
            {
                //Подключение к серверу
                await opcUaClient.ConnectServer("opc.tcp://192.168.88.100:4840"); // Могут отличаться от станка к станку
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDiscoverServer_Click(object sender, EventArgs e)
        {
            // Получение адреса зарегистрированного сервера
            //string endpointUrl = new Opc.Ua.Client.Control.DiscoverServerDlg().ShowDialog(opcUaClient.AppConfig, null);

            // Получить адрес, зарегистрированный другими серверами. Обратите внимание, что политика безопасности IP должна быть настроена правильно
            // string endpointUrl = new Opc.Ua.Client.Controls.DiscoverServerDlg( ).ShowDialog( opcUaClient.AppConfig, "192.168.0.100" );

           // if (!string.IsNullOrEmpty(endpointUrl))
            {
                // Получить адрес сервера, которым нужно управлять
            }
        }
        /// <summary>
        /// Операция чтения узла
        /// </summary>
        private void btnReadSingleNode_Click(object sender, EventArgs e)
        {
            try
            {
               //float value = opcUaClient.ReadNode<float>("ns=4;s=|var|HCQ0-1200D-1.04.00.04.Application.GVL.Zadanie_Speed");
                //string value = opcUaClient.ReadNode<string>("ns=4;|var|HCQ0-1200D-1.04.00.04.Application.GVL.bg");
                float val = 15F;
                opcUaClient.WriteNode<float>(plc+"GVL.Zadanie_Speed",val);
                opcUaClient.WriteNode<float>(plc + "Privoda.ConsolSpeed", val);
                opcUaClient.WriteNode<float>(plc + "Privoda.PodachaSpeed", val);
                opcUaClient.WriteNode<float>(plc + "Privoda.PovorotSpeed", val);
                double koordPodacha = opcUaClient.ReadNode<double>(plc + "IoConfig_Globals.PodachaNUM.fActPosition");
                double koordConsol = opcUaClient.ReadNode<double>(plc + "IoConfig_Globals.ConsolNUM.fActPosition");
                double koordPovorot = opcUaClient.ReadNode<double>(plc + "IoConfig_Globals.PovorotNUM.fActPosition");
                // MessageBox.Show($"Скорость {value} %");
            }
            catch (Exception ex)
            {
               
            }
        }
        private void ZazhimFR(object sender, EventArgs e)
        {
            try
            {
                //float value = opcUaClient.ReadNode<float>("ns=4;s=|var|HCQ0-1200D-1.04.00.04.Application.GVL.Zadanie_Speed");
                
                //string value = opcUaClient.ReadNode<string>("ns=4;|var|HCQ0-1200D-1.04.00.04.Application.GVL.bg");
                bool tr = true;

                opcUaClient.WriteNode<bool>(plc + "Privoda.ConsolBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PovorotBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PodachaBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.Obnulenie_podacha", tr);

                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZashimNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.PrishimNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.DornNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.Doshim1Nazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZangaRazjim", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.ConsolForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PodachaForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PovorotForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZashimVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.PrishimVpered", tr);

                opcUaClient.WriteNode<bool>(plc + "Vihoda.Dishim1Vpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibDozhim", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZangaZajim", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.VKL_Gidro1", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.VKL_Gidro2", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.OsnVniz", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.OsnVverh", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.DornVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.SmazkaDorn", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.Poddergka_naladka", tr);
                // MessageBox.Show($"Скорость {value} %");
            }
            catch (Exception ex)
            {

            }
        }
        private void ZazhimR(object sender, EventArgs e)
        {
            try
            {
                //float value = opcUaClient.ReadNode<float>("ns=4;s=|var|HCQ0-1200D-1.04.00.04.Application.GVL.Zadanie_Speed");
                //string value = opcUaClient.ReadNode<string>("ns=4;|var|HCQ0-1200D-1.04.00.04.Application.GVL.bg");
                bool tr = false;
                opcUaClient.WriteNode<bool>(plc + "Privoda.ConsolBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PovorotBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PodachaBackward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.Obnulenie_podacha", tr);

                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZashimNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.PrishimNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.DornNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.Doshim1Nazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibNazad", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZangaRazjim", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.ConsolForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PodachaForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Privoda.PovorotForward", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZashimVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.PrishimVpered", tr);

                opcUaClient.WriteNode<bool>(plc + "Vihoda.Dishim1Vpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.GibDozhim", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.ZangaZajim", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.VKL_Gidro1", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.VKL_Gidro2", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.OsnVniz", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.OsnVverh", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.DornVpered", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.SmazkaDorn", tr);
                opcUaClient.WriteNode<bool>(plc + "Vihoda.Poddergka_naladka", tr);
                // MessageBox.Show($"Скорость {value} %");
            }
            catch (Exception ex)
            {

            }
        }





        public ManualView(Grid labledTextBox1, Grid labledTextBox2, Grid labledTextBox3, Grid labledTextBox4, bool contentLoaded)
        {
            LabledTextBox1 = labledTextBox1;
            LabledTextBox2 = labledTextBox2;
            LabledTextBox3 = labledTextBox3;
            LabledTextBox4 = labledTextBox4;
            _contentLoaded = contentLoaded;
        }
    }
}
