using CDBServices;
using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralDB
{
    public class SecurityService : ISecurityService
    {
        DataBase db = new DataBase("data.json");
        public void Add(Expense expense)
        {
            db.Add(expense);
            try
            {
                Audit.Add(expense.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

           
            ServerSync.Send(expense.Region);
        }

        public void Delete(string id)
        {
            try
            {
                Audit.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var obj = db.GetExpense(id);
            if (obj == null)
                return;
            db.Delete(id);
            ServerSync.Send(obj.Region);
        }

        public List<Expense> Read(List<string> regions)
        {
            return db.GetExpenses(regions);
        }

        public void Update(Expense expense)
        {
            try
            {
                Audit.Update(expense.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            db.Update(expense);
            ServerSync.Send(expense.Region);
        }
    }
}
