﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Expense
    {
        string id = null;
        string region;
        string city;
        int year;
        Dictionary<int, double> expensesPerMonth;

        public Expense()
        {
            ExpensesPerMonth = new Dictionary<int, double>();
        }
        public Expense(string region, string city, int year)
        {
            Region = region;
            City = city;
            Year = year;
            ExpensesPerMonth = new Dictionary<int, double>();
        }

        public string Id { get => id; set => id = value; }
        public string Region { get => region; set => region = value; }
        public string City { get => city; set => city = value; }
        public int Year { get => year; set => year = value; }
        public Dictionary<int, double> ExpensesPerMonth { get => expensesPerMonth; set => expensesPerMonth = value; }

        public override string ToString()
        {
            var ret = $"ID:{Id}\nRegion:{Region}\nCity:{City}\nYear:{Year}\nExpenses per month:\n";
            foreach (var item in ExpensesPerMonth)
            {
                ret += $"\t{item.Key}: {item.Value}\n";
            }
            return ret;
        }
    }
}
