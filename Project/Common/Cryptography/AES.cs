using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AES
    {
        public static byte[] Encrypt(string secret, byte[] secretKey)
        {
            byte[] body = ASCIIEncoding.UTF8.GetBytes(secret);  
            byte[] encryptedBody = null;

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = secretKey,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };

            aesCryptoProvider.GenerateIV();
            ICryptoTransform aesEncryptTransform = aesCryptoProvider.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptTransform, CryptoStreamMode.Write))
                {
                   
                    Array.Resize(ref body, body.Length + 16 - body.Length % 16);
                    
                    cryptoStream.Write(body, 0, body.Length);
                    encryptedBody = aesCryptoProvider.IV.Concat(memoryStream.ToArray()).ToArray();
                }
            }

            return encryptedBody;
        }

        public static string Decrypt(byte[] secret, byte[] secretKey)
        {
            byte[] body = secret;
            byte[] decryptedBody = null;

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = secretKey,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };

            aesCryptoProvider.IV = body.Take(aesCryptoProvider.BlockSize / 8).ToArray();
            ICryptoTransform aesDecryptTransform = aesCryptoProvider.CreateDecryptor();

            using (MemoryStream memoryStream = new MemoryStream(body.Skip(aesCryptoProvider.BlockSize / 8).ToArray()))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptTransform, CryptoStreamMode.Read))
                {
                    decryptedBody = new byte[body.Length - aesCryptoProvider.BlockSize / 8];
                    try
                    {
                        cryptoStream.Read(decryptedBody, 0, decryptedBody.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            string s = ASCIIEncoding.UTF8.GetString(decryptedBody).Replace("\0", "");

            return s;
        }
    }
}
