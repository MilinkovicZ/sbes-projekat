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
        //TO DO - Decrypt ans and encrypt req
        public void AddNew(byte[] expense)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpense(byte[] id)
        {
            throw new NotImplementedException();
        }

        public double GetAverageValue(byte[] region)
        {
            throw new NotImplementedException();
        }

        public List<Expense> ReadData()
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrentMonthUsage(byte[] newValue, byte[] id)
        {
            throw new NotImplementedException();
        }
    }
}
