using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Cache {
    public class XslCompiledTransform {
        private static Dictionary<string, System.Xml.Xsl.XslCompiledTransform> _xslLibrary = new Dictionary<string, System.Xml.Xsl.XslCompiledTransform>();

        public static System.Xml.Xsl.XslCompiledTransform Get(string template, bool enableCode = false, bool enableCache = true) {
            System.Xml.Xsl.XslCompiledTransform xslTransform;
            if (enableCache && _xslLibrary.ContainsKey(template)) {
                xslTransform = _xslLibrary[template];
            } else {
                xslTransform = new System.Xml.Xsl.XslCompiledTransform();
                if (enableCode) {
                    System.Xml.Xsl.XsltSettings xslSetting = new System.Xml.Xsl.XsltSettings();
                    xslSetting.EnableScript = true;
                    xslTransform.Load(System.Xml.XmlReader.Create(new System.IO.StringReader(template)), xslSetting, new System.Xml.XmlUrlResolver());
                } else {
                    xslTransform.Load(System.Xml.XmlReader.Create(new System.IO.StringReader(template)));
                }
                if (enableCache) {
                    try {
                        _xslLibrary.Add(template, xslTransform);
                    } catch {
                        //do nothing
                    }
                }
            }
            return xslTransform;
        }
    }
}
