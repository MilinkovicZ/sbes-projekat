using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Expense
    {
        string id;
        string region;
        string city;
        uint year;
        Dictionary<string, double> expensesPerMonth;

        public Expense()
        {
            ExpensesPerMonth = new Dictionary<string, double>();
        }
        public Expense(string id, string region, string city, uint year, Dictionary<string, double> expensesPerMonth)
        {
            Id = id;
            Region = region;
            City = city;
            Year = year;
            ExpensesPerMonth = expensesPerMonth;
        }

        public string Id { get => id; set => id = value; }
        public string Region { get => region; set => region = value; }
        public string City { get => city; set => city = value; }
        public uint Year { get => year; set => year = value; }
        public Dictionary<string, double> ExpensesPerMonth { get => expensesPerMonth; set => expensesPerMonth = value; }
    }
}
