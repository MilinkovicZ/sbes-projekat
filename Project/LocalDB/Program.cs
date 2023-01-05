using Manager;
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

            //Fali Povezivanje
            WCFService service = new WCFService(proxy);

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
