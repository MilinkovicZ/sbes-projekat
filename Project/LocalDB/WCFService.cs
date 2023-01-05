using CDBServices;
using Common;
using LDBContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class WCFService : ILocalService
    {
        //TO DO - Decrypt req and encrypt ans

        string keyFile = "SecretKey.txt";
        ISecurityService proxy;

        public WCFService(ISecurityService proxy)
        {
            this.proxy = proxy;
        }

        public void AddNew(byte[] expense)
        {
            Expense decryptedExpense = (Expense)AES.Decrypt(expense, SecretKey.LoadKey(keyFile));
            Console.WriteLine("Adding new client expense");
            proxy.Add(decryptedExpense);
        }

        public void DeleteExpense(byte[] id)
        {
            string idToDelete = AES.Decrypt(id, SecretKey.LoadKey(keyFile)).ToString();
            Console.WriteLine("Deleting expense with ID:" + idToDelete);
            proxy.Delete(idToDelete);
        }

        public double GetAverageValue(byte[] region)
        {
            double retVal = 0;            
            string targetRegion = AES.Decrypt(region, SecretKey.LoadKey(keyFile)).ToString();
            List<Expense> expensesInRegion = proxy.Read(new List<string> { targetRegion });
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {                
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;                    
                }
            }

            return retVal / counter;
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
