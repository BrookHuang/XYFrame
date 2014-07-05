using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data {

    //Parameter="{ UserID='Data:CurrentUser.ID|i' }"
    //Parameter="{ ClassID='Group|i' PageIndex='Url|i' PageSize='10|i' #TotalCount='-1|i' }" DefaultParameter="{ PageIndex='0|i' ClassID='-1|i' }"
    public class DataParameter {

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

            value = Xy.Tools.Control.Tag.TransferValue(_parameterName, value, CurrentPageClass);
            if (!string.IsNullOrEmpty(value)) _parameterValue = value;
        }

    }

    //<% @Data Provide="Data" Name="IsFix" EnableScript="True" DefaultRoot="True" %>
    //<% @Data Provide="Data" Name="Pages" DefaultRoot="True" EnableScript="True" %>
    //<% @Data Provide="ADO" Command="select a.[ID],a.[Name],a.[Nickname],a.[Email],b.[Name] as 'GroupName' from [User] a left join [UserGroup] b on a.UserGroup = b.ID " EnableScript="True" %>
    //<% @Data Provide="ADO" Command="select [Nickname],[Description],[Sex],[Hometown],[Birthday] from [User] left join [UserExtra] on [User].ID = [UserExtra].UserID where [User].ID = @UserID" Parameter="{ UserID='Data:CurrentUser.ID|i' }" %>
    //<% @Data Provide="ADO" Procedure="ClickJia_Post_PostGroup_GetList" Parameter="{ PageIndex='' PageSize='21' OrderBy='[Member] Desc' }" Xslt="group.xslt" EnableScript="True" %>
    //<% @Data Provide="ADO" Procedure="XiaoYang_Article_Article_GetList" Parameter="{ ClassID='Group|i' PageIndex='Url|i' PageSize='10|i' #TotalCount='-1|i' }" DefaultParameter="{ PageIndex='0|i' ClassID='-1|i' }" DefaultRoot="True" EnableScript="True" Xslt="ArticleList.xslt" %>
    //<% @Data Provide="ADO" Procedure="ClickJia_Message_Message_GetList" Parameter="{ UserID='Data:CurrentUser.ID|i' PageIndex='0' }" EnableScript="True" %>
    public class DataParameterCollection : Dictionary<string, DataParameter> {

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
}
