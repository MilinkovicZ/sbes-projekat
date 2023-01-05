using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SecretKey
    {
        public static string GenerateKey(string identity)
        {
            string _pepper = "secret";
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(
                Encoding.Unicode.GetBytes(identity + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }
    }
}