using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CDBServices
{
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        List<Expense> Read(List<string> regions);

        [OperationContract]
        void Add(Expense expense);

        [OperationContract]
        void Delete(string id);

        [OperationContract]
        void Update(Expense expense);
    }
}
