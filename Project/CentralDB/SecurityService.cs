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
        public void Add(Expense expense)
        {
            DataBase.Add(expense);

            //slanje dalje
        }

        public void Delete(string id)
        {
            DataBase.Delete(id);

            //slanje dalje
        }

        public List<Expense> Read(List<string> regions)
        {
            return DataBase.GetExpenses(regions);
        }

        public void Update(Expense expense)
        {
            DataBase.Update(expense);

            //slanje dalje
        }
    }
}
