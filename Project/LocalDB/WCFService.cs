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
        //TO DO - Change Key When we figure out how to.

        ISecurityService proxy;
        DataBase db = new DataBase("data.json");
        string key;
        public WCFService(ISecurityService proxy, string key)
        {
            this.proxy = proxy;
            this.key = key;
        }

        public void AddNew(byte[] expense)
        {
            Expense decryptedExpense = AES.Decrypt<Expense>(expense, key);
            Console.WriteLine("Adding new client expense");
            proxy.Add(decryptedExpense);
        }

        public void DeleteExpense(byte[] id)
        {
            string idToDelete = AES.Decrypt<string>(id, key);
            Console.WriteLine("Deleting expense with ID:" + idToDelete);
            proxy.Delete(idToDelete);
        }

        public byte[] GetAverageValueForRegion(byte[] region)
        {
            double retVal = 0;            
            string targetRegion = AES.Decrypt<string>(region, key);
            List<Expense> expensesInRegion = db.GetExpenses();
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {                
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;                    
                }
            }

            return AES.Encrypt(retVal / counter, key);
        }

        public byte[] GetAverageValueForCity(byte[] city)
        {
            double retVal = 0;
            string targetCity = AES.Decrypt<string>(city, key);
            List<Expense> expensesInRegion = db.GetExpenses().FindAll(e => e.City == targetCity);
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;
                }
            }

            return AES.Encrypt(retVal / counter, key);
        }

        public byte[] ReadData()
        {
            return AES.Encrypt(db.GetExpenses(), key);
        }

        public void UpdateCurrentMonthUsage(byte[] newValue, byte[] id)
        {
            double newValueDecrypted = AES.Decrypt<double>(newValue, key);
            string idDecrypted = AES.Decrypt<string>(newValue, key);

            Expense expense = db.GetExpense(idDecrypted);
            expense.ExpensesPerMonth[DateTime.Now.Month] = newValueDecrypted;
            proxy.Update(expense);
        }
    }
}
