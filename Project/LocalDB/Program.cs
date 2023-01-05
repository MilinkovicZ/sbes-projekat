using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> myRegions = new List<string>();
            // Unos regiona


            // Kacenje na host
            WCFClient proxy = new WCFClient(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:9999/SecurityService"));

            new ClientSync();

            using (Task t = Task.Run(() =>
            {
                string s = ClientSync.Receive();
                if (myRegions.Contains(s))
                    proxy.Read(new List<string>() { s });
            }))
                Console.ReadLine();
        }
    }
}
