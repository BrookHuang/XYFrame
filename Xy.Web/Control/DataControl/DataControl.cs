using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public class DataControl : Xy.Web.Control.IControl {

        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }

        private ThreadEntity _threadEntity;
        private bool _enableScript = false;
        private bool _enableCode = false;
        private bool _enableCache = true;

        //common
        private Xy.Data.IDataBuilder _dataBuilder;
        private System.Collections.Specialized.NameValueCollection _tagList;

        //for template
        private string _root;
        private string _xsltPath;
        private System.Xml.XPath.XPathDocument _datastring;
        private string _xsltCache;
        private string _xsltString;
        private string _xsltParameter;
        private bool _xsltLoaded;

        public DataControl() {
            _isNeedData = false;
            _tagList = new System.Collections.Specialized.NameValueCollection();
            _xsltLoaded = false;
        }

        public void Init(System.Collections.Specialized.NameValueCollection createTag, string map, int index) {
            for (int i = 0; i < createTag.Count; i++) {
                switch (createTag.Keys[i]) {
                    case "Provider":
                        switch (createTag[i]) {
                            case "Procedure":
                                _dataBuilder = new Xy.Data.DataBuilders.ProcedureBuilder();
                                break;
                            case "Data":
                                _dataBuilder = new Xy.Data.DataBuilders.DataBuilder();
                                break;
                            case "Request":
                                _dataBuilder = new Xy.Data.DataBuilders.RequestBuilder();
                                break;
                            case "XML":
                                _dataBuilder = new Xy.Data.DataBuilders.XMLBuilder();
                                break;
                            default:
                                try { 
                                    _dataBuilder = Runtime.Web.DataBuilderFactory.Get(createTag[i]);
                                } catch (NullReferenceException ex) {
                                    throw new Exception(string.Format("Cannot found data builder:{0}, please assign a correct provider.", createTag[i]));
                                }
                                if(_dataBuilder == null)
                                    throw new Exception(string.Format("Cannot found data builder:{0}, please assign a correct provider.", createTag[i]));
                                break;
                        }
                        break;
                    case "Xslt":
                        _xsltPath = createTag[i];
                        break;
                    case "XsltParameter":
                        _xsltParameter = createTag[i];
                        break;
                    case "EnableScript":
                        _enableScript = string.Compare(createTag[i], "true", true) == 0;
                        break;
                    case "EnableCode":
                        _enableCode = string.Compare(createTag[i], "true", true) == 0;
                        break;
                    case "EnableCache":
                        _enableCache = string.Compare(createTag[i], "true", true) == 0;
                        break;
                    case "Root":
                        _root = createTag[i];
                        break;
                    default:
                        _tagList.Add(createTag.Keys[i], createTag[i]);
                        break;

                }
            }
            _map = string.Concat(map, "DataControl", index);
            _isNeedData = string.IsNullOrEmpty(_xsltPath);
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex += 1;
            _dataBuilder.Init(_tagList);
            _datastring = _dataBuilder.HandleData(CurrentPageClass);

            HTMLContainer _temp = new HTMLContainer(ContentContainer.Encoding);

            if (!_xsltLoaded || _threadEntity.WebSetting.DebugMode) {
                _xsltCache = HandleXSLT();
            }
            _xsltString = _xsltCache;
            _xsltString = _dataBuilder.InsertParameter(_xsltString);
            if (!string.IsNullOrEmpty(_xsltParameter)) {
                _xsltString = InsertParameter(_xsltString, CurrentPageClass);
            }
            _temp.Write(HandleHTML(_datastring, _xsltString));

            if (_enableScript && _temp.Length > 0) {
                Control.ControlAnalyze _controls = new ControlAnalyze(CurrentThreadEntity, this.Map, true);
                _controls.SetContent(_temp.ToArray());
                _controls.Analyze();
                _temp.Clear();
                _controls.Handle(CurrentPageClass, _temp);
            }
            ContentContainer.Write(_temp);
            _threadEntity.ControlIndex -= 1;
        }

        private string HandleXSLT() {
            if (!string.IsNullOrEmpty(_xsltPath)) {
                using (System.IO.FileStream fs = new System.IO.FileStream(_threadEntity.WebSetting.XsltDir + _xsltPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(fs)) {
                        _xsltLoaded = true;
                        return sr.ReadToEnd();
                    }
                }
            } else {
                StringBuilder xsltsb = new StringBuilder();
                xsltsb.Append("<?xml version=\"1.0\" encoding=\"").Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\"?>");
                xsltsb.Append("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"");
                if(_enableCode){
                    xsltsb.Append(" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\"");
                }
                if (_enableScript) {
                    xsltsb.Append(" xmlns:xyxsl=\"http://www.xiaoyang.com/HodeNamespace\"");
                }
                xsltsb.Append(">");
                xsltsb.Append("<xsl:output method=\"html\" omit-xml-declaration=\"yes\" version=\"1.0\" encoding=\"").Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\" />");
                if (_enableScript) {
                    xsltsb.Append("<xsl:namespace-alias stylesheet-prefix=\"xyxsl\" result-prefix=\"xsl\"/>");
                }
                if (!string.IsNullOrEmpty(_root)) {
                    xsltsb.Append(string.Format("<xsl:template match=\"{0}\">", _root));
                } else {
                    string _temp = _dataBuilder.GetRoot();
                    if (string.IsNullOrEmpty(_temp)) {
                        xsltsb.Append("<xsl:template match=\"DataTable/DataItem\">");
                    } else {
                        xsltsb.Append(string.Format("<xsl:template match=\"{0}\">", _temp));
                    }
                }
                xsltsb.Append(_innerData.ToString());
                xsltsb.Append("</xsl:template>");
                xsltsb.Append("</xsl:stylesheet>");
                _xsltLoaded = true;
                return xsltsb.ToString();
            }
        }

        private string InsertParameter(string XsltString, Page.PageAbstract CurrentPageClass) {
            StringBuilder xsltParametersb = new StringBuilder();
            System.Collections.Specialized.NameValueCollection _xsltParameterCollection = Xy.Tools.Control.Tag.Decode(_xsltParameter);
            foreach (string _key in _xsltParameterCollection.Keys) {
                xsltParametersb.Append(string.Format(@"<xsl:variable name=""{0}"">{1}</xsl:variable>", _key, Xy.Tools.Control.Tag.TransferValue(_key,_xsltParameterCollection[_key],CurrentPageClass)));
            }
            xsltParametersb.Append("<xsl:template");
            return XsltString.Replace("<xsl:template", xsltParametersb.ToString());
        }

        private byte[] HandleHTML(System.Xml.XPath.XPathDocument DataString, string XsltString) {
            if (DataString != null) {
                System.Xml.Xsl.XslCompiledTransform xslTransform = Xy.Web.Cache.XslCompiledTransform.Get(XsltString, _enableCode, _enableCache);
                return Xy.Tools.IO.XML.TransfromXmlStringToHtml(xslTransform, DataString);
            }
            return new byte[0];
        }
    }
}
