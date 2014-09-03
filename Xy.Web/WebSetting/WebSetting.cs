using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.WebSetting {
    public class WebSettingItem {
        private string _name;
        public string Name { get { return _name; } }

        private string _inherit;
        private WebSettingItem _inheritInstance = null;
        public WebSettingItem Inherit { get { if (_inheritInstance == null) _inheritInstance = WebSettingCollection.GetWebSetting(_inherit); return _inheritInstance; } }

        private string _root;
        public string Root { get { return _root; } }

        private string _port;
        public string Port { get { return _port; } }
        
        private string _theme;
        public string Theme { get { return _theme; } }

        private Encoding _encoding;
        public Encoding Encoding { get { return _encoding; } }

        private int _sessionOutTime;
        public int SessionOutTime { get { return _sessionOutTime; } }

        private string _encryptKey;
        public string EncryptKey { get { return _encryptKey; } }

        private string _encryptIV;
        public string EncryptIV { get { return _encryptIV; } }

        private string _userKeyCookieName;
        public string UserKeyCookieName { get { return _userKeyCookieName; } }

        private System.Collections.Specialized.NameValueCollection _config;
        public System.Collections.Specialized.NameValueCollection Config { get { return _config; } }

        private System.Collections.Specialized.NameValueCollection _translate;
        public System.Collections.Specialized.NameValueCollection Translate { get { return _translate; } }

        private string _pageDir;
        public string PageDir { get { return _pageDir; } }

        private string _xsltDir;
        public string XsltDir { get { return _xsltDir; } }

        private string _cssDir;
        public string CssDir { get { return _cssDir; } }

        private string _cssPath;
        public string CssPath { get { return _cssPath; } }

        private string _scriptDir;
        public string ScriptDir { get { return _scriptDir; } }

        private string _scriptPath;
        public string ScriptPath { get { return _scriptPath; } }

        private string _includeDir;
        public string IncludeDir { get { return _includeDir; } }

        private string _cacheDir;
        public string CacheDir { get { return _cacheDir; } }

        private bool _debugMode;
        public bool DebugMode { get { return _debugMode; } }

        private bool _compatible;
        public bool Compatible { get { return _compatible; } }

        internal WebSettingItem(System.Xml.XmlNode XMLNode) {
            if (XMLNode.Attributes["Name"] == null) {
                _name = WebSettingCollection.DEFAULTWEBSETTINGNAME;
            } else {
                _name = XMLNode.Attributes["Name"].Value;
            }
            if (XMLNode.Attributes["Inherit"] == null) {
                _inherit = WebSettingCollection.DEFAULTWEBSETTINGNAME;
            }else{
                _inherit = XMLNode.Attributes["Inherit"].Value;
            }
            _config = new System.Collections.Specialized.NameValueCollection();
        }

        private string _themePath;
        internal void Init(System.Xml.XmlNode XMLNode) {

            if (XMLNode.SelectSingleNode("Root") != null) {
                _root = XMLNode.SelectSingleNode("Root").InnerText;
            }

            if (XMLNode.SelectSingleNode("Port") != null) {
                _port = XMLNode.SelectSingleNode("Port").InnerText;
            }

            if (XMLNode.SelectSingleNode("Compatible") != null) {
                _compatible = Convert.ToBoolean(XMLNode.SelectSingleNode("Compatible").InnerText);
            }

            if (XMLNode.SelectSingleNode("Theme") != null) {
                _theme = XMLNode.SelectSingleNode("Theme").InnerText;
                _themePath = _theme + '\\';
            }

            if (XMLNode.SelectSingleNode("SessionOutTime") != null) {
                _sessionOutTime = Convert.ToInt32(XMLNode.SelectSingleNode("SessionOutTime").InnerText);
            }

            if (XMLNode.SelectSingleNode("EncryptKey") != null) {
                _encryptKey = XMLNode.SelectSingleNode("EncryptKey").InnerText;
            }

            if (XMLNode.SelectSingleNode("EncryptIV") != null) {
                _encryptIV = XMLNode.SelectSingleNode("EncryptIV").InnerText;
            }

            if (XMLNode.SelectSingleNode("XySessionId") != null) {
                _userKeyCookieName = XMLNode.SelectSingleNode("XySessionId").InnerText;
            } else {
                _userKeyCookieName = "XyFrameSessionId";
            }

            if (XMLNode.SelectSingleNode("DebugMode") != null) {
                _debugMode = string.Compare(XMLNode.SelectSingleNode("DebugMode").InnerText, "true", true) == 0;
            }

            if (XMLNode.SelectSingleNode("Encoding") != null) {
                switch (XMLNode.SelectSingleNode("Encoding").InnerText.ToLower()) {
                    case "defalut":
                        _encoding = System.Text.Encoding.Default;
                        break;
                    case "gb2312":
                        _encoding = System.Text.Encoding.GetEncoding("GB2312");
                        break;
                    case "ascii":
                        _encoding = System.Text.Encoding.ASCII;
                        break;
                    case "unicode":
                        _encoding = System.Text.Encoding.Unicode;
                        break;
                    case "utf8":
                    case "utf-8":
                    default:
                        _encoding = new System.Text.UTF8Encoding(true);
                        break;
                }
            }

            foreach (System.Xml.XmlNode _xn in XMLNode.SelectNodes("Config/Item")) {
                if (_xn.Attributes["Name"] != null) {
                    _config.Add(_xn.Attributes["Name"].Value, _xn.InnerText);
                }
            }

            _translate = new System.Collections.Specialized.NameValueCollection();
            foreach (System.Xml.XmlNode _xn in XMLNode.SelectNodes("Translate/Item")) {
                if (_xn.Attributes["Name"] != null) {
                    _translate.Add(_xn.Attributes["Name"].Value, _xn.InnerText);
                }
            }
            
            _xsltDir = Xy.AppSetting.ThemeDir + _themePath + Xy.AppSetting.XSLT_PATH;
            _pageDir = Xy.AppSetting.ThemeDir + _themePath + Xy.AppSetting.PAGE_PATH;
            _includeDir = Xy.AppSetting.ThemeDir + _themePath + Xy.AppSetting.INCLUDE_PATH;
            _cacheDir = Xy.AppSetting.CacheDir + _themePath;

            if (!string.IsNullOrEmpty(_config["ScriptPath"])) {
                _scriptPath = _config["ScriptPath"];
            } else {
                _scriptPath = "/Xy_Theme/" + (_themePath + Xy.AppSetting.SCRIPT_PATH).Replace('\\', '/'); ;
            }

            if (!string.IsNullOrEmpty(_config["CssPath"])) {
                _cssPath = _config["CssPath"];
            } else {
                _cssPath = "/Xy_Theme/" + (_themePath + Xy.AppSetting.CSS_PATH).Replace('\\', '/'); ;
            }

            if (!string.IsNullOrEmpty(_config["ScriptDir"])) {
                _scriptDir = _config["ScriptDir"];
            } else {
                _scriptDir = Xy.AppSetting.ThemeDir + _themePath + Xy.AppSetting.SCRIPT_PATH;
            }

            if (!string.IsNullOrEmpty(_config["CssDir"])) {
                _cssDir = _config["CssDir"];
            } else {
                _cssDir = Xy.AppSetting.ThemeDir + _themePath + Xy.AppSetting.CSS_PATH;
            }
        }

        internal void CreateFolders() {
            Xy.Tools.IO.File.ifNotExistsThenCreate(_cssDir);
            Xy.Tools.IO.File.ifNotExistsThenCreate(_scriptDir);
            Xy.Tools.IO.File.ifNotExistsThenCreate(_xsltDir);
            Xy.Tools.IO.File.ifNotExistsThenCreate(_pageDir);
            Xy.Tools.IO.File.ifNotExistsThenCreate(_includeDir);
            Xy.Tools.IO.File.ifNotExistsThenCreate(_cacheDir);
        }

        internal void CopyBase(WebSettingItem WebConfig) {
            _root = WebConfig.Root;
            _port = WebConfig.Port;
            _themePath = WebConfig.Theme;
            _compatible = WebConfig.Compatible;
            _encoding = WebConfig.Encoding;
            _sessionOutTime = WebConfig.SessionOutTime;
            _encryptKey = WebConfig.EncryptKey;
            _encryptIV = WebConfig.EncryptIV;
            _debugMode = WebConfig.DebugMode;
            _userKeyCookieName = WebConfig.UserKeyCookieName;
        }

        internal void CopyConfig(WebSettingItem WebConfig) {
            foreach (string _key in WebConfig.Config.Keys) {
                if (string.IsNullOrEmpty(_config[_key]))
                    _config[_key] = WebConfig.Config[_key];
            }
            if (!string.IsNullOrEmpty(_config["ScriptPath"])) { _scriptPath = _config["ScriptPath"]; }
            if (!string.IsNullOrEmpty(_config["CssPath"])) { _cssPath = _config["CssPath"]; }
            if (!string.IsNullOrEmpty(_config["ScriptDir"])) { _scriptDir = _config["ScriptDir"]; }
            if (!string.IsNullOrEmpty(_config["CssDir"])) { _cssDir = _config["CssDir"]; }
        }
    }
}
