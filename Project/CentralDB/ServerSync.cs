using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralDB
{
    public class ServerSync
    {
        static IPEndPoint ipEndPoint;
        static Socket socket;
        static List<Socket> sockets = new List<Socket>();
        public ServerSync()
        {
            ipEndPoint = new IPEndPoint(IPAddress.Loopback, 4500);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(100);
        }

        public static void Accept()
        {
            while(true) sockets.Add(socket.Accept());
        }

        public static void Send(string message)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            try
            {
                Audit.SyncSuccess(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            sockets.ForEach(s => s.Send(ASCIIEncoding.UTF8.GetBytes(message), SocketFlags.None));
        }

        public static void Close()
        {
            sockets.ForEach(s => s.Shutdown(SocketShutdown.Both));
        }
    }
}
