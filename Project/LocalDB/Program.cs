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
using System.ServiceModel.Description;
using System.IdentityModel.Policy;

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

            uint port = 0;
            while (port < 9000 || port > 9200)
            {
                try
                {
                    Console.WriteLine("Unesite port na kom radite: (9000-9200)");
                    port = UInt32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Unesite validan broj (9000-9200)");
                    port = 0;
                }
            }

            ServiceProperties.Key = SecretKey.GenerateKey();

            Console.WriteLine(ServiceProperties.Key);
            
            List<string> myRegions = new List<string>();
            // Unos regiona
            string enter = "";
            while (true)
            {
                Console.WriteLine("Enter region: ('x' if you want to finish)");
                enter = Console.ReadLine();
                if (enter == "" || enter == "x")
                    break;
                myRegions.Add(enter);
            }

            if (myRegions.Count == 0)
                return;
            ServiceProperties.Regions = myRegions;
            WCFClient proxy = new WCFClient(binding, address);
            ServiceProperties.Database = new DataBase($"data_{port}.json");
            ServiceProperties.Database.UpdateAll(proxy.Read(myRegions));
            ServiceProperties.Proxy = proxy;

            //Otvaranje Endpoint-a za clienta - Maybe fix
            NetTcpBinding bindingClient = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:" + port.ToString() + "/WCFService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost hostClient = new ServiceHost(typeof(WCFService));
            hostClient.AddServiceEndpoint(typeof(ILocalService), bindingClient, addressClient);

            hostClient.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            hostClient.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            hostClient.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            hostClient.Open();
            new ClientSync();
            
            using (Task t = Task.Run(() =>
            {
                while (true)
                {
                    string s = ClientSync.Receive();
                    if (myRegions.Contains(s))
                        ServiceProperties.Database.UpdateAll(proxy.Read(myRegions));
                }
            }))
            {
                Console.WriteLine("Started Everything...");
                Console.ReadLine();
            }
            Console.ReadLine();
            proxy.Close();
        }
    }
}
