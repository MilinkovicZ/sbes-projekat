using CDBServices;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class WCFClient : ChannelFactory<ISecurityService>, ISecurityService, IDisposable
    {
        ISecurityService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void Add(Expense expense)
        {
            try
            {
                factory.Add(expense);
                Console.WriteLine("Added new expense");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Delete(string id)
        {
            try
            {
                factory.Delete(id);
                Console.WriteLine("Deleted expense");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Expense> Read(List<string> regions)
        {
            List<Expense> ret = null;
            try
            {
                ret = factory.Read(regions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        public void Update(Expense expense)
        {
            try
            {
                factory.Update(expense);
                Console.WriteLine("Updated expense");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

    }
}
