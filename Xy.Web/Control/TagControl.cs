using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public class TagControl : Xy.Web.Control.IControl {
        private string _map;
        public string Map { get { return _map; } set { _map = value; } }
        private bool _isNeedData;
        public bool IsNeedData { get { return _isNeedData; } }
        private HTMLContainer _innerData;
        public HTMLContainer InnerData { get { return _innerData; } set { _innerData = value; } }

        private enum TagMode { Get, Exist, Compare, NotExist }
        private enum CompareMode { Equal, Greater, Lesser, GreaterAndEqual, LesserAndEqual, NotEqual }
        private delegate void _fillContent(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer);
        private _fillContent _fillContentDelegate = null;

        private ThreadEntity _threadEntity;
        private string _settingClass;
        private string _settingProperty;
        private string _defaultValue;
        private bool _enableScript;
        private bool _useInnerTag;
        private TagMode _mode;
        private CompareMode _compareMode;
        private string _compareValue;
        private string _configName;
        private Xy.WebSetting.WebSettingItem _webConfig = null;
        


        public TagControl() {
            _isNeedData = false;
            _enableScript = false;
            _useInnerTag = false;
            _mode = TagMode.Get;
        }

        public void Init(System.Collections.Specialized.NameValueCollection createTag,string map, int index) {
            for (int i = 0; i < createTag.Count; i++) {
                switch (createTag.Keys[i]) {
                    case "Provider":
                        _settingClass = createTag[i];
                        break;
                    case "Name":
                        _settingProperty = createTag[i];
                        break;
                    case "Default":
                        _defaultValue = createTag[i];
                        break;
                    case "EnableScript":
                        _enableScript = string.Compare("true", createTag[i], true) == 0;
                        break;
                    case "UseInnerTag":
                        _useInnerTag = string.Compare("true", createTag[i], true) == 0;
                        break;
                    case "Mode":
                        switch (createTag[i]) {
                            case "Get":
                                _mode = TagMode.Get;
                                break;
                            case "Exist":
                                _mode = TagMode.Exist;
                                _isNeedData = true;
                                break;
                            case "Compare":
                                _mode = TagMode.Compare;
                                _isNeedData = true;
                                break;
                            case "NotExist":
                                _mode = TagMode.NotExist;
                                _isNeedData = true;
                                break;
                        }
                        break;
                    case "CompareValue":
                        _compareValue = createTag[i];
                        break;
                    case "CompareMethod":
                        switch (createTag[i]) {
                            case "Equal":
                            case "=":
                                _compareMode = CompareMode.Equal;
                                break;
                            case "NotEqual":
                            case "!=":
                                _compareMode = CompareMode.NotEqual;
                                break;
                            case "Greater":
                            case ">":
                                _compareMode = CompareMode.Greater;
                                break;
                            case "Lesser":
                            case "<":
                                _compareMode = CompareMode.Lesser;
                                break;
                            case "GreaterAndEqual":
                            case ">=":
                                _compareMode = CompareMode.GreaterAndEqual;
                                break;
                            case "LesserAndEqual":
                            case "<=":
                                _compareMode = CompareMode.LesserAndEqual;
                                break;
                        }
                        break;
                    case "Config":
                        _configName = createTag[i];
                        break;
                }
            }
            _map = string.Concat(map, "TagControl", index);
            if (!string.IsNullOrEmpty(_configName)) {
                _webConfig = Xy.WebSetting.WebSettingCollection.GetWebSetting(_configName);
            }
            _fillContentDelegate = initDelegate();
        }

        #region delegateInit
        private _fillContent initDelegate() {
            switch (_settingClass) {
                case "App":
                    switch (_settingProperty) {
                        case "Domain": return new _fillContent(delegate_App_Domain);
                        case "CssPath": return new _fillContent(delegate_App_CssPath);
                        case "ScriptPath": return new _fillContent(delegate_App_ScriptPath);
                        case "Config": return new _fillContent(delegate_App_ScriptPath);
                    }
                    break;
                case "Config":
                    switch (_settingProperty) {
                        case "CssPath": return new _fillContent(delegate_Config_CssPath);
                        case "ScriptPath": return new _fillContent(delegate_Config_ScriptPath);
                        case "DebugMode": return new _fillContent(delegate_Config_DebugMode);
                        default: return new _fillContent(delegate_Config);
                    }
                case "Translate": return new _fillContent(delegate_Translate);
                case "Theme":
                    switch (_settingProperty) {
                        case "Name": return new _fillContent(delegate_Theme_Name);
                        case "Template": return new _fillContent(delegate_Theme_Template);
                    }
                    break;
                case "Page":
                    switch (_settingProperty) {
                        case "Url": return new _fillContent(delegate_Page_Url);
                        case "UrlPath": return new _fillContent(delegate_Page_UrlPath);
                    }
                    break;
                case "Session":
                    return new _fillContent(delegate_Session);
                case "Url":
                    return new _fillContent(delegate_Url);
                case "Form":
                    return new _fillContent(delegate_Form);
                case "Group":
                    return new _fillContent(delegate_Group);
                case "Data":
                    return new _fillContent(delegate_Data);
            }
            throw new Exception(string.Format("Invalied Provider({1}) or Name({2}) on TagControl:{0}", _map, _settingClass, _settingProperty));
        }

        private void delegate_App_Domain(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentPageClass.URL.Site);
        }
        private void delegate_App_CssPath(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentPageClass.URL.Site + _webConfig.CssPath);
        }
        private void delegate_App_ScriptPath(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentPageClass.URL.Site + _webConfig.ScriptPath);
        }
        private void delegate_App_Config(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(_webConfig.Name);
        }
        private void delegate_Config_CssPath(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(_webConfig.CssPath);
        }
        private void delegate_Config_ScriptPath(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(_webConfig.ScriptPath);
        }
        private void delegate_Config_DebugMode(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(_webConfig.DebugMode ? "True" : "False");
        }
        private void delegate_Config(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = _webConfig.Config[name];
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Translate(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = _webConfig.Translate[name];
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Theme_Name(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(_webConfig.Theme);
        }
        private void delegate_Theme_Template(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentThreadEntity.URLItem.PagePath);
        }
        private void delegate_Page_Url(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentPageClass.URL.ToString());
        }
        private void delegate_Page_UrlPath(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            contentContainer.Write(currentPageClass.URL.Path);
        }
        private void delegate_Session(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = Convert.ToString(currentPageClass.Session[name]);
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Url(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = currentPageClass.Request.QueryString[name];
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Form(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = currentPageClass.Request.Form[name];
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Group(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            string temp = currentPageClass.Request.GroupString[name];
            if (!string.IsNullOrEmpty(temp)) {
                contentContainer.Write(temp);
            }
        }
        private void delegate_Data(string name, ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            Page.PageDataItem pdi = currentPageClass.PageData[name];
            if (pdi != null) {
                contentContainer.Write(pdi.GetDataString());
            }
        }
        #endregion

        public void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer ContentContainer) {
            _threadEntity = CurrentThreadEntity;
            if (_webConfig == null)
                _webConfig = CurrentPageClass.WebSetting;
            _threadEntity.ControlIndex += 1;
            HTMLContainer _innerHTML = new HTMLContainer(ContentContainer.Encoding);
            _fillContentDelegate(_settingProperty, CurrentThreadEntity, CurrentPageClass, _innerHTML);

            switch (_mode) {
                case TagMode.Get:
                    break;
                case TagMode.Compare:
                    bool _passed = false;
                    string _dataValue = _innerHTML.ToString();
                    _innerHTML.Clear();
                    switch (_compareMode) {
                        case CompareMode.Equal:
                            if (string.Compare(_dataValue, _compareValue) == 0) _passed = true;
                            break;
                        case CompareMode.NotEqual:
                            if (string.Compare(_dataValue, _compareValue) != 0) _passed = true;
                            break;
                        case CompareMode.Greater:
                            if (string.Compare(_dataValue, _compareValue) > 0) _passed = true;
                            break;
                        case CompareMode.Lesser:
                            if (string.Compare(_dataValue, _compareValue) < 0) _passed = true;
                            break;
                        case CompareMode.GreaterAndEqual:
                            if (string.Compare(_dataValue, _compareValue) >= 0) _passed = true;
                            break;
                        case CompareMode.LesserAndEqual:
                            if (string.Compare(_dataValue, _compareValue) <= 0) _passed = true;
                            break;
                    }
                    if (_passed) {
                        _innerHTML.Write(_innerData);
                    }
                    break;
                case TagMode.Exist:
                    if (_innerHTML.Length > 0) {
                        _innerHTML.Clear();
                        _innerHTML.Write(_innerData);
                    }
                    break;
                case TagMode.NotExist:
                    if (_innerHTML.Length > 0) {
                        _innerHTML.Clear();
                    } else {
                        _innerHTML.Write(_innerData);
                    }
                    break;
            }

            if (_innerHTML.Length == 0 && !string.IsNullOrEmpty(_defaultValue)) {
                ContentContainer.Write(_defaultValue);
            } else {
                if (_enableScript) {
                    Control.ControlAnalyze _controls = new ControlAnalyze(CurrentThreadEntity, this.Map, _useInnerTag);
                    _controls.SetContent(_innerHTML.ToArray());
                    _controls.Analyze();
                    _controls.Handle(CurrentPageClass, ContentContainer);
                } else {
                    ContentContainer.Write(_innerHTML);
                }
            }
            _threadEntity.ControlIndex -= 1;
        }
    }
}
