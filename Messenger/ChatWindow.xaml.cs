using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        AppContext db;
        List<User> users;

        Socket Connection;
        const string ip = "127.0.0.1";
        const int port = 8080;
        IPEndPoint tcpEndPoint;

        byte[] buffer;
        //int size;
        StringBuilder answer;

        //string name;

        public ChatWindow()
        {
            InitializeComponent();
            //Debug.WriteLine("\t" + name.ToString() + "\t");
            //UserName.Text = name;

            ListUsers.Items.Add("Общий чат");
            db = new AppContext();
            users = db.Users.ToList();

            foreach(var us in users)
                ListUsers.Items.Add(us);

            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connection.Connect(tcpEndPoint);

            buffer = new byte[1024];
            answer = new StringBuilder();
            //size = 0;

            Thread thread = new Thread(ClientListen);
            thread.Start();
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            ListUsers.Items.Clear();
            ListUsers.Items.Add("Общий чат");
            users.Clear();
            users = db.Users.ToList();
            foreach (var us in users)
                ListUsers.Items.Add(us);
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            string msg = MessageBox.Text;
            MessageBox.Text = "";
            buffer = Encoding.UTF8.GetBytes(msg);
            Connection.Send(buffer);
            buffer = new byte[1024];
            MessagesList.Items.Add(msg);

        }

        void ClientListen() { 
            var buffer = new byte[1024];
            var size = 0;
            var answer = new StringBuilder();
            while (true) { 
                size = Connection.Receive(buffer);
                Debug.WriteLine("\t"+size+"\t");
                try
                {
                    answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                catch {
                    Debug.WriteLine("\ttry get cannot work\t");
                }
                //Debug.WriteLine(answer.ToString());
                try
                {
                    string m = answer.ToString();
                    MessagesList.Items.Add(m);
                }
                catch {
                    Debug.WriteLine("\ttry cannot work\t");
                }
                buffer = new byte[1024];
                answer.Clear();
                size = 0;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                FriendName.Text = item.Content.ToString();
            }
        }

        public void nameSet(string name) {
            //Debug.WriteLine("\t" + name.ToString() + "\t");
            UserName.Text = name;
        }

    }
}
