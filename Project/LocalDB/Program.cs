using LDBContracts;
using Manager;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string servCertName = "CentralDBCA";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, servCertName);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/SecurityService"), 
                                      new X509CertificateEndpointIdentity(srvCert));

            List<string> myRegions = new List<string>();
            // Unos regiona


            WCFClient proxy = new WCFClient(binding, address);
            WCFService service = new WCFService(proxy);

            //Otvaranje Endpoint-a za clienta - Maybe fix
            NetTcpBinding bindingClient = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:9998/WCFService";
            ServiceHost hostClient = new ServiceHost(typeof(WCFService));
            hostClient.AddServiceEndpoint(typeof(ILocalService), bindingClient, addressClient);

            new ClientSync();
            DataBase db = new DataBase("data.json");
            using (Task t = Task.Run(() =>
            {
                while (true)
                {
                    string s = ClientSync.Receive();
                    if (myRegions.Contains(s))
                        db.UpdateAll(proxy.Read(myRegions));
                }
            }))
                Console.ReadLine();
        }
    }
}
