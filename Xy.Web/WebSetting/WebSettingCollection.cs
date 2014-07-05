using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.WebSetting {
    public class WebSettingCollection {
        private static System.Collections.Generic.Dictionary<string, WebSettingItem> _instances;
        private static WebSettingItem _defaultItem = null;
        public const string DEFAULTWEBSETTINGNAME = "default";

        private static string[] _configNameList = null;
        public static string[] ConfigNameList { get { return _configNameList; } }

        static WebSettingCollection() {
            Init();
        }

        public static WebSettingItem GetWebSetting(string name) {
            if (string.IsNullOrEmpty(name)) name = DEFAULTWEBSETTINGNAME;
            if (_instances.ContainsKey(name)) {
                return _instances[name];
            } else {
                throw new Exception(string.Format("Can not found WebSetting by name:\'{0}\'", name));
            }
        }

        private static void Init() {
            _instances = new Dictionary<string, WebSettingItem>();
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            List<string> _tempConfigNameList = new List<string>();
            _document.Load(Xy.Tools.IO.File.foundConfigurationFile("Web", Xy.AppSetting.FILE_EXT));
            System.Xml.XmlNodeList _webSettings = _document.GetElementsByTagName("WebSetting");
            for (int i = 0; i < _webSettings.Count; i++) {
                System.Xml.XmlNode _item = _webSettings[i];
                WebSettingItem _ws = new WebSettingItem(_item);
                if (string.Compare(_ws.Name, DEFAULTWEBSETTINGNAME, true) == 0) {
                    WebSettingItem _defaultWS = InitDefaultConfig();
                    _ws.CopyBase(_defaultWS);
                    _ws.CopyConfig(_defaultWS);
                    _ws.Init(_item);
                    _ws.CreateFolders();
                    _instances.Add(_ws.Name, _ws);
                    _defaultItem = _ws;
                    break;
                }
            }
            if (_defaultItem == null) _defaultItem = InitDefaultConfig();
            _tempConfigNameList.Add(DEFAULTWEBSETTINGNAME);
            for (int i = 0; i < _webSettings.Count; i++) {
                System.Xml.XmlNode _item = _webSettings[i];
                WebSettingItem _ws = new WebSettingItem(_item);
                if (string.Compare(_ws.Name, DEFAULTWEBSETTINGNAME, true) != 0) {
                    _ws.CopyBase(_defaultItem);
                    _ws.CopyConfig(_defaultItem);
                    _ws.Init(_item);
                    _ws.CreateFolders();
                    _instances.Add(_ws.Name, _ws);
                    _tempConfigNameList.Add(_ws.Name);
                }
            }
            _configNameList = _tempConfigNameList.ToArray();
        }

        private static WebSettingItem InitDefaultConfig() {
            WebSettingItem _default;
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            //<SiteUrl>self</SiteUrl>
            _document.LoadXml(@"<WebSettingCollection>
                                    <WebSetting>
                                        <Theme>default</Theme>
                                        <Encoding>UTF-8</Encoding>
                                        <SessionOutTime>30</SessionOutTime>
                                        <EncryptKey>THISISXYFRAMEENCRYPTKEY</EncryptKey>
                                        <EncryptIV>VITPYRCNEEMARYXSISIHT</EncryptIV>
                                        <XySessionId>XyFrameSessionId</XySessionId>
                                        <DebugMode>False</DebugMode>
                                    </WebSetting>
                                </WebSettingCollection>");
            System.Xml.XmlNode _node = _document.GetElementsByTagName("WebSetting")[0];
            _default = new WebSettingItem(_node);
            _default.Init(_node);
            return _default;
        }

        public static void Clear() {
            Init();
        }
    }
}
