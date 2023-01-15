using CDBServices;
using Common;
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
        public static List<string> Regions { get; set; }
        public static DataBase Database { get; set; }
    }
}
