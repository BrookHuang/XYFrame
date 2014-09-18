using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.URLManage {
    public class URLManager {
        private static URLManager _instance;
        private List<URLCollection> _urlControl;
        private URLCollection _defaultUrlControl;

        internal static URLManager GetInstance() {
            if (_instance == null) {
                _instance = new URLManager();
            }
            return _instance;
        }

        private URLManager() {
            Init();
        }

        private void Init() {
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            _document.Load(Xy.Tools.IO.File.foundConfigurationFile("UrlControl", Xy.AppSetting.FILE_EXT));
            System.Xml.XmlNodeList default_xnl = _document.SelectNodes("UrlRewrite/Item");

            _defaultUrlControl = new URLCollection(string.Empty, string.Empty, string.Empty, string.Empty);
            foreach (System.Xml.XmlNode _xn in default_xnl) {
                _defaultUrlControl.Add(new URLItem(_xn));
            }

            _defaultUrlControl.Add(new URLItem(null, null, @".*\.js$", "application/x-javascript", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.css$", "text/css", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.gif$", "image/gif", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.jpg$", "image/jpeg", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.png$", "image/png", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.bmp$", "image/bmp", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.swf$", "application/x-shockwave-flash", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @".*\.ico$", "image/x-icon", false, false, "ResourceFile", URLItem.DEFAULTAGE));
            _defaultUrlControl.Add(new URLItem(null, null, @"^/Xy_(App|Data|Log)/.*", "text/html", false, false, "Prohibit", 0));
            _defaultUrlControl.Add(new URLItem(null, null, @"^/Xy_Theme/[^/]+/Xy_(Cache|Include|Page|Xslt)/.*", "text/html", false, false, "Prohibit", 0));
            _defaultUrlControl.Add(new URLItem(null, "Xy.Web,Xy.Web.Page.ErrorPage", @"^/error\.aspx", "text/html", true, false, "MainContent", 0));

            _urlControl = new List<URLCollection>();
            foreach (System.Xml.XmlElement _xe in _document.SelectNodes("UrlRewrite/UrlCollection")) {
                URLCollection _urlItemCollection = new URLCollection(_xe);
                if (_urlItemCollection.SiteUrlReg == null && string.IsNullOrEmpty(_urlItemCollection.Name)) continue;
                
                foreach (System.Xml.XmlNode _xn in _xe.SelectNodes("Item")) {
                    _urlItemCollection.Add(new URLItem(_xn));
                }
                URLCollection _parent = null;
                if (!string.IsNullOrEmpty(_urlItemCollection.Inherit)) {
                    _parent = GetUrlItemCollection(_urlItemCollection.Inherit);
                }
                if (_parent == null) _parent = _defaultUrlControl;
                _urlItemCollection.AddRange(_parent.ToArray()); 
                _urlControl.Add(_urlItemCollection);
            }

        }

        internal URLCollection GetUrlItemCollection(string name) {
            if (_urlControl == null) return null;
            for (int i = 0; i < _urlControl.Count; i++) {
                if (string.Compare(name, _urlControl[i].Name, true) == 0) {
                    return _urlControl[i];
                }
            }
            return null;
        }

        internal URLCollection GetUrlItemCollection(Xy.Tools.Web.UrlAnalyzer regUrl) {
            if (_urlControl == null) { Init(); }
            for (int i = 0; i < _urlControl.Count; i++) {
                if (_urlControl[i].IsMatch(regUrl.Site)) {
                    URLCollection _current = _urlControl[i];
                    if (!string.IsNullOrEmpty(_current.WebConfig.Root)){
                        if (!regUrl.HasRoot(_current.WebConfig.Root)) {
                            continue;
                        }
                    }
                    return _current;
                }
            }
            return null;
        }

        public static void ClearCache() {
            _instance = null;
        }
    }
}
