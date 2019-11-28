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

namespace WPFChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void butAuth_Click(object sender, RoutedEventArgs e)
        {
            RequestClass request = new RequestClass("https://localhost:44326/api/user/IsUser", Convert.ToInt32(tbId.Text), tbPass.Text);
            var req = request.MakeGetRequest(request.Url, request.Id, request.Password);
            var str = request.GetMessageUser(req);
            listMessages.Items.Clear();
            listMessages.Items.Add(str);//TODO преобразовать потом уже в виде читабельных сообщений, а не JSON
        }

        private void bExit_Click(object sender, RoutedEventArgs e)
        {
            listMessages.Items.Clear();
            tbId.Text = "";
            tbPass.Text = "";
        }

        private void CheckNewMessage_Click(object sender, RoutedEventArgs e)
        {
            RequestClass request = new RequestClass("https://localhost:44326/api/user/IsUser", Convert.ToInt32(tbId.Text), tbPass.Text);
            var req = request.MakeGetRequest(request.Url, request.Id, request.Password);
            var str = request.GetMessageUser(req);
            listMessages.Items.Clear();
            listMessages.Items.Add(str);
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
