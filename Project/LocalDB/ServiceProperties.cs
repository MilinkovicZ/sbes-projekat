using CDBServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDB
{
    class ServiceProperties
    {
        public static ISecurityService Proxy { get; set; }
        public static byte[] Key { get; set; }
    }
}
