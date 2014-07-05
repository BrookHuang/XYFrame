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

            _defaultUrlControl = new URLCollection(string.Empty, string.Empty);
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
            _defaultUrlControl.Add(new URLItem(null, "Xy.Web,Xy.Web.Page.ErrorPage", @"^/error\.aspx", "text/html", false, false, "MainContent", 0));

            _urlControl = new List<URLCollection>();
            foreach (System.Xml.XmlElement _xe in _document.SelectNodes("UrlRewrite/WebSite")) {
                URLCollection _urlItemCollection = new URLCollection(_xe);
                foreach (System.Xml.XmlNode _xn in _xe.SelectNodes("Item")) {
                    _urlItemCollection.Add(new URLItem(_xn));
                }
                _urlItemCollection.AddRange(_defaultUrlControl.ToArray());
                _urlControl.Add(_urlItemCollection);
            }

        }

        internal URLCollection GetUrlItemCollection(string regUrl) {
            if (_urlControl == null) { Init(); }
            for (int i = 0; i < _urlControl.Count; i++) {
                if (_urlControl[i].IsMatch(regUrl)) {
                    return _urlControl[i];
                }
            }
            return null;
        }

        public static void ClearCache() {
            _instance = null;
        }
    }
}
