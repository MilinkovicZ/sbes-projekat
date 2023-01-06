using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "yo mama"; //OVO POPRAVITI XD

            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9998/WCFService"));
            WCFClient proxy = new WCFClient(binding,address);

            Console.WriteLine("WELCOME TO EXPENSE PROGRAM!\n\n");

            while (true)
            {
                Console.WriteLine("1 - Add new expense");
                Console.WriteLine("2 - Read expense data");
                Console.WriteLine("3 - Get average value for region");
                Console.WriteLine("4 - Get average value for city");
                Console.WriteLine("5 - Update expense value for month");
                Console.WriteLine("6 - Delete existing expense");
                Console.WriteLine("7 - Get average value for city");
                Console.WriteLine("Choose command:");                

                int commandNumber = int.Parse(Console.ReadLine());

                switch (commandNumber)
                {
                    case 1:
                        Console.WriteLine("Expense ID:");
                        string id = Console.ReadLine();
                        Console.WriteLine("Expense region:");
                        string region = Console.ReadLine();
                        Console.WriteLine("Expense city:");
                        string city = Console.ReadLine();
                        Expense expense = new Expense(id, region, city, (uint)DateTime.Now.Year, new Dictionary<string, double>());
                        Console.WriteLine("Encrypting expense for safety reasons...");
                        byte[] cryptedExpense = AES.Encrypt(expense, key);
                        proxy.AddNew(cryptedExpense);
                        break;
                    case 2:
                        //NEEDS FIXING PROBABLY
                        byte[] encodedData = proxy.ReadData();
                        List<Expense> expenses = new List<Expense>();
                        Console.WriteLine("Decrypting expenses data...");
                        expenses.Add((Expense)AES.Decrypt(encodedData, key));
                        break;
                    case 3:
                        Console.WriteLine("Expense region:");
                        string regionAvg = Console.ReadLine();
                        Console.WriteLine("Encrypting regionAvg for safety reasons...");
                        byte[] cryptedRegionAvg = AES.Encrypt(regionAvg, key);
                        byte[] encodedDataRegionAvg = proxy.GetAverageValueForRegion(cryptedRegionAvg);
                        Console.WriteLine("Decrypting recieved data for region average...");
                        double dataRegionAvg = (double)AES.Decrypt(encodedDataRegionAvg, key);
                        Console.WriteLine("Average expense for region: " + regionAvg + " is " + dataRegionAvg + ".");
                        break;
                    case 4:
                        Console.WriteLine("Expense city:");
                        string cityAvg = Console.ReadLine();
                        Console.WriteLine("Encrypting cityAvg for safety reasons...");
                        byte[] cryptedCityAvg = AES.Encrypt(cityAvg, key);
                        byte[] encodedDataCityAvg = proxy.GetAverageValueForCity(cryptedCityAvg);
                        Console.WriteLine("Decrypting recieved data for city average...");
                        double dataCityAvg = (double)AES.Decrypt(encodedDataCityAvg, key);
                        Console.WriteLine("Average expense for city: " + cityAvg + " is " + dataCityAvg + ".");
                        break;
                    case 5:
                        Console.WriteLine("Expense ID:");
                        string idUpdate = Console.ReadLine();
                        Console.WriteLine("New month expense usage:");
                        double usage = double.Parse(Console.ReadLine());
                        Console.WriteLine("Encrypting month expense usage for safety reasons...");
                        byte[] idUpdateEncrypted = AES.Encrypt(idUpdate, key);
                        byte[] usageEncrypted = AES.Encrypt(usage, key);
                        proxy.UpdateCurrentMonthUsage(usageEncrypted, idUpdateEncrypted);
                        break;
                    case 6:
                        Console.WriteLine("Expense ID:");
                        string idDelete = Console.ReadLine();
                        Console.WriteLine("Encrypting delete ID for safety reasons...");
                        byte[] idDeleteEncrypted = AES.Encrypt(idDelete, key);
                        proxy.DeleteExpense(idDeleteEncrypted);
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please choose valid option.");
                        break;
                }
            }
        }
    }
}
