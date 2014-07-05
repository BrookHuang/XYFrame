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
        private Xy.Data.DataBuilder.IDataBuilder _dataBuilder;
        private System.Collections.Specialized.NameValueCollection _tagList;

        //for template
        private string _root;
        private string _xsltPath;
        private System.Xml.XPath.XPathDocument _datastring;
        private string _xsltstring;
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
                                _dataBuilder = new Xy.Data.DataBuilder.ProcedureBuilder();
                                break;
                            case "Data":
                                _dataBuilder = new Xy.Data.DataBuilder.DataBuilder();
                                break;
                            case "Request":
                                _dataBuilder = new Xy.Data.DataBuilder.RequestBuilder();
                                break;
                            case "XML":
                                _dataBuilder = new Xy.Data.DataBuilder.XMLBuilder();
                                break;
                            default:
                                throw new Exception("缺少正确的数据支持,请将控件的Provide属性设置为Procedure,Data,URL,XML其中的一个");
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
            if (!_xsltLoaded || _threadEntity.WebSetting.DebugMode) {
                _xsltstring = HandleXSLT();
            }
            HTMLContainer _temp = new HTMLContainer(ContentContainer.Encoding);
            _xsltstring = _dataBuilder.InsertParameter(_xsltstring);
            if (!string.IsNullOrEmpty(_xsltParameter)) {
                _xsltstring = InsertParameter(_xsltstring, CurrentPageClass);
            }
            _temp.Write(HandleHTML(_datastring, _xsltstring));

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
                    xsltsb.Append("<xsl:template match=\"DataTable/DataItem\">");
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
                string _value = _xsltParameterCollection[_key];
                string tempGetName = _key;
                if (_value.IndexOf(':') > 0) {
                    tempGetName = _value.Substring(_value.LastIndexOf(':') + 1);
                    _value = _value.Substring(0, _value.LastIndexOf(':'));
                }

                switch (_value) {
                    case "Request":
                        _value = CurrentPageClass.Request[tempGetName];
                        break;
                    case "Url":
                        _value = CurrentPageClass.Request.QueryString[tempGetName];
                        break;
                    case "Form":
                        _value = CurrentPageClass.Request.Form[tempGetName];
                        break;
                    case "Group":
                        _value = CurrentPageClass.Request.GroupString[tempGetName];
                        break;
                    case "Data":
                        _value = CurrentPageClass.PageData[tempGetName] == null ? string.Empty : CurrentPageClass.PageData[tempGetName].GetDataString();
                        break;
                    case "Config":
                        if (tempGetName == _key) {
                            _value = CurrentPageClass.WebSetting.Name;
                        } else {
                            _value = CurrentPageClass.WebSetting.Config[tempGetName];
                        }
                        break;
                }
                xsltParametersb.Append(string.Format(@"<xsl:variable name=""{0}"">{1}</xsl:variable>", _key, _value));
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

namespace Xy.Data.DataBuilder {
    public interface IDataBuilder {
        void Init(System.Collections.Specialized.NameValueCollection Tags);
        System.Xml.XPath.XPathDocument HandleData(Xy.Web.Page.PageAbstract CurrentPageClass);
        string InsertParameter(string xslt);
    }

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

        //<% @Data Provide="Data" Name="IsFix" EnableScript="True" DefaultRoot="True" %>
        //<% @Data Provide="Data" Name="Pages" DefaultRoot="True" EnableScript="True" %>
        //<% @Data Provide="ADO" Command="select a.[ID],a.[Name],a.[Nickname],a.[Email],b.[Name] as 'GroupName' from [User] a left join [UserGroup] b on a.UserGroup = b.ID " EnableScript="True" %>
        //<% @Data Provide="ADO" Command="select [Nickname],[Description],[Sex],[Hometown],[Birthday] from [User] left join [UserExtra] on [User].ID = [UserExtra].UserID where [User].ID = @UserID" Parameter="{ UserID='Data:CurrentUser.ID|i' }" %>
        //<% @Data Provide="ADO" Procedure="ClickJia_Post_PostGroup_GetList" Parameter="{ PageIndex='' PageSize='21' OrderBy='[Member] Desc' }" Xslt="group.xslt" EnableScript="True" %>
        //<% @Data Provide="ADO" Procedure="XiaoYang_Article_Article_GetList" Parameter="{ ClassID='Group|i' PageIndex='Url|i' PageSize='10|i' #TotalCount='-1|i' }" DefaultParameter="{ PageIndex='0|i' ClassID='-1|i' }" DefaultRoot="True" EnableScript="True" Xslt="ArticleList.xslt" %>
        //<% @Data Provide="ADO" Procedure="ClickJia_Message_Message_GetList" Parameter="{ UserID='Data:CurrentUser.ID|i' PageIndex='0' }" EnableScript="True" %>
        private class DataParameterCollection : Dictionary<string, DataParameter> {

