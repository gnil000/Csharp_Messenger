using System.Net;
using System.Net.Sockets;
using System.Text;

Socket[] Connections = new Socket[5];
int count = 0;

const string ip = "127.0.0.1";
const int port = 8080;

var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

var tcpListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

tcpListen.Bind(tcpEndPoint);
tcpListen.Listen(5);

Socket newConnect;
for (int i = 0; i < 2; i++)
{
    Thread thread = new Thread(ClientsSend);
    newConnect = tcpListen.Accept();

    if (newConnect == null)
        Console.WriteLine("Error");
    else
    {
        Console.WriteLine("Client connected!");
        Connections[i] = newConnect;
        count++;
        thread.Start(i);
    }
    Thread.Sleep(1000);
}

void ClientsSend(object? obj)
{
    var buffer = new byte[1024];
    var size = 0;
    var data = new StringBuilder();
    string msg;

    int index;
    if (obj != null)
        index = (int)obj;
    else
        return;

    while (true)
    {
        size = Connections[index].Receive(buffer);
        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
        msg = data.ToString();
        Console.WriteLine("Message client: " + msg);

        for (int i = 0; i < count; i++)
        {
            if (i == index)
                continue;
            Connections[i].Send(Encoding.UTF8.GetBytes(msg));
        }
        buffer = new byte[1024];
        msg = "";
        data.Clear();
        size = 0;
        Thread.Sleep(10);
    }
}
