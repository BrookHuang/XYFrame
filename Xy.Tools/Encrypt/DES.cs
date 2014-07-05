using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Xy.Tools.Encrypt {
    public sealed class DES : IEncrypt {
        private byte[] key;
        private byte[] iv;
        public DES(string Key, string IV) {
            Key += "1988100419881004";
            Key = Key.Substring(0, 8);
            key = Encoding.UTF8.GetBytes(Key);
            IV += "1988100419881004";
            IV = IV.Substring(0, 8);
            iv = Encoding.UTF8.GetBytes(IV);
        }
        public string Encrypt(string value) {
            System.Security.Cryptography.DES des = System.Security.Cryptography.DES.Create();
            byte[] tmp = Encoding.UTF8.GetBytes(value);
            Byte[] encryptoData;
            ICryptoTransform encryptor = des.CreateEncryptor(key, iv);
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (var cs = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter writer = new StreamWriter(cs)) {
                        writer.Write(value);
                        writer.Flush();
                    }
                }
                encryptoData = memoryStream.ToArray();
            }
            des.Clear();
            return Convert.ToBase64String(encryptoData);
        }
        public bool Decrypt(ref string value) {
            string resultData = string.Empty;
            Byte[] tmpData = Convert.FromBase64String(value);
            System.Security.Cryptography.DES des = System.Security.Cryptography.DES.Create();

            ICryptoTransform decryptor = des.CreateDecryptor(key, iv);
            using (var memoryStream = new MemoryStream(tmpData)) {
                try {
                    using (var cs = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
                        StreamReader reader = new StreamReader(cs);
                        resultData = reader.ReadLine();
                    }
                } catch (System.Security.Cryptography.CryptographicException ex) {
                    value = ex.Message;
                    return false;
                }
            }
            value = resultData;
            return true;
        }
    }
}