            private bool _inited;
            public bool Inited { get { return _inited; } }
            public bool HasContent { get { return this.Count > 0; } }
            private bool _hasReturn;
            public bool HasReturn { get { return _hasReturn; } }

            //private static char[] _trimArray = new char[2] { '{', '}' };

            public DataParameterCollection() {
                _hasReturn = false;
                _inited = false;
            }

            public void AnalyzeParameter(string ParameterString, string DefaultParameterString) {
                System.Collections.Specialized.NameValueCollection values = null;
                if (!string.IsNullOrEmpty(ParameterString)) values = Xy.Tools.Control.Tag.Decode(ParameterString);
                System.Collections.Specialized.NameValueCollection defaultValues = null;
                if (!string.IsNullOrEmpty(DefaultParameterString)) defaultValues = Xy.Tools.Control.Tag.Decode(DefaultParameterString);
                if (values != null) {
                    for (int i = 0; i < values.Count; i++) {
                        DataParameter temp = new DataParameter(values.Keys[i], values[i]);
                        base.Add(temp.Name, temp);
                    }
                }
                if (defaultValues != null) {
                    for (int i = 0; i < defaultValues.Count; i++) {
                        if (base.ContainsKey(defaultValues.Keys[i])) {
                            base[defaultValues.Keys[i]].SetDefaultValue(defaultValues[i]);
                        } else {
                            DataParameter temp = new DataParameter(defaultValues.Keys[i], defaultValues[i]);
                            base.Add(temp.Name, temp);
                        }
                    }
                }
                _inited = true;
            }

            public void HandleValue(Xy.Web.Page.PageAbstract CurrentPageClass) {
                foreach (DataParameter _item in this.Values) {
                    _item.HandleValue(CurrentPageClass);
                }
            }

            public void FillParameter(Xy.Data.Procedure Procedure) {
                foreach (DataParameter _item in this.Values) {
                    Xy.Data.ProcedureParameter _param = new Xy.Data.ProcedureParameter(_item.Name, _item.Type, _item.Validate, _item.Value);
                    if (_item.Direction != System.Data.ParameterDirection.Input) {
                        _param.Direction = _item.Direction;
                        if (!_hasReturn) _hasReturn = true;
                    }
                    Procedure.AddItem(_param);
                }
            }

            public void GetReturnParameter(Xy.Data.Procedure Procedure) {
                foreach (Xy.Data.ProcedureParameter _item in Procedure) {
                    if (_item.Direction != System.Data.ParameterDirection.Input) {
                        this[_item.Name].UpdateReturnValue(_item.Value.ToString());
                    }
                }
            }

            public string GenerateXsltParameter(string XsltString) {
                StringBuilder xsltParametersb = new StringBuilder();
                foreach (DataParameter _item in this.Values) {
                    xsltParametersb.Append(string.Format(@"<xsl:variable name=""{0}"">{1}</xsl:variable>", _item.NameText, _item.Value));
                }
                xsltParametersb.Append("<xsl:template");
                return XsltString.Replace("<xsl:template", xsltParametersb.ToString());
            }
        }

        //Parameter="{ UserID='Data:CurrentUser.ID|i' }"
        //Parameter="{ ClassID='Group|i' PageIndex='Url|i' PageSize='10|i' #TotalCount='-1|i' }" DefaultParameter="{ PageIndex='0|i' ClassID='-1|i' }"
        private class DataParameter {

            string _parameterName;
            string _parameterValue;
            string _parameterValueText;
            string _parameterDefaultValueText;
            //TODO implement validation function.
            string _parameterValidate = string.Empty;
            System.Data.DbType _parameterType;
            System.Data.ParameterDirection _parameterDirection;
            bool _isHandled;

