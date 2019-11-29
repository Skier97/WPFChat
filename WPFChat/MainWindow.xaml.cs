using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace WPFChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool flagTime = false;
        public MainWindow()
        {
            InitializeComponent();
            /*while (flagTime == false)
            {
                Thread.Sleep(5000);
                //Console.WriteLine("*** calling MyMethod *** ");
                CheckNewMessage();
            }*/
        }

        private void butAuth_Click(object sender, RoutedEventArgs e)
        {
            RequestClass request = new RequestClass("https://localhost:44326/api/user/IsUser", Convert.ToInt32(tbId.Text), tbPass.Password);
            var req = request.MakeGetRequest(request.Url, request.Id, request.Password);
            var str = request.GetMessageUser(req);
            listMessages.Items.Clear();
            flagTime = false;
            var messages = JsonConvert.DeserializeObject<List<Message>>(str);
            for (int i = 0; i < messages.Count; i++)
            {
                string outMess = "От кого: " + messages[i].IdSend + "\n" + "Сообщение: " + messages[i].TextMessage + "\n";
                listMessages.Items.Add(outMess);
            }
            
            //listMessages.Items.Add(str);//TODO преобразовать потом уже в виде читабельных сообщений, а не JSON
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            listMessages.Items.Clear();
            tbId.Text = "";
            tbPass.Password = "";
            flagTime = true;
        }

        private void CheckNewMessage()
        {
            if ((tbId.Text != "")&&(tbPass.Password != ""))
            {
                RequestClass request = new RequestClass("https://localhost:44326/api/user/IsUser", Convert.ToInt32(tbId.Text), tbPass.Password);
                var req = request.MakeGetRequest(request.Url, request.Id, request.Password);
                var str = request.GetMessageUser(req);
                listMessages.Items.Clear();
                var messages = JsonConvert.DeserializeObject<List<Message>>(str);
                for (int i = 0; i < messages.Count; i++)
                {
                    string outMess = "От кого: " + messages[i].IdSend + "\n" + "Сообщение: " + messages[i].TextMessage + "\n";
                    listMessages.Items.Add(outMess);
                }
            }
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestClass("https://localhost:44326/api/user/AddMessage");
            var req = WebRequest.Create("https://localhost:44326/api/user/AddMessage");
            Message message = new Message(Convert.ToInt32(tbId.Text),
                Convert.ToInt32(tbRecUser.Text),
                tbNewMes.Text);

            request.MakePostReq(req, message);
            tbNewMes.Text = "";
            tbRecUser.Text = "";
        }
    }
}
