using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data {
    public class TransException : Exception {
        public TransException(string text)
            : base(text) {
        }
    }
}
