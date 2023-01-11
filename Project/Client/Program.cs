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
        static void PrintList(List<Expense> expenses)
        {
            foreach (var e in expenses)
            {
                Console.WriteLine($"\tID: {e.Id}");
                Console.WriteLine($"\tCity: {e.City}");
                Console.WriteLine($"\tRegion: {e.Region}");
                Console.WriteLine($"\tYear: {e.Year}");
                Console.WriteLine($"\tExpenses per month:");
                foreach (var ex in e.ExpensesPerMonth)
                {
                    Console.WriteLine($"\t\t{ex.Key} : {ex.Value}");
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Unesite port na kom radite: (9000-9200)");
            uint port = 0;
            while (port < 9000 || port > 9200)
            {
                try
                {
                    port = UInt32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Unesite validan broj (9000-9200)");
                    port = 0;
                }
            }

            string key = SecretKey.LoadKey(port);

            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:" + port.ToString() + "/WCFService"));

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            WCFClient proxy = new WCFClient(binding,address);

            Console.WriteLine("WELCOME TO EXPENSE PROGRAM!\n\n");

            while (true)
            {
                Console.WriteLine("1 - Read expense data");  //Read
                Console.WriteLine("2 - Get average value for region"); //Read + Calculate
                Console.WriteLine("3 - Get average value for city");
                Console.WriteLine("4 - Update expense value for month"); //Read + Modify
                Console.WriteLine("5 - Add new expense"); //Read + Admin
                Console.WriteLine("6 - Delete existing expense");
                Console.WriteLine("7 - Quit");
                Console.WriteLine("Choose command:");                

                int commandNumber = int.Parse(Console.ReadLine());

                switch (commandNumber)
                {
                    case 1:
                        //NEEDS FIXING PROBABLY
                        try
                        {
                            byte[] encodedData = proxy.ReadData();
                            if (encodedData == null)
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            List<Expense> expenses = AES.Decrypt<List<Expense>>(encodedData, key);
                            PrintList(expenses);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            Console.WriteLine("Expense region:");
                            string regionAvg = Console.ReadLine();
                            if(String.IsNullOrEmpty(regionAvg))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Encrypting regionAvg for safety reasons...");
                            byte[] cryptedRegionAvg = AES.Encrypt(regionAvg, key);
                            byte[] encodedDataRegionAvg = proxy.GetAverageValueForRegion(cryptedRegionAvg);
                            if (encodedDataRegionAvg == null)
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Decrypting recieved data for region average...");
                            double dataRegionAvg = AES.Decrypt<double>(encodedDataRegionAvg, key);
                            Console.WriteLine("Average expense for region: " + regionAvg + " is " + dataRegionAvg + ".");
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            Console.WriteLine("Expense city:");
                            string cityAvg = Console.ReadLine();
                            if (String.IsNullOrEmpty(cityAvg))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Encrypting cityAvg for safety reasons...");
                            byte[] cryptedCityAvg = AES.Encrypt(cityAvg, key);
                            byte[] encodedDataCityAvg = proxy.GetAverageValueForCity(cryptedCityAvg);
                            if (encodedDataCityAvg == null)
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Decrypting recieved data for city average...");
                            double dataCityAvg = AES.Decrypt<double>(encodedDataCityAvg, key);
                            Console.WriteLine("Average expense for city: " + cityAvg + " is " + dataCityAvg + ".");
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 4:
                        try
                        {
                            Console.WriteLine("Expense ID:");
                            string idUpdate = Console.ReadLine();
                            if (String.IsNullOrEmpty(idUpdate))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("New month expense usage:");
                            double usage = double.Parse(Console.ReadLine());
                            if (usage < 0)
                            {
                                Console.WriteLine("Usage need to be positive number.");
                                break;
                            }
                            Console.WriteLine("Encrypting month expense usage for safety reasons...");
                            byte[] idUpdateEncrypted = AES.Encrypt(idUpdate, key);
                            byte[] usageEncrypted = AES.Encrypt(usage, key);
                            proxy.UpdateCurrentMonthUsage(usageEncrypted, idUpdateEncrypted);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.WriteLine("Expense ID:");
                            string id = Console.ReadLine();
                            if (String.IsNullOrEmpty(id))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Expense region:");
                            string region = Console.ReadLine();
                            if (String.IsNullOrEmpty(region))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Expense city:");
                            string city = Console.ReadLine();
                            if (String.IsNullOrEmpty(city))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Expense year:");
                            uint year = uint.Parse(Console.ReadLine());
                            Dictionary<int, double> temp = new Dictionary<int, double>();
                            while (true)
                            {
                                Console.WriteLine("Enter 'x' to stop entering values");
                                Console.WriteLine("Enter month in number [1-12]: ");
                                string s = Console.ReadLine();
                                if (s == "x")
                                    break;
                                int month = int.Parse(s);
                                if (month < 1 || month > 12)
                                {
                                    Console.WriteLine("Enter valid month number");
                                    continue;
                                }
                                Console.WriteLine("Enter usage value for " + month + ". month: ");
                                double usageMonth = double.Parse(Console.ReadLine());
                                if (usageMonth < 0)
                                {
                                    Console.WriteLine("Enter a valid usage");
                                    continue;
                                }
                                temp.Add(month, usageMonth);
                            }
                            Expense expense = new Expense(id, region, city, year, temp);
                            if (expense == null)
                            {
                                Console.WriteLine("There is no expense with given parameters.");
                                continue;
                            }
                            Console.WriteLine("Encrypting expense for safety reasons...");
                            byte[] cryptedExpense = AES.Encrypt(expense, key);
                            proxy.AddNew(cryptedExpense);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 6:
                        try
                        {
                            Console.WriteLine("Expense ID:");
                            string idDelete = Console.ReadLine();
                            if (String.IsNullOrEmpty(idDelete))
                            {
                                Console.WriteLine("There is no data.");
                                break;
                            }
                            Console.WriteLine("Encrypting delete ID for safety reasons...");
                            byte[] idDeleteEncrypted = AES.Encrypt(idDelete, key);
                            proxy.DeleteExpense(idDeleteEncrypted);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }
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
