using LDBContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class WCFClient : ChannelFactory<ILocalService>, ILocalService, IDisposable
    {
        //TO DO - Decrypt ans and encrypt req
    }
}
