using LDBContracts;
using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class WCFClient : ChannelFactory<ILocalService>, ILocalService, IDisposable
    {
        ILocalService factory;
        //TO DO - Decrypt ans and encrypt req
        public WCFClient()
        {
            factory = this.CreateChannel();
        }

        public void AddNew(byte[] expense)
        {
            try
            {
                Console.WriteLine("Sending request for adding new expense!");
                factory.AddNew(expense);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteExpense(byte[] id)
        {
            try
            {
                Console.WriteLine("Sending request for deleting expense!");
                factory.DeleteExpense(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public byte[] GetAverageValueForCity(byte[] city)
        {
            byte[] retVal = null;
            try
            {
                Console.WriteLine("Sending request for average city expense!");
                retVal = factory.GetAverageValueForCity(city);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return retVal;
        }

        public byte[] GetAverageValueForRegion(byte[] region)
        {
            byte[] retVal = null;
            try
            {
                Console.WriteLine("Sending request for average region expense!");
                retVal = factory.GetAverageValueForRegion(region);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return retVal;
        }

        public byte[] ReadData()
        {
            byte[] retVal = null;
            try
            {
                Console.WriteLine("Sending request for all expenses!");
                retVal = factory.ReadData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return retVal;
        }

        public void UpdateCurrentMonthUsage(byte[] newValue, byte[] id)
        {
            try
            {
                Console.WriteLine("Sending request for deleting expense!");
                factory.UpdateCurrentMonthUsage(newValue, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
