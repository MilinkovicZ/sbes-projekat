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
        public WCFService()
        {
            this.proxy = ServiceProperties.Proxy;
            this.key = ServiceProperties.Key;
        }

        public void DeleteExpense(byte[] id)
        {
            string idToDelete = AES.Decrypt(id, key);
            Console.WriteLine("Deleting expense with ID:" + idToDelete);
            proxy.Delete(idToDelete);
        }

        public byte[] GetAverageValueForRegion(byte[] region)
        {
            double retVal = 0;            
            string targetRegion = AES.Decrypt(region, key);
            List<Expense> expensesInRegion = db.GetExpenses();
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {                
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;                    
                }
            }

            return AES.Encrypt((retVal / counter).ToString(), key);
        }

        public byte[] GetAverageValueForCity(byte[] city)
        {
            double retVal = 0;
            string targetCity = AES.Decrypt(city, key);
            List<Expense> expensesInRegion = db.GetExpenses().FindAll(e => e.City == targetCity);
            int counter = expensesInRegion.Count;
            foreach (var item in expensesInRegion)
            {
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;
                }
            }

            return AES.Encrypt((retVal / counter).ToString(), key);
        }

        public List<byte[]> ReadData()
        {
            var list = db.GetExpenses();
            var ret = new List<byte[]>();
            foreach (var item in list)
            {
                ret.Add(AES.Encrypt(item.ToString(), key));
            }
            return ret;
        }


        public void UpdateCurrentMonthUsage(byte[] region, byte[] city, byte[] value)
        {
            string _region = AES.Decrypt(region, key);
            string _city = AES.Decrypt(city, key);
            double _value = double.Parse(AES.Decrypt(value, key));
            int _year = DateTime.Now.Year;
            Expense e = db.GetExpenses().Find(t => t.Region == _region && t.Year == _year);
            if(e == null)
                e = new Expense(_region, _city, _year);
            e.ExpensesPerMonth[DateTime.Now.Month] = _value;
        }

        public void AddNew(byte[] region, byte[] year, byte[] city, Dictionary<byte[], byte[]> expensesPerMonth)
        {
            string _region = AES.Decrypt(region, key);
            string _city = AES.Decrypt(city, key);
            int _year = int.Parse(AES.Decrypt(year, key));
            Dictionary<int, double> exp = new Dictionary<int, double>();
            foreach (var item in expensesPerMonth)
            {
                exp.Add(int.Parse(AES.Decrypt(item.Key, key)), double.Parse(AES.Decrypt(item.Value, key)));
            }
        }

        public List<byte[]> ReadDataRegion(byte[] region)
        {
            var list = db.GetExpenses().FindAll(t => t.Region == AES.Decrypt(region, key));
            var ret = new List<byte[]>();
            foreach (var item in list)
            {
                ret.Add(AES.Encrypt(item.ToString(), key));
            }
            return ret;
        }
    }
}
