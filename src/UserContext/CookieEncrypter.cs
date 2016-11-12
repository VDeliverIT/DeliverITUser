using System;
using System.Security.Cryptography;
using System.IO;

namespace UserContext
{
    public class CookieEncrypter : ICookieBinaryProcessor
    {
        private byte[] _key = { 112, 124, 238, 23, 21, 33, 22, 35, 175, 93, 84, 102, 22, 49, 199, 205, 228, 19, 44, 51, 80, 200, 100, 147, 92, 9, 27, 3, 162, 217, 7, 11 };

        private byte[] _iv = new Guid("BA064632-0FCF-4F02-AD76-80E3ECA86FB8").ToByteArray();
        public string Decrypt(byte[] objectArray)
        {
            Aes provider = Aes.Create();
            provider.Mode = CipherMode.CBC;
            string result;
            MemoryStream incomingStream = new MemoryStream(objectArray);
            using (CryptoStream stream = new CryptoStream(incomingStream, provider.CreateDecryptor(_key, _iv), CryptoStreamMode.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        string ICookieBinaryProcessor.Read(byte[] objectArray)
        {
            return Decrypt(objectArray);
        }

        public byte[] Encrypt(string value)
        {

            Aes provider = Aes.Create();
            provider.Mode = CipherMode.CBC;

            MemoryStream result = new MemoryStream();
            using (CryptoStream stream = new CryptoStream(result, provider.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(value);
                }
            }

            return result.ToArray();
        }
        byte[] ICookieBinaryProcessor.Write(string value)
        {
            return Encrypt(value);
        }
    }
}
