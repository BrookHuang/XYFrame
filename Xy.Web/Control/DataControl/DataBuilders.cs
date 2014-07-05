using System;
using System.Collections.Generic;
using System.Text;


namespace Xy.Data {
    public interface IDataBuilder {
        void Init(System.Collections.Specialized.NameValueCollection Tags);
        System.Xml.XPath.XPathDocument HandleData(Xy.Web.Page.PageAbstract CurrentPageClass);
        string InsertParameter(string xslt);
        string GetRoot();
    }
}

namespace Xy.Data.DataBuilders {

    public class ProcedureBuilder : IDataBuilder {
        private string _dataConnection;
        private string _command;
        private string _procedureName;
        private string _parameter;
        private string _defaultParameter;
        private DataParameterCollection _dataParameterCollection = null;
        private System.Data.DataTable _data;

        public void Init(System.Collections.Specialized.NameValueCollection Tags) {
            for (int i = 0; i < Tags.Count; i++) {
                switch (Tags.Keys[i]) {
                    case "Connection":
                        _dataConnection = Tags[i];
                        break;
                    case "Command":
                        _command = Tags[i];
                        break;
                    case "Procedure":
                        _procedureName = Tags[i];
                        break;
                    case "Parameter":
                        _parameter = Tags[i];
                        break;
                    case "DefaultParameter":
                        _defaultParameter = Tags[i];
                        break;
                }
            }
            if (_dataParameterCollection == null) _dataParameterCollection = new DataParameterCollection();
        }

        public System.Xml.XPath.XPathDocument HandleData(Xy.Web.Page.PageAbstract CurrentPageClass) {
            Xy.Data.Procedure _procedure;
            if (!string.IsNullOrEmpty(_command)) {
                _procedure = new Data.Procedure("XYControlInnerProcedure", _command);
            } else {
                _procedure = new Data.Procedure(_procedureName);
            }
            if (!string.IsNullOrEmpty(_parameter) || !string.IsNullOrEmpty(_defaultParameter)) {
                if (!_dataParameterCollection.Inited) {
                    _dataParameterCollection.AnalyzeParameter(_parameter, _defaultParameter);
                }
                _dataParameterCollection.HandleValue(CurrentPageClass);
                if (_dataParameterCollection.HasContent) {
                    _dataParameterCollection.FillParameter(_procedure);
                }
            }
            if (!string.IsNullOrEmpty(_dataConnection)) {
                _procedure.SetDataBase(new Xy.Data.DataBase(_dataConnection));
            }
            _data = _procedure.InvokeProcedureFill();
            if (_dataParameterCollection.HasReturn) {
                _dataParameterCollection.GetReturnParameter(_procedure);
            }
            return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(Xy.Tools.IO.XML.ConvertDataTableToXML(_data)));
        }

        public string InsertParameter(string xslt) {
            return _dataParameterCollection.GenerateXsltParameter(xslt);
        }

        public string GetRoot() {
            return string.Empty;
        }
    }

    public class DataBuilder : IDataBuilder {

        private string _dataName;

        public void Init(System.Collections.Specialized.NameValueCollection Tags) {
            for (int i = 0; i < Tags.Count; i++) {
                switch (Tags.Keys[i]) {
                    case "Name":
                        _dataName = Tags[i];
                        break;
                }
            }
        }

        public System.Xml.XPath.XPathDocument HandleData(Web.Page.PageAbstract CurrentPageClass) {
            Xy.Web.Page.PageDataItem pageData;
            if (CurrentPageClass.PageData.TryGetValue(_dataName, out pageData)) {
                return pageData.GetDataXml();
            } else {
                return new System.Xml.XPath.XPathDocument(new System.IO.StringReader("<Data></Data>"));
            }
        }

        public string InsertParameter(string xslt) {
            return xslt;
        }

        public string GetRoot() {
            return string.Empty;
        }
    }

    public class RequestBuilder : IDataBuilder {

        public void Init(System.Collections.Specialized.NameValueCollection Tags) { }

        public System.Xml.XPath.XPathDocument HandleData(Web.Page.PageAbstract CurrentPageClass) {
            StringBuilder _tempsb = new StringBuilder();
            foreach (string _key in CurrentPageClass.Request.Values) {
                if (_tempsb.Length == 0) { _tempsb.Append("<Request>"); }
                _tempsb.Append(string.Format("<{0}>{1}</{0}>", _key, CurrentPageClass.Request[_key]));
            }
            if (_tempsb.Length > 0) {
                _tempsb.Append("</Request>");
            } else {
                _tempsb.Append("<Request></Request>");
            }
            return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_tempsb.ToString()));
        }

        public string InsertParameter(string xslt) {
            return xslt;
        }

        public string GetRoot() {
            return string.Empty;
        }
    }

    public class XMLBuilder : IDataBuilder {

        private string _dataPath;

        public void Init(System.Collections.Specialized.NameValueCollection Tags) {
            for (int i = 0; i < Tags.Count; i++) {
                switch (Tags.Keys[i]) {
                    case "Path":
                        _dataPath = Tags[i];
                        break;
                }
            }
        }

        public System.Xml.XPath.XPathDocument HandleData(Web.Page.PageAbstract CurrentPageClass) {
            if (!string.IsNullOrEmpty(_dataPath)) {
                System.IO.StreamReader _tempXmlData = new System.IO.StreamReader(Xy.AppSetting.DataDir + _dataPath, CurrentPageClass.WebSetting.Encoding);
                return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_tempXmlData.ReadToEnd()));
            }
            return new System.Xml.XPath.XPathDocument(new System.IO.StringReader("<Xml></Xml>"));
        }

        public string InsertParameter(string xslt) {
            return xslt;
        }

        public string GetRoot() {
            return string.Empty;
        }
    }
}