using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Encrypt {
    public enum EncryptType {
        DES,
        TripleDES,
        Rijndeal
    }
    public interface IEncrypt {
        string Encrypt(string value);
        bool Decrypt(ref string value);
    }
    public class EncryptHelper {
        static IEncrypt _instance;
        static EncryptHelper() {
            _instance = new Rijndael("THISISXYFRAMEENCRYPTKEY", "VITPYRCNEEMARYXSISIHT");
        }
        public static IEncrypt GetEncryptProvider(EncryptType type) {
            return GetEncryptProvider(type, "THISISXYFRAMEENCRYPTKEY", "VITPYRCNEEMARYXSISIHT");
        }
        public static IEncrypt GetEncryptProvider(EncryptType type, string key, string iv) {
            switch (type) {
                case EncryptType.DES:
                    return new DES(key, iv);
                case EncryptType.TripleDES:
                    return new TripleDES(key, iv);
                case EncryptType.Rijndeal:
                default:
                    return new Rijndael(key, iv);
            }
        }
        public static string Encrypt(string value) {
            return _instance.Encrypt(value);
        }
        public static bool Decrypt(ref string value) {
            return _instance.Decrypt(ref value);
        }
    }
}
