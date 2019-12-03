using System;
using System.Configuration;
using System.Collections.Specialized;
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
            RequestClass reqServer = new RequestClass(ConfigurationSettings.AppSettings["AddressServerUser"], Convert.ToInt32(tbId.Text), tbPass.Password); 
            var reqUser = reqServer.MakeGetRequest(reqServer.Url, reqServer.Id, reqServer.Password); 
            var strMess = reqServer.GetMessageUser(reqUser);
            listMessages.Items.Clear();
            flagTime = false;
            var messages = JsonConvert.DeserializeObject<List<Message>>(strMess);
            for (int i = 0; i < messages.Count; i++)
            {
                string outMess = "От кого: " + messages[i].IdSend + "\n" + "Сообщение: " + messages[i].TextMessage + "\n";
                listMessages.Items.Add(outMess);
            }
            
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
            
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestClass(ConfigurationSettings.AppSettings["AddressServerMessage"]);
            var req = WebRequest.Create(ConfigurationSettings.AppSettings["AddressServerMessage"]);
            Message message = new Message(Convert.ToInt32(tbId.Text),
                Convert.ToInt32(tbRecUser.Text),
                tbNewMes.Text);

            request.MakePostReq(req, message);
            tbNewMes.Text = "";
            tbRecUser.Text = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if ((tbId.Text != "") && (tbPass.Password != ""))
            {
                RequestClass reqServer = new RequestClass(ConfigurationSettings.AppSettings["AddressServerUser"], Convert.ToInt32(tbId.Text), tbPass.Password);
                var reqUser = reqServer.MakeGetRequest(reqServer.Url, reqServer.Id, reqServer.Password);
                var strMess = reqServer.GetMessageUser(reqUser);
                listMessages.Items.Clear(); //Оптимизировать вот эту штуку, возможно использовать флаг сообщении 
                var messages = JsonConvert.DeserializeObject<List<Message>>(strMess);
                for (int i = 0; i < messages.Count; i++)
                {
                    if (messages[i].IsRead == false)
                    {
                        string outMess = "От кого: " + messages[i].IdSend + "\n" + "Сообщение: " + messages[i].TextMessage + "\n";
                        listMessages.Items.Add(outMess);
                    }
                    
                }
            }
        }
    }
}
