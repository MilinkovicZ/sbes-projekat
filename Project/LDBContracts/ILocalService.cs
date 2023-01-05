using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LDBContracts
{
    [ServiceContract]
    public interface ILocalService
    {
        //TODO - Not sure
        [OperationContract]
        List<Expense> ReadData();
        [OperationContract]
        double GetAverageValue(byte[] region); //ako lokalDb moze da ima vise regiona, ako ne moze onda ne treba parametar.
        [OperationContract]
        void UpdateCurrentMonthUsage(byte[] newValue, byte[] id);
        [OperationContract]
        void AddNew(byte[] expense);
        [OperationContract]
        void DeleteExpense(byte[] id);
    }
}
