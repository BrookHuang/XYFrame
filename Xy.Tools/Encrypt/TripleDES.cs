using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Xy.Tools.Encrypt {
    public sealed class TripleDES : IEncrypt {
        private byte[] key;
        private byte[] iv;
        public TripleDES(string Key, string IV) {
            Key += "1988100419881004";
            Key = Key.Substring(0, 16);
            key = Encoding.UTF8.GetBytes(Key);
            IV += "1988100419881004";
            IV = IV.Substring(0, 8);
            iv = Encoding.UTF8.GetBytes(IV);
        }
        public string Encrypt(string value) {
            Byte[] tmp = null;
            Byte[] tmpData = Encoding.UTF8.GetBytes(value);
            System.Security.Cryptography.TripleDES tripleDes = System.Security.Cryptography.TripleDES.Create();
            ICryptoTransform encryptor = tripleDes.CreateEncryptor(key, iv);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(value);
                    writer.Flush();//这句很重要，在对流操作结束后必须用这句话强制将缓冲区中的数据全部写入到目标对象中   
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);

        }
        public bool Decrypt(ref string value) {
            Byte[] tmp = Convert.FromBase64String(value);
            string result = string.Empty;
            System.Security.Cryptography.TripleDES tripleDES = System.Security.Cryptography.TripleDES.Create();
            ICryptoTransform decryptor = tripleDES.CreateDecryptor(key, iv);
            using (MemoryStream ms = new MemoryStream(tmp)) {
                try {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                        StreamReader reader = new StreamReader(cs);
                        result = reader.ReadLine();
                    }
                } catch (System.Security.Cryptography.CryptographicException ex) {
                    value = ex.Message;
                    return false;
                }
            }
            tripleDES.Clear();
            value = result;
            return true;
        }
    }
}
