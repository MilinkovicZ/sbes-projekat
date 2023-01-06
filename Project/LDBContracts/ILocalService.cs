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
        [OperationContract]
        byte[] ReadData();
        [OperationContract]
        byte[] GetAverageValueForRegion(byte[] region); //ako lokalDb moze da ima vise regiona, ako ne moze onda ne treba parametar.
        [OperationContract]
        byte[] GetAverageValueForCity(byte[] city);
        [OperationContract]
        void UpdateCurrentMonthUsage(byte[] newValue, byte[] id);
        [OperationContract]
        void AddNew(byte[] expense);
        [OperationContract]
        void DeleteExpense(byte[] id);
    }
}
