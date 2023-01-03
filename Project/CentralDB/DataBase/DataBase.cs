using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDB
{
    public class DataBase
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + "DataBase/data.json";
        public static List<Expense> GetExpenses()
        {
            var list = JsonConvert.DeserializeObject<List<Expense>>(File.ReadAllText(path));
            return list == null ? new List<Expense>() : list;
        }

        public static List<Expense> GetExpenses(List<string> regions)
        {
            return GetExpenses().FindAll(t => regions.Contains(t.Region));
        }

        public static Expense GetExpense(string id)
        {
            return GetExpenses().Find(t => t.Id == id);
        } 

        public static void Add(Expense expense)
        {
            var list = GetExpenses();
            expense.Id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            list.Add(expense);
            File.WriteAllText(path, JsonConvert.SerializeObject(list));
        }

        public static void Update(Expense expense)
        {
            var list = GetExpenses();
            int ind = list.FindIndex(t => t.Id == expense.Id);
            if(ind != -1)
            {
                list[ind] = expense;
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
            }
        }

        public static void Delete(string id)
        {
            var list = GetExpenses();
            int ind = list.FindIndex(t => t.Id == id);
            if(ind != -1)
            {
                list.RemoveAt(ind);
                File.WriteAllText(path, JsonConvert.SerializeObject(list));
            }
        }

        public static void UpdateAll(List<Expense> expenses)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(expenses));
        }
    }
}
