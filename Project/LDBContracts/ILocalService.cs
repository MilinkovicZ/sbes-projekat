using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LDBContracts
{
    [ServiceContract]
    public interface ILocalService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<byte[]> ReadData();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<byte[]> ReadDataRegion(byte[] region);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        byte[] GetAverageValueForRegion(byte[] region); 

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        byte[] GetAverageValueForCity(byte[] city);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void UpdateCurrentMonthUsage(byte[] region, byte[] city, byte[] value);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void AddNew(byte[] region, byte[] year, byte[] city, Dictionary<byte[], byte[]> expensesPerMonth);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void DeleteExpense(byte[] id);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        byte[] GetKey(X509Certificate2 certificate);
    }
}
