using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Xy.Tools.Encrypt {
    public sealed class Rijndael : IEncrypt {
        private byte[] key;
        private byte[] iv;
        public Rijndael(string Key, string IV) {
            Key += "1988100419881004";
            Key = Key.Substring(0, 8);
            key = Encoding.UTF8.GetBytes(Key);
            IV += "1988100419881004";
            IV = IV.Substring(0, 16);
            iv = Encoding.UTF8.GetBytes(IV);
        }
        public string Encrypt(string value) {
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();
            byte[] tmp = null;
            ICryptoTransform encryptor = rijndael.CreateEncryptor(key, iv);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(value);
                    writer.Flush();
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);

        }
        public bool Decrypt(ref string value) {
            string result = string.Empty;
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();
            ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv);
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(value))) {
                try {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                        StreamReader reader = new StreamReader(cs);
                        result = reader.ReadLine();
                        reader.Close();
                    }
                } catch (System.Security.Cryptography.CryptographicException ex) {
                    value = ex.Message;
                    return false;
                }
            }
            value = result;
            return true;
        }
    }
}
