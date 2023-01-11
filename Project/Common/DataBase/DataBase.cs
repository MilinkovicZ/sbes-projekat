using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DataBase
    {
        string path;
        public DataBase(string path)
        {
           this.path = AppDomain.CurrentDomain.BaseDirectory + path;
            if (!File.Exists(this.path))
                File.WriteAllText(path, "[]");
        }
        
        public List<Expense> GetExpenses()
        {
            List<Expense> list = null;
            try
            {
                list = JsonConvert.DeserializeObject<List<Expense>>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return list == null ? new List<Expense>() : list;
        }

        public List<Expense> GetExpenses(string region)
        {
            return GetExpenses().FindAll(t => t.Region == region);
        }
        public List<Expense> GetExpenses(List<string> regions)
        {
            return GetExpenses().FindAll(t => regions.Contains(t.Region));
        }

        public Expense GetExpense(string id)
        {
            return GetExpenses().Find(t => t.Id == id);
        } 

        public void Add(Expense expense)
        {
            var list = GetExpenses();
            expense.Id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            list.Add(expense);
            File.WriteAllText(path, JsonConvert.SerializeObject(list));
        }

        public void Update(Expense expense)
        {
            var list = GetExpenses();
            int ind = list.FindIndex(t => t.Id == expense.Id);
            if(ind != -1)
            {
                list[ind] = expense;
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
            }
        }

        public void Delete(string id)
        {
            var list = GetExpenses();
            int ind = list.FindIndex(t => t.Id == id);
            if(ind != -1)
            {
                list.RemoveAt(ind);
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
            }
        }

        public void UpdateAll(List<Expense> expenses)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(expenses));
        }
    }
}
