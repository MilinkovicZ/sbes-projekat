using CDBServices;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDB
{
    public class SecurityService : ISecurityService
    {
        DataBase db = new DataBase("data.json");
        public void Add(Expense expense)
        {
            db.Add(expense);
            ServerSync.Send(expense.Region);
        }

        public void Delete(string id)
        {
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
            db.Update(expense);
            ServerSync.Send(expense.Region);
        }
    }
}