            public string NameText { get { return _parameterName; } }
            public string Name { get { return _parameterName; } }
            public System.Data.DbType Type { get { return _parameterType; } }
            public object Value {
                get {
                    if (!_isHandled) { throw new Exception("can not use an un-init parameter."); }
                    if (this.IsEmpty) return string.Empty;
                    switch (_parameterType) {
                        case System.Data.DbType.Int16:
                            return Convert.ToInt16(_parameterValue);
                        case System.Data.DbType.Int32:
                            return Convert.ToInt32(_parameterValue);
                        case System.Data.DbType.Int64:
                            return Convert.ToInt64(_parameterValue);
                        default:
                            return _parameterValue;
                    }
                }
            }
            public string Validate { get { return _parameterValidate; } }
            public System.Data.ParameterDirection Direction { get { return _parameterDirection; } }
            public bool IsHandled { get { return _isHandled; } }
            public bool IsEmpty { get { return string.IsNullOrEmpty(_parameterValue); } }

            public DataParameter(string inName, string inValue) {
                switch (inName[0]) {
                    case '!':
                        _parameterDirection = System.Data.ParameterDirection.ReturnValue;
                        break;
                    case '#':
                        _parameterDirection = System.Data.ParameterDirection.InputOutput;
                        break;
                    default:
                        _parameterDirection = System.Data.ParameterDirection.Input;
                        break;
                }
                _parameterName = inName.TrimStart('@').TrimStart('#').TrimStart('!');
                _parameterValueText = inValue;
                _isHandled = false;
            }

            public void SetDefaultValue(string inValue) {
                _parameterDefaultValueText = inValue;
            }

            public void UpdateReturnValue(string inValue) {
                _parameterValue = inValue;
                _isHandled = true;
            }

            public void HandleValue(Xy.Web.Page.PageAbstract PaCurrentPageClassge) {
                handleValue(_parameterValueText, PaCurrentPageClassge);
                if (string.IsNullOrEmpty(_parameterValue))
                    handleValue(_parameterDefaultValueText, PaCurrentPageClassge);
                _isHandled = true;
            }

            private void handleValue(string value, Xy.Web.Page.PageAbstract CurrentPageClass) {
                if (string.IsNullOrEmpty(value)) return;
                _parameterType = System.Data.DbType.String;
                if (value.IndexOf('|') >= 0) {
                    switch (value.Substring(value.LastIndexOf('|') + 1)) {
                        case "s":
                        case "short": _parameterType = System.Data.DbType.Int16; break;
                        case "i":
                        case "int": _parameterType = System.Data.DbType.Int32; break;
                        case "l":
                        case "long": _parameterType = System.Data.DbType.Int64; break;
                        case "d":
                        case "date": _parameterType = System.Data.DbType.Date; break;
                        case "t":
                        case "time": _parameterType = System.Data.DbType.DateTime; break;
                    }
                    value = value.Substring(0, value.LastIndexOf('|'));
                }

                string tempGetName = _parameterName;
                if (value.IndexOf(':') > 0) {
                    tempGetName = value.Substring(value.LastIndexOf(':') + 1);
                    value = value.Substring(0, value.LastIndexOf(':'));
                }

                switch (value) {
                    case "Request":
                        value = CurrentPageClass.Request[tempGetName];
                        break;
                    case "Url":
                        value = CurrentPageClass.Request.QueryString[tempGetName];
                        break;
                    case "Form":
                        value = CurrentPageClass.Request.Form[tempGetName];
                        break;
                    case "Group":
                        value = CurrentPageClass.Request.GroupString[tempGetName];
                        break;
                    case "Data":
                        value = CurrentPageClass.PageData[tempGetName] == null ? string.Empty : CurrentPageClass.PageData[tempGetName].GetDataString();
                        break;
                    case "Config":
                        if (tempGetName == _parameterName) {
                            value = CurrentPageClass.WebSetting.Name;
                        } else {
                            value = CurrentPageClass.WebSetting.Config[tempGetName];
                        }
                        break;
                }
                if (!string.IsNullOrEmpty(value)) _parameterValue = value;
            }

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
    }
}