using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xy.Web.URLManage {
    public class URLCollection : List<URLItem> {
        private string _webConfigName;
        public string WebConfigName { get { return _webConfigName; } }

        private WebSetting.WebSettingItem _webConfig;
        public WebSetting.WebSettingItem WebConfig { get { return _webConfig; } }

        private System.Text.RegularExpressions.Regex _siteUrlReg;
        public Regex SiteUrlReg { get { return _siteUrlReg; } }

        internal URLCollection(System.Xml.XmlElement URLXml) :
            this(
                URLXml.Attributes["WebConfig"] == null ?
                    string.Empty : URLXml.Attributes["WebConfig"].Value,
                URLXml.Attributes["SiteUrlReg"].Value
            ) { }

        internal URLCollection(string webConfigName, string siteUrlRegex) {
            _webConfigName = webConfigName;
            _webConfig = Xy.WebSetting.WebSettingCollection.GetWebSetting(_webConfigName);
            if (!string.IsNullOrEmpty(siteUrlRegex)) {
                _siteUrlReg = new Regex(siteUrlRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }

        public bool IsMatch(string url) {
            if (_siteUrlReg == null) return false;
            return _siteUrlReg.IsMatch(url);
        }

        public URLItem GetUrlItem(string regUrl) {
            for (int i = 0; i < this.Count; i++) {
                if (this[i].IsMatch(regUrl)) {
                    return this[i];
                }
            }
            return null;
        }
    }
}
