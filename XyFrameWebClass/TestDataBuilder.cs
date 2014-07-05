using System;
using System.Collections.Generic;
using System.Text;

namespace XyFrameWebClass {
    public class TestDataBuilder: Xy.Data.IDataBuilder  {
        public void Init(System.Collections.Specialized.NameValueCollection Tags) {
            throw new NotImplementedException();
        }

        public System.Xml.XPath.XPathDocument HandleData(Xy.Web.Page.PageAbstract CurrentPageClass) {
            throw new NotImplementedException();
        }

        public string InsertParameter(string xslt) {
            throw new NotImplementedException();
        }

        public string GetRoot() {
            throw new NotImplementedException();
        }
    }
}
