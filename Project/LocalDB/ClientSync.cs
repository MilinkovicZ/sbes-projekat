using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class ClientSync
    {
        static IPEndPoint ip;
        static Socket socket;

        public ClientSync()
        {
            ip = new IPEndPoint(IPAddress.Loopback, 4500);
            socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip);
        }

        public static string Receive()
        {
            byte[] buffer = new byte[1024];
            int size = socket.Receive(buffer, SocketFlags.None);
            Array.Resize(ref buffer, size);
            return ASCIIEncoding.UTF8.GetString(buffer);
        }
    }
}
