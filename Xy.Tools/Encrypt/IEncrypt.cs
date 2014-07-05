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
}
