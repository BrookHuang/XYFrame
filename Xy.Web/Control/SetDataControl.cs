using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    #region old DataControl
    //public class DataControl_old : Xy.Web.Control.IControl {
    //    private string _id;
    //    public string ID { get { return _id; } set { _id = value; } }
    //    private int _insertIndex;
    //    public int InsertIndex { get { return _insertIndex; } set { _insertIndex = value; } }
    //    private ThreadEntity _threadEntity;
    //    public ThreadEntity ThreadEntity { get { return _threadEntity; } set { _threadEntity = value; } }
    //    private bool _isNeedData;
    //    public bool IsNeedData { get { return _isNeedData; } }
    //    private List<byte> _innerHtml;
    //    public List<byte> InnerHtml { get { return _innerHtml; } }
    //    public int Length { get { return _innerHtml.Count; } }
    //    private List<byte> _innerData;
    //    public List<byte> InnerData { get { return _innerData; } set { _innerData = value; } }

    //    private string _command;
    //    private string _procedure;
    //    private string _dataProvide;
    //    private string _dataconnection;
    //    private System.Data.DataTable _data;
    //    private string _datastring;
    //    private string _datapath;
    //    private string _parameter;
    //    private System.Collections.Specialized.NameValueCollection _defaultParameter;
    //    private string _dataName;
    //    private static System.Text.RegularExpressions.Regex findparam = new System.Text.RegularExpressions.Regex(@"@\w+", System.Text.RegularExpressions.RegexOptions.Compiled);
    //    private string _xsltstring;
    //    private bool _enableScript = false;
    //    private bool _defaultTemplate = false;
    //    private bool _defaultRoot = false;
    //    private string _root = string.Empty;
    //    private System.Collections.Specialized.NameValueCollection _paramereturns;
    //    private System.Xml.Xsl.XslCompiledTransform xsl;

    //    public DataControl_old() {
    //        _isNeedData = false;
    //        _insertIndex = 0;
    //        _innerHtml = new List<byte>();
    //        _paramereturns = new System.Collections.Specialized.NameValueCollection();
    //    }

    //    public void Init(System.Collections.Specialized.NameValueCollection CreateTag) {
    //        for (int i = 0; i < CreateTag.Count; i++) {
    //            switch (CreateTag.Keys[i]) {
    //                case "Provide":
    //                    _dataProvide = CreateTag[i];
    //                    break;
    //                case "Connection":
    //                    _dataconnection = CreateTag[i];
    //                    break;
    //                case "Command":
    //                    _command = CreateTag[i];
    //                    break;
    //                case "DataPath":
    //                    _datapath = CreateTag[i];
    //                    break;
    //                case "Procedure":
    //                    _procedure = CreateTag[i];
    //                    break;
    //                case "Parameter":
    //                    _parameter = CreateTag[i];
    //                    break;
    //                case "DefaultParameter":
    //                    _defaultParameter = Control.ControlAnalyze.DeCodeMark(CreateTag[i]);
    //                    break;
    //                case "Name":
    //                    _dataName = CreateTag[i];
    //                    break;
    //                case "Xslt":
    //                    _xsltstring = CreateTag[i];
    //                    break;
    //                case "EnableScript":
    //                    _enableScript = string.Compare(CreateTag[i], "true", true) == 0;
    //                    break;
    //                case "DefaultTemplate":
    //                    _defaultTemplate = string.Compare(CreateTag[i], "true", true) == 0;
    //                    break;
    //                case "DefaultRoot":
    //                    _defaultRoot = string.Compare(CreateTag[i], "true", true) == 0;
    //                    break;
    //                case "Root":
    //                    _defaultRoot = false;
    //                    _root = CreateTag[i];
    //                    break;
    //            }
    //        }
    //        _isNeedData = string.IsNullOrEmpty(_xsltstring);
    //    }

    //    public void Handle(ref ThreadEntity ThreadEntity) {
    //        _threadEntity = ThreadEntity;
    //        _threadEntity.ControlIndex += 1;
    //        if (_innerData == null || (!_isNeedData && _threadEntity.WebSetting.DebugMode)) {
    //            LoadXSLT();
    //        }
    //        //if (_innerData.Count > 0) {
    //        //    TrimXSL();
    //        //}
    //        HandleData();
    //        HandleContent();
    //        if (_enableScript) {
    //            ControlCollection af = new Control.ControlAnalyze(ref _innerHtml, _threadEntity, true).ControlCollection;
    //            af.Handle(ref _innerHtml, ref _threadEntity);
    //        }
    //        _threadEntity.ControlIndex -= 1;
    //    }

    //    public string BuildHtmlString() {
    //        return _threadEntity.WebSetting.Encoding.GetString(_innerHtml.ToArray());
    //    }

    //    public IControl GetInstance() {
    //        return new DataControl();
    //    }

    //    private void HandleData() {
    //        switch (_dataProvide) {
    //            case "ADO":
    //                bool seekmode = false; //false为command模式,true为procedure模式
    //                _paramereturns.Clear();
    //                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DataSetting.GetConnectionString(string.IsNullOrEmpty(_dataconnection) ? DataSetting.DEFAULTCONNECTIONNAME : _dataconnection));
    //                System.Data.SqlClient.SqlCommand cmd = null;
    //                if (!string.IsNullOrEmpty(_command)) {
    //                    cmd = new System.Data.SqlClient.SqlCommand(_command, con);
    //                }
    //                if (!string.IsNullOrEmpty(_procedure)) {
    //                    cmd = new System.Data.SqlClient.SqlCommand(_procedure, con);
    //                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //                    seekmode = true;
    //                }
    //                if (cmd == null) throw new Exception("您未提供查询语句或存储过程");
    //                if (!seekmode) {
    //                    switch (_parameter) {
    //                        case "Request":
    //                            foreach (System.Text.RegularExpressions.Match match in findparam.Matches(_command)) {
    //                                string tempParameterValue = _threadEntity.Page.Request[match.Value.Substring(1)];
    //                                if (string.IsNullOrEmpty(tempParameterValue)) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, GetDefaultParameter(match.Value)));
    //                                } else {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, tempParameterValue));
    //                                }
    //                            }
    //                            break;
    //                        case "Url":
    //                            foreach (System.Text.RegularExpressions.Match match in findparam.Matches(_command)) {
    //                                string tempParameterValue = _threadEntity.Page.Request.QueryString[match.Value.Substring(1)];
    //                                if (string.IsNullOrEmpty(tempParameterValue)) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, GetDefaultParameter(match.Value)));
    //                                } else {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, tempParameterValue));
    //                                }
    //                            }
    //                            break;
    //                        case "Form":
    //                            foreach (System.Text.RegularExpressions.Match match in findparam.Matches(_command)) {
    //                                string tempParameterValue = _threadEntity.Page.Request.Form[match.Value.Substring(1)];
    //                                if (string.IsNullOrEmpty(tempParameterValue)) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, GetDefaultParameter(match.Value)));
    //                                } else {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, tempParameterValue));
    //                                }
    //                            }
    //                            break;
    //                        case "Group":
    //                            foreach (System.Text.RegularExpressions.Match match in findparam.Matches(_command)) {
    //                                string tempParameterValue = _threadEntity.Page.Request.GroupString[match.Value.Substring(1)];
    //                                if (string.IsNullOrEmpty(tempParameterValue)) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, GetDefaultParameter(match.Value)));
    //                                } else {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, tempParameterValue));
    //                                }
    //                            }
    //                            break;
    //                        case "Data":
    //                            foreach (System.Text.RegularExpressions.Match match in findparam.Matches(_command)) {
    //                                Page.PageDataItem tempParameterValue = _threadEntity.Page.PageData[match.Value.Substring(1)];
    //                                if (tempParameterValue == null) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, GetDefaultParameter(match.Value)));
    //                                } else {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(match.Value, tempParameterValue.GetDataString()));
    //                                }
    //                            }
    //                            break;
    //                        default:
    //                            if (!string.IsNullOrEmpty(_parameter)) {
    //                                System.Collections.Specialized.NameValueCollection temp = ControlAnalyze.DeCodeMark(_parameter);
    //                                if (temp.Length > 0) {
    //                                    foreach (TagItem _item in temp) {
    //                                        fillParameter(_item, cmd);
    //                                    }
    //                                } else {
    //                                    throw new Exception("未提供正确的参数支持");
    //                                }
    //                            }
    //                            break;
    //                    }
    //                } else {
    //                    System.Collections.IEnumerator ietr = null;
    //                    switch (_parameter) {
    //                        case "Request":
    //                            ietr = _threadEntity.Page.Request.GetEnumerator();
    //                            while (ietr.MoveNext()) {
    //                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(ietr.Current.ToString(), _threadEntity.Page.Request[ietr.Current.ToString()]));
    //                            }
    //                            break;
    //                        case "Url":
    //                            ietr = _threadEntity.Page.Request.QueryString.GetEnumerator();
    //                            while (ietr.MoveNext()) {
    //                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(ietr.Current.ToString(), _threadEntity.Page.Request.QueryString[ietr.Current.ToString()]));
    //                            }
    //                            break;
    //                        case "Form":
    //                            ietr = _threadEntity.Page.Request.Form.GetEnumerator();
    //                            while (ietr.MoveNext()) {
    //                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(ietr.Current.ToString(), _threadEntity.Page.Request.Form[ietr.Current.ToString()]));
    //                            }
    //                            break;
    //                        case "Group":
    //                            ietr = _threadEntity.Page.Request.GroupString.GetEnumerator();
    //                            while (ietr.MoveNext()) {
    //                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(ietr.Current.ToString(), _threadEntity.Page.Request.GroupString[ietr.Current.ToString()]));
    //                            }
    //                            break;
    //                        case "Data":
    //                            foreach (Page.PageDataItem _item in _threadEntity.Page.PageData) {
    //                                if (_item.Type == Page.PageDataType.String) {
    //                                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(_item.Name, _item.GetDataString()));
    //                                }
    //                            }
    //                            break;
    //                        default:
    //                            if (!string.IsNullOrEmpty(_parameter)) {
    //                                System.Collections.Specialized.NameValueCollection temp = ControlAnalyze.DeCodeMark(_parameter);
    //                                if (temp.Length > 0) {
    //                                    foreach (TagItem _item in temp) {
    //                                        fillParameter(_item, cmd);
    //                                    }
    //                                } else {
    //                                    throw new Exception("未提供正确的参数支持");
    //                                }
    //                            }
    //                            break;
    //                    }
    //                }
    //                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(cmd);
    //                _data = new System.Data.DataTable();
    //                sda.Fill(_data);
    //                _datastring = Xy.Tools.DataTools.ConvertDataTableToXML(_data);
    //                foreach (System.Data.SqlClient.SqlParameter _item in cmd.Parameters) {
    //                    //_datastring = _datastring.Insert(_datastring.Length - 12, string.Format("<{0}>{1}</{0}>", _item.ParameterName.Trim('@'), _item.Value.ToString()));
    //                    _paramereturns.Add(_item.ParameterName.Trim('@'), _item.Value.ToString());
    //                }
    //                break;
    //            case "Data":
    //                Page.PageDataItem pageData;
    //                if (_threadEntity.Page.PageData.TryGetValue(_dataName, out pageData)) {
    //                    _datastring = pageData.GetDataXml().CreateNavigator().OuterXml;
    //                } else {
    //                    _datastring = "<Data></Data>";
    //                }
    //                break;
    //            case "Request":
    //                StringBuilder _tempsb = new StringBuilder();
    //                foreach (string _key in _threadEntity.Page.Request) {
    //                    if (_tempsb.Length == 0) { _tempsb.Append("<Request>"); }
    //                    _tempsb.Append(string.Format("<{0}>{1}</{0}>", _key, _threadEntity.Page.Request[_key]));
    //                }
    //                if (_tempsb.Length > 0) {
    //                    _tempsb.Append("</Request>");
    //                } else {
    //                    _tempsb.Append("<Request></Request>");
    //                }
    //                _datastring = _tempsb.ToString();
    //                break;
    //            case "XML":
    //                if (!string.IsNullOrEmpty(_datapath)) {
    //                    System.IO.StreamReader _tempXmlData = new System.IO.StreamReader(Xy.AppSetting.DataDir + _datapath, _threadEntity.WebSetting.Encoding);
    //                    _datastring = _tempXmlData.ReadToEnd();
    //                }
    //                break;
    //            default:
    //                throw new Exception("缺少正确的数据支持,请将控件的Provide属性设置为ADO,Data,URL,XML其中的一个");
    //        }
    //    }

    //    private void HandleContent() {
    //        if (!string.IsNullOrEmpty(_datastring)) {
    //            System.IO.StringReader xml = new System.IO.StringReader(_datastring);
    //            System.Xml.XPath.XPathDocument xpathDoc = new System.Xml.XPath.XPathDocument(xml);

    //            //if (xsl == null || _threadEntity.WebSetting.DebugMode) {
    //            if (xsl == null) {
    //                xsl = HandleXSLT();
    //            }

    //            _innerHtml = new List<byte>(Xy.Tools.WebTools.TransfromXmlStringToHtml(xsl, xpathDoc));
    //        }
    //    }

    //    private void fillParameter(TagItem item, System.Data.SqlClient.SqlCommand cmd) {
    //        char _parameterType = char.MinValue;
    //        bool _isCertainType = false;
    //        bool _isDefaultParameter = false;
    //        string _parameterNameText = item.Name.TrimStart('@').TrimStart('#').TrimStart('!');
    //        string _parameterName = "@" + _parameterNameText;
    //        string _parameterValue = item.Value;
    //        if (item.Value.IndexOf('|') > 0 && (item.Value.LastIndexOf('|') == item.Value.Length - 2)) {
    //            _isCertainType = true;
    //            _parameterType = _parameterValue.Substring(_parameterValue.LastIndexOf('|') + 1)[0];
    //            _parameterValue = _parameterValue.Substring(0, _parameterValue.LastIndexOf('|'));
                
    //        }

    //        if (_parameterValue.IndexOf(':') > 0) {
    //            string _internalName = _parameterValue.Substring(_parameterValue.LastIndexOf(':') + 1);
    //            _parameterValue = _parameterValue.Substring(0, _parameterValue.LastIndexOf(':'));
    //            switch (_parameterValue) {
    //                case "Request":
    //                    _parameterValue = _threadEntity.Page.Request[_internalName];
    //                    break;
    //                case "Url":
    //                    _parameterValue = _threadEntity.Page.Request.QueryString[_internalName];
    //                    break;
    //                case "Form":
    //                    _parameterValue = _threadEntity.Page.Request.Form[_internalName];
    //                    break;
    //                case "Group":
    //                    _parameterValue = _threadEntity.Page.Request.GroupString[_internalName];
    //                    break;
    //                case "Data":
    //                    _parameterValue = _threadEntity.Page.PageData[_internalName].GetDataString();
    //                    break;
    //            }
    //        } else {
    //            switch (_parameterValue) {
    //                case "Request":
    //                    _parameterValue = _threadEntity.Page.Request[_parameterNameText];
    //                    break;
    //                case "Url":
    //                    _parameterValue = _threadEntity.Page.Request.QueryString[_parameterNameText];
    //                    break;
    //                case "Form":
    //                    _parameterValue = _threadEntity.Page.Request.Form[_parameterNameText];
    //                    break;
    //                case "Group":
    //                    _parameterValue = _threadEntity.Page.Request.GroupString[_parameterNameText];
    //                    break;
    //                case "Data":
    //                    _parameterValue = _threadEntity.Page.PageData[_parameterNameText].GetDataString();
    //                    break;
    //            }
    //        }
    //        if (string.IsNullOrEmpty(_parameterValue)) {
    //            _isDefaultParameter = true;
    //            _parameterValue = GetDefaultParameter(item.Name);
    //        }
    //        if (_parameterValue.IndexOf('|') > 0 && (_parameterValue.LastIndexOf('|') == _parameterValue.Length - 2)) {
    //            _isCertainType = true;
    //            _parameterType = _parameterValue.Substring(_parameterValue.LastIndexOf('|') + 1)[0];
    //            _parameterValue = _parameterValue.Substring(0, _parameterValue.LastIndexOf('|'));
    //        }

    //        switch (item.Name[0]) {
    //            case '!':
    //                cmd.Parameters.AddWithValue(_parameterName, GetParameterByType(_isCertainType, _isDefaultParameter, _parameterType, _parameterValue)).Direction = System.Data.ParameterDirection.ReturnValue;
    //                break;
    //            case '#':
    //                cmd.Parameters.AddWithValue(_parameterName, GetParameterByType(_isCertainType, _isDefaultParameter, _parameterType, _parameterValue)).Direction = System.Data.ParameterDirection.InputOutput;
    //                break;
    //            default:
    //                cmd.Parameters.AddWithValue(_parameterName, GetParameterByType(_isCertainType, _isDefaultParameter, _parameterType, _parameterValue));
    //                break;
    //        }
    //    }

    //    private string GetDefaultParameter(string name) {
    //        if (_defaultParameter == null) { return string.Empty; }
    //        foreach (TagItem _item in _defaultParameter) {
    //            if (string.Compare(_item.Name, name) == 0) { return _item.Value; }
    //        }
    //        return string.Empty;
    //    }

    //    private object GetParameterByType(bool _isCertainType, bool _isDefaultParameter, char _parameterType, string _parameterValue) {
    //        if (_isCertainType && !_isDefaultParameter) {
    //            switch (_parameterType) {
    //                case 'i':
    //                    return Convert.ToInt32(_parameterValue);
    //            }
    //        }
    //        return _parameterValue;
    //    }

    //    private void LoadXSLT() {
    //        _innerData = new List<byte>();
    //        if (!string.IsNullOrEmpty(_xsltstring)) {
    //            using (System.IO.FileStream fs = new System.IO.FileStream(_threadEntity.WebSetting.XsltDir + _xsltstring, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
    //                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs)) {
    //                    _xsltstring = sr.ReadToEnd();
    //                    //_innerData.AddRange(new List<byte>(_threadEntity.WebSetting.Encoding.GetBytes(_xsltstring)));
    //                    sr.Close();
    //                }
    //                fs.Close();
    //            }
    //        }
    //    }

    //    private System.Xml.Xsl.XslCompiledTransform HandleXSLT() {
    //        System.Xml.Xsl.XslCompiledTransform _xsl = new System.Xml.Xsl.XslCompiledTransform();
    //        StringBuilder xsltsb = new StringBuilder();
            
    //        if (!string.IsNullOrEmpty(_xsltstring)) {
    //            xsltsb.Append(_xsltstring);
    //        } else {
    //            if (!_defaultTemplate) {
    //                xsltsb.Append("<?xml version=\"1.0\" encoding=\"").Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\"?>")
    //                    .Append("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">")
    //                    .Append("<xsl:output method=\"html\" omit-xml-declaration=\"yes\" version=\"1.0\" encoding=\"")
    //                    .Append(_threadEntity.WebSetting.Encoding.BodyName).Append("\" />");
    //                if (!_defaultRoot) {
    //                    if (!string.IsNullOrEmpty(_root)) {
    //                        xsltsb.Append(string.Format("<xsl:template match=\"{0}\">", _root));
    //                    } else {
    //                        xsltsb.Append("<xsl:template match=\"DataTable/DataItem\">");
    //                    }
    //                } else { xsltsb.Append("<xsl:template match=\"/\">"); }
    //            }
    //            xsltsb.Append(_threadEntity.WebSetting.Encoding.GetString(_innerData.ToArray()));
    //            if (!_defaultTemplate) {
    //                xsltsb.Append("</xsl:template>");
    //                xsltsb.Append("</xsl:stylesheet>");
    //            }
    //        }
    //        if (_paramereturns.Count > 0) {
    //            StringBuilder xsltParametersb = new StringBuilder();
    //            foreach (string _key in _paramereturns.Keys) {
    //                xsltParametersb.Append(string.Format(@"<xsl:variable name=""{0}"">{1}</xsl:variable>", _key, _paramereturns[_key]));
    //            }
    //            xsltParametersb.Append("<xsl:template");
                
    //            xsltsb.Replace("<xsl:template", xsltParametersb.ToString());
    //        }
    //        System.Xml.XmlReader xslcontent = System.Xml.XmlReader.Create(new System.IO.StringReader(xsltsb.ToString()));
    //        _xsl.Load(xslcontent);
    //        return _xsl;
    //    }
    //}
    #endregion

    public class SetDataControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }

        private ThreadEntity _threadEntity;
        private string _dataName;
        private bool _preEnableScript;
        private bool _append;

        public SetDataControl() {
            _isNeedData = true;
            _preEnableScript = false;
        }

        public void Init(System.Collections.Specialized.NameValueCollection createTag, string map, int index) {
            for (int i = 0; i < createTag.Count; i++) {
                switch (createTag.Keys[i]) {
                    case "Name":
                        _dataName = createTag[i];
                        break;
                    case "PreEnableScript":
                        _preEnableScript = string.Compare(createTag[i], "True") == 0;
                        break;
                    case "Append":
                        _append = string.Compare(createTag[i], "True") == 0;
                        break;
                }
            }
            _map = string.Concat(map, "SetDataControl", index);
        }

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            _threadEntity.ControlIndex += 1;
            string _content = string.Empty;
            if (_preEnableScript && _innerData.Length > 0) {
                Control.ControlAnalyze _controls = Cache.PageAnalyze.GetInstance(CurrentThreadEntity, CurrentPageClass, this.Map);
                if (!_controls.IsHandled || CurrentPageClass.WebSetting.DebugMode) {
                    _controls.SetContent(_innerData.ToArray());
                    _controls.Analyze();
                }
                HTMLContainer _temp = new HTMLContainer(_innerData.Encoding);
                _controls.Handle(CurrentPageClass, _temp);
                _content = _temp.ToString();
            } else {
                _content = _innerData.ToString();
            }
            if (_append) {
                string _originalContent = string.Empty;
                if (CurrentPageClass.PageData[_dataName] != null) _originalContent = CurrentPageClass.PageData[_dataName].GetDataString();
                CurrentPageClass.PageData.Add(_dataName, _originalContent + _content);
            } else {
                CurrentPageClass.PageData.Add(_dataName, _content);
            }
            
            _threadEntity.ControlIndex -= 1;
        }

        public string BuildHtmlString() {
            return string.Empty;
        }
    }
}
