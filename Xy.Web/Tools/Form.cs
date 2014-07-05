using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Web {
    public class Form {
        public static bool ValidateForm(System.Collections.Specialized.NameValueCollection values) {
            for (int i = 0; i < values.Count; i++) {
                if (string.IsNullOrEmpty(values[i])) return false;
            }
            return true;
        }

        public static bool ValidateForm(System.Collections.Specialized.NameValueCollection values, string[] keys) {
            for (int i = 0; i < keys.Length; i++) {
                if (string.IsNullOrEmpty(values[keys[i]])) return false;
            }
            return true;
        }
    }
}
