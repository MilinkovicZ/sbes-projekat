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
        static byte[] SerializeObjToByte(object obj)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        static object DeserializeObjToByte(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
               return new BinaryFormatter().Deserialize(ms);
            }
        }

        public static byte[] Encrypt(object secret, string secretKey)
        {
            byte[] body = SerializeObjToByte(secret);  
            byte[] encryptedBody = null;

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            aesCryptoProvider.GenerateIV();
            ICryptoTransform aesEncryptTransform = aesCryptoProvider.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(body, 0, body.Length);
                    encryptedBody = aesCryptoProvider.IV.Concat(memoryStream.ToArray()).ToArray();
                }
            }

            return encryptedBody;
        }

        public static T Decrypt<T>(byte[] secret, string secretKey)
        {
            byte[] body = secret;
            byte[] decryptedBody = null;

            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            aesCryptoProvider.IV = body.Take(aesCryptoProvider.BlockSize / 8).ToArray();
            ICryptoTransform aesDecryptTransform = aesCryptoProvider.CreateDecryptor();

            using (MemoryStream memoryStream = new MemoryStream(body.Skip(aesCryptoProvider.BlockSize / 8).ToArray()))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptTransform, CryptoStreamMode.Read))
                {
                    decryptedBody = new byte[body.Length - aesCryptoProvider.BlockSize / 8];
                }
            }

            return (T)DeserializeObjToByte(decryptedBody);
        }
    }
}
