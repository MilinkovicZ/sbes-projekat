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

        public WCFService(ISecurityService proxy)
        {
            this.proxy = proxy;
        }

        public void AddNew(byte[] expense)
        {
            Expense decryptedExpense = AES.Decrypt<Expense>(expense, "Yo mama");
            Console.WriteLine("Adding new client expense");
            proxy.Add(decryptedExpense);
        }

        public void DeleteExpense(byte[] id)
        {
            string idToDelete = AES.Decrypt<string>(id, "Yo mama");
            Console.WriteLine("Deleting expense with ID:" + idToDelete);
            proxy.Delete(idToDelete);
        }

        public byte[] GetAverageValueForRegion(byte[] region)
        {
            double retVal = 0;            
            string targetRegion = AES.Decrypt<string>(region, "Yo mama");
            List<Expense> expensesInRegion = db.GetExpenses();
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {                
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;                    
                }
            }

            return AES.Encrypt(retVal / counter, "Yo mama");
        }

        public byte[] GetAverageValueForCity(byte[] city)
        {
            double retVal = 0;
            string targetCity = AES.Decrypt<string>(city, "Yo mama");
            List<Expense> expensesInRegion = db.GetExpenses().FindAll(e => e.City == targetCity);
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;
                }
            }

            return AES.Encrypt(retVal / counter, "Yo mama");
        }

        public byte[] ReadData()
        {
            return AES.Encrypt(db.GetExpenses(), "Yo mama");
        }

        public void UpdateCurrentMonthUsage(byte[] newValue, byte[] id)
        {
            double newValueDecrypted = AES.Decrypt<double>(newValue, "Yo mama");
            string idDecrypted = AES.Decrypt<string>(newValue, "Yo mama");

            Expense expense = db.GetExpense(idDecrypted);
            expense.ExpensesPerMonth[DateTime.Now.Month] = newValueDecrypted;
            proxy.Update(expense);
        }
    }
}
