using CDBServices;
using Common;
using LDBContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class WCFService : ILocalService
    {
        ISecurityService proxy;
        DataBase db;
        byte[] key;
        public WCFService()
        {
            proxy = ServiceProperties.Proxy;
            key = ServiceProperties.Key;
            db = ServiceProperties.Database;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void DeleteExpense(byte[] id)
        {
            string idToDelete = AES.Decrypt(id, key);
            if (db.GetExpenses().Find(t => t.Id == idToDelete) == null)
                throw new FaultException("ID doesn't exist in current local database");
            Console.WriteLine("Deleting expense with ID:" + idToDelete);
            db.Delete(idToDelete);
            proxy.Delete(idToDelete);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public byte[] GetAverageValueForRegion(byte[] region)
        {
            double retVal = 0;
            string targetRegion = AES.Decrypt(region, key);
            if (!ServiceProperties.Regions.Contains(targetRegion))
                throw new FaultException("Region doesn't exist in current local database");
            List<Expense> expensesInRegion = db.GetExpenses().FindAll(e => e.Region == targetRegion);
            int counter = expensesInRegion.Count;

            if (counter == 0)
                throw new FaultException("No data in region");

            foreach (var item in expensesInRegion)
            {
                foreach (var value in item.ExpensesPerMonth.Values)
                {
                    retVal += value;
                }
            }

            return AES.Encrypt((retVal / counter).ToString(), key);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public byte[] GetAverageValueForCity(byte[] city)
        {
            double retVal = 0;
            string targetCity = AES.Decrypt(city, key);
            List<Expense> expensesInRegion = db.GetExpenses().FindAll(e => e.City == targetCity);
            int counter = expensesInRegion.Count;
            if (counter == 0)
                throw new FaultException("City doesn't exist in current local database");
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
            if (list.Count == 0)
                throw new FaultException("No expenses in local database");
            var ret = new List<byte[]>();
            foreach (var item in list)
            {
                ret.Add(AES.Encrypt(item.ToString(), key));
            }
            return ret;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Modify")]
        public void UpdateCurrentMonthUsage(byte[] region, byte[] city, byte[] value)
        {
            string _region = AES.Decrypt(region, key);
            if (!ServiceProperties.Regions.Contains(_region))
                throw new FaultException("Region doesn't exist in current local database");
            string _city = AES.Decrypt(city, key);
            double _value = double.Parse(AES.Decrypt(value, key));
            int _year = DateTime.Now.Year;
            Expense e = db.GetExpenses().Find(t => t.Region == _region && t.Year == _year && t.City == _city);
            if (e == null)
            {
                e = new Expense(_region, _city, _year);
                e.ExpensesPerMonth[DateTime.Now.Month] = _value;
                db.Add(e);
                proxy.Add(e);
                return;
            }

            e.ExpensesPerMonth[DateTime.Now.Month] = _value;
            db.Update(e);
            proxy.Update(e);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void AddNew(byte[] region, byte[] year, byte[] city, Dictionary<byte[], byte[]> expensesPerMonth)
        {
            string _region = AES.Decrypt(region, key);
            if (!ServiceProperties.Regions.Contains(_region))
                throw new FaultException("Region doesn't exist in current local database");
            string _city = AES.Decrypt(city, key);
            int _year = int.Parse(AES.Decrypt(year, key));
            Expense exp = new Expense(_region, _city, _year);
            foreach (var item in expensesPerMonth)
            {
                exp.ExpensesPerMonth.Add(int.Parse(AES.Decrypt(item.Key, key)), double.Parse(AES.Decrypt(item.Value, key)));
            }
            db.Add(exp);
            proxy.Add(exp);
        }

        public List<byte[]> ReadDataRegion(byte[] region)
        {
            var _region = AES.Decrypt(region, key);
            if (!ServiceProperties.Regions.Contains(_region))
                throw new FaultException("Region doesn't exist in current local database");
            var list = db.GetExpenses().FindAll(t => t.Region == _region);
            if (list.Count == 0)
                throw new FaultException("No data for given region");
            var ret = new List<byte[]>();
            foreach (var item in list)
            {
                ret.Add(AES.Encrypt(item.ToString(), key));
            }
            return ret;
        }

        public byte[] GetKey(X509Certificate2 certificate)
        {
            if (certificate == null)
                throw new FaultException("No certificate was given");
            var rsa = certificate.GetRSAPublicKey();
            return rsa.Encrypt(key, System.Security.Cryptography.RSAEncryptionPadding.OaepSHA256);
        }
    }
}
