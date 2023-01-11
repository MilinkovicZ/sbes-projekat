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
        List<byte[]> ReadData();
        [OperationContract]
        List<byte[]> ReadData(byte[] region);
        [OperationContract]
        byte[] GetAverageValueForRegion(byte[] region); //ako lokalDb moze da ima vise regiona, ako ne moze onda ne treba parametar.
        [OperationContract]
        byte[] GetAverageValueForCity(byte[] city);
        [OperationContract]
        void UpdateCurrentMonthUsage(byte[] region, byte[] city, byte[] value);
        [OperationContract]
        void AddNew(byte[] region, byte[] year, byte[] city, Dictionary<byte[], byte[]> expensesPerMonth);
        [OperationContract]
        void DeleteExpense(byte[] id);
    }
}
